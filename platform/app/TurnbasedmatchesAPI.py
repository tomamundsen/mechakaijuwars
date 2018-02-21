import time

from flask import Blueprint, jsonify, request, abort

from lib.Server.Utils import matchDatabase, is_valid_token, get_username_from_token, get_id_from_match,\
    get_match_id, GAMEHOST_ENDPOINT, crossdomain, MY_POST, get_deck_id
from lib.Server.Deck import Deck
from lib.Server.CardPile import CardPile
from lib.Server.TurnBasedMatchResource import TurnBasedMatchResource

MATCH_AUTO_MATCHING = 'MATCH_AUTO_MATCHING'
MATCH_ACTIVE = 'MATCH_ACTIVE'
MATCH_COMPLETE = 'MATCH_COMPLETE'
MATCH_CANCELLED = 'MATCH_CANCELLED'
MATCH_EXPIRED = 'MATCH_EXPIRED'
MATCH_DELETED = 'MATCH_DELETED'

USER_INVITED = 'USER_INVITED'
USER_AWAITING_TURN = 'USER_AWAITING_TURN'
USER_TURN = 'USER_TURN'
USER_MATCH_COMPLETED = 'USER_MATCH_COMPLETED'

PARTICIPANT_NOT_INVITED_YET = 'PARTICIPANT_NOT_INVITED_YET'
PARTICIPANT_INVITED = 'PARTICIPANT_INVITED'
PARTICIPANT_JOINED = 'PARTICIPANT_JOINED'
PARTICIPANT_DECLINED = 'PARTICIPANT_DECLINED'
PARTICIPANT_LEFT = 'PARTICIPANT_LEFT'
PARTICIPANT_FINISHED = 'PARTICIPANT_FINISHED'
PARTICIPANT_UNRESPONSIVE = 'PARTICIPANT_UNRESPONSIVE'

MATCH_RESULT_WIN = 'MATCH_RESULT_WIN'
MATCH_RESULT_LOSS = 'MATCH_RESULT_LOSS'
MATCH_RESULT_TIE = 'MATCH_RESULT_TIE'
MATCH_RESULT_NONE = 'MATCH_RESULT_NONE'
MATCH_RESULT_DISCONNECT = 'MATCH_RESULT_DISCONNECT'
MATCH_RESULT_DISAGREED = 'MATCH_RESULT_DISAGREED'

def is_valid_match(match):
    if match['status'] == unicode(MATCH_COMPLETE) or \
        match['status'] == unicode(MATCH_CANCELLED) or \
        match['status'] == unicode(MATCH_EXPIRED) or \
        match['status'] == unicode(MATCH_DELETED):
         return False
    return True

turnbasedmatches_api = Blueprint('turnbasedmatches_api', __name__)

@turnbasedmatches_api.route('/turnbasedmatches/<match_id>/turn', methods = ['POST'])
@crossdomain()
def _take_turn(match_id):
    try:
        url = GAMEHOST_ENDPOINT + match_id + '/turn'
        data = request.get_json()

        if len(data) < 2:
            return jsonify({'success': False, 'errors': {'message': 'missing access token'}})

        access_token = data['access_token']

        if not is_valid_token(access_token):
            return jsonify({'success': False, 'errors': {'message': 'invalid accesss token'}})

        match = matchDatabase.get(get_id_from_match(match_id)).value
        if not is_valid_match(match):
            return jsonify({'success': False, 'errors': {'message': 'cannot take turn. match ' + match_id + ' has been cancelled.'}})

        resp = MY_POST(url, data)

        return resp
    except Exception, e:
        return jsonify({'success': False, 'errors': {'message': str(e)}})

@turnbasedmatches_api.route('/turnbasedmatches/<match_id>', methods=['GET'])
@crossdomain()
def _get_match_info(match_id):
    try:
        token = request.args.get('access_token')
        username = get_username_from_token(token)
        if is_valid_token(token):
            try:
                result = matchDatabase.get(get_id_from_match(match_id)).value
                for k in result['data']['data'][username]:
                    result['data']['data'][k] = result['data']['data'][username][k]
                return jsonify({'success': True, 'results': {'match': result}})
            except Exception, e:
                return jsonify({'success': False, 'errors': {'message': str(e)}})
    except Exception, e:
        return jsonify({'success': False, 'errors': {'message': str(e)}})

@turnbasedmatches_api.route('/turnbasedmatches', methods=['GET'])
@crossdomain()
def _list_matches():
    try:
        token = request.args.get('access_token')
        if is_valid_token(token):
            results = []
            username = get_username_from_token(token)

            i = 0
            try:
                current = int(matchDatabase.get('current').value)
            except Exception, e:
                current = 0

            while i <= current:
                try:
                    match = matchDatabase.get(str(i)).value
                    participants = match['participants']
                    for p in participants:
                        if p['player']['participantId'] == username:
                            results.append(match)
                except Exception, e:
                    pass
                i += 1

            try:
                json_result = jsonify({'success': True, 'results': {'matches': results}})
            except Exception, e:
                return jsonify({'success': False, 'errors': {'message': str(e)}})
            return json_result
        else:
            return jsonify({'errors':{'message': 'invalid access token'}})
    except Exception, e:
        return jsonify({'success': False, 'errors': {'message': str(e)}})

