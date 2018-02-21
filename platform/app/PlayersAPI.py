from flask import Blueprint, request, jsonify

from lib.Server.Utils import crossdomain, is_valid_token, deckDatabase, get_username_from_token


players_api = Blueprint('players_api', __name__)

@players_api.route('/players/<player_id>/categories/decks', methods = ['GET'])
@crossdomain()
def _list_decks(player_id):
    token = request.args.get('access_token')
    if token is not None:
        if is_valid_token(token):
            try:
                username = get_username_from_token(token)
                decks = deckDatabase.get(username).value
                i = 0
                return jsonify({'success': True, 'results': decks})
            except Exception, e:
                return jsonify({'success': False, 'errors': {'message': str(e)}})
        else:
            return jsonify({'success': False, 'errors':{'message':'invalid access token'}})
    else:
        return jsonify({'success': False, 'errors': {'message': 'no access token provided'}})

@players_api.route('/players/<player_id>/categories/decks/<deck_id>', methods = ['GET'])
@crossdomain()
def _get_deck_info(player_id, deck_id):
    token = request.args.get('access_token')
    if token is not None:
        if is_valid_token(token):
            try:
                pass
                # deck = deckDatabase.get(deck_id).value
            except Exception, e:
                return jsonify({'success': False, 'errors': {'message': str(e)}})
        else:
            return jsonify({'success': False, 'errors':{'message':'invalid access token'}})
    else:
        return jsonify({'success': False, 'errors': {'message': 'no access token provided'}})