@turnbasedmatches_api.route('/turnbasedmatches/create', methods = ['POST'])
@crossdomain()
def _create_match():
    data = request.get_json()

    if 'access_token' not in data or not is_valid_token(data['access_token']):
        abort(400)

    username = get_username_from_token(data['access_token'])

    deck_id = None
    if 'deck_id' not in data or not data['deck_id']:
        return jsonify({'success': False, 'errors': {'message': 'no deck_id provided'}})
    else:
        deck_id = get_deck_id(username, data['deck_id'])

    if not deck_id:
        return jsonify({'success': False, 'errors': {'message': 'invalid deck_id'}})

    # todo: setup player's game data: hand, drawpile, discardpile, etc.
    deck = Deck(username, deck_id)
    draw_pile = CardPile(deck)
    draw_pile.shuffle()
    hand = CardPile()
    discard_pile = CardPile()

    for i in range(0,5):
        hand.push(draw_pile.pop())

    next = 0
    try:
        current = int(matchDatabase.get('current').value)
    except Exception, e:
        matchDatabase.set('current', str(0))
        current = int(matchDatabase.get('current').value)

    while True:
        try:
            match = matchDatabase.get(str(next)).value
        except Exception, e:
            break

        try:
            participants = match['participants']
            theparticipant = participants[0]
            the_player = theparticipant['player']
            theparticipantid = the_player['participantId']
            thematchstatus = match['status']
            if participants[0]['player']['participantId'] != username and \
                match['status'] == MATCH_AUTO_MATCHING:
                participants.append({
                    'kind': 'games#turnBasedMatchParticipant',
                    'id': 'player2',
                    'player': {
                        'kind' : 'games#turnBasedMatchParticipant',
                        'participantId': username,
                        'avatarImageUrl': None
                    }
                })
                match['status'] = MATCH_ACTIVE
                match['data']['data'][username] = {}
                match['data']['data'][username]['mana'] = 1
                match['data']['data'][username]['hand'] = hand.cards
                match['data']['data'][username]['draw_pile'] = draw_pile.cards
                match['data']['data'][username]['discard_pile'] = discard_pile.cards
                matchDatabase.set(str(next), match)
                return jsonify({'success': True, 'results': {'match': match}})
            next += 1
        except Exception, e:
            import sys
            sys.exit()

    match_id = get_match_id(next)
    turnbased_match = TurnBasedMatchResource.fromstring(match_id, username, hand, draw_pile, discard_pile)

    try:
        t = turnbased_match.get_json()
        matchDatabase.add(str(next), t)
        matchDatabase.set('current', str(next))
    except Exception, e:
        return jsonify({'success': False, 'errors': {'message': 'COUCHBASE: ' + str(e)}})
    the_match = matchDatabase.get(str(next))
    return jsonify({'success': True, 'results': {'match': the_match.value}})

@turnbasedmatches_api.route('/turnbasedmatches/<match_id>/cancel', methods = ['PUT'])
@crossdomain()
def _cancel_match(match_id):
    data = request.get_json()
    if len(data) < 1:
        return jsonify({'success': False, 'errors': {'message': 'missing access_token'}})

    access_token = data['access_token']

    if not is_valid_token(access_token):
        return jsonify({'success': False, 'errors': {'message': 'invalid token'}})

    match = matchDatabase.get(get_id_from_match(match_id)).value

    if not is_valid_match(match):
        return jsonify({'success': False, 'errors': {'message': 'cannot take turn. match ' + match_id + ' has status ' + match['status']}})

    try:
        match = matchDatabase.get(get_id_from_match(match_id)).value
        match['status'] = MATCH_CANCELLED

    except Exception, e:
        return jsonify({'success': False, 'errors': {'message': str(e)}})

    try:
        results = []

        for p in match['participants']:
            username = get_username_from_token(access_token)
            participantId = p['player']['participantId']
            result = ''
            placing = ''
            if participantId == username:
                if (len(match['participants']) == 1):
                    result = MATCH_RESULT_NONE
                    placing = 1
                elif (len(match['participants']) == 2):
                    result = MATCH_RESULT_LOSS
                    placing = 2
                else:
                    result = MATCH_RESULT_LOSS
                    placing = int(len(match['participants']))
                    print 'WARN!!! cancelling a match with more than 2 players'
            else:
                if (len(match['participants']) == 1):
                    result = MATCH_RESULT_WIN
                    placing = 1
                    print 'WARN!!! canceling a match for someone other than the player'
                elif (len(match['participants']) == 2):
                    result = MATCH_RESULT_WIN
                    placing = 1
                else:
                    result = MATCH_RESULT_NONE
                    placing = -1
                    print 'WARN!!! cancelling a match with more than 2 players. unknown placing and results'
            results.append({'kind': 'games#participantResult', 'participantId': participantId, 'result': result, 'placing': placing})
        match['results'] = results
    except Exception, e:
        return jsonify({'success': False, 'errors': {'message': str(e)}})

    try:
        matchDatabase.set(str(match_id), match)
    except Exception, e:
        return jsonify({'success': False, 'errors': {'message': str(e)}})

    return jsonify({'success': True, 'results': {'match': matchDatabase.get(match_id).value}})
