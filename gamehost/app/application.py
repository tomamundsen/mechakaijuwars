
from flask import Flask, request, session, g, redirect, url_for, \
     abort, render_template, flash, make_response, current_app, jsonify
import json
from datetime import timedelta
from functools import update_wrapper
from couchbase import Couchbase

from communications.ActionFactory import ActionFactory as ActionFactory
from communications.Action import Action as Action

from lib.Server.TurnBasedMatchResource import TurnBasedMatchResource

######################################################################
##
## constants
##
##
######################################################################

PLATFORM_ENDPOINT = 'http://localhost:10008/'

######################################################################
##
## framework
##
##
######################################################################

application = Flask(__name__)
application.config.from_object(__name__)
application.config.from_envvar('GAMEHOST_SETTINGS', silent=True)

matchDatabase = Couchbase.connect(bucket='match', host='localhost')

# @app.before_request
# def before_request():
#   g.db = connect_db()

# @app.teardown_request
# def teardown_request(exception):
#   db = getattr(g, 'db', None)
#   if db is not None:
#       db.close()

def crossdomain(origin=['*'], methods=None, headers=['Content-Type','Origin','Accept'],
                max_age=21600, attach_to_all=True,
                automatic_options=True):
    if methods is not None:
        methods = ', '.join(sorted(x.upper() for x in methods))
    if headers is not None and not isinstance(headers, basestring):
        headers = ', '.join(x.upper() for x in headers)
    if not isinstance(origin, basestring):
        origin = ', '.join(origin)
    if isinstance(max_age, timedelta):
        max_age = max_age.total_seconds()

    def get_methods():
        if methods is not None:
            return methods

        options_resp = current_app.make_default_options_response()
        return options_resp.headers['allow']

    def decorator(f):
        def wrapped_function(*args, **kwargs):
            if automatic_options and request.method == 'OPTIONS':
                resp = current_app.make_default_options_response()
            else:
                resp = make_response(f(*args, **kwargs))
            if not attach_to_all and request.method != 'OPTIONS':
                return resp

            h = resp.headers

            h['Access-Control-Allow-Origin'] = origin
            h['Access-Control-Allow-Methods'] = get_methods()
            h['Access-Control-Max-Age'] = str(max_age)
            if headers is not None:
                h['Access-Control-Allow-Headers'] = headers
            return resp

        f.provide_automatic_options = False
        return update_wrapper(wrapped_function, f)
    return decorator

######################################################################
##
## helper functions
##
##
######################################################################

def MY_GET(url, headers=None):
    try:
        import urllib2
        import json

        # if headers is None:
        #     headers = {'Content-Type': 'application/json'}
        print 'GET: ' + url
        req = urllib2.Request(url)#, headers=headers)
        conn = urllib2.urlopen(req, timeout=10)
        resp = conn.read()
        conn.close()
        print 'Response: ' + resp
        return resp
    except Exception, e:
        print e

def MY_POST(url, data=None, headers=None):
    try:
        import urllib2
        import json

        print 'POST: ' + url

        if headers is None:
            headers = {'Content-Type': 'application/json'}
        req = urllib2.Request(url, json.dumps(data), headers=headers)
        conn = urllib2.urlopen(req, timeout=10)
        resp = conn.read()
        conn.close()
        print 'Response: ' + resp
        return resp
    except Exception, e:
        print e

def get_id_from_match(match_id):
    print match_id[6:-2]
    return match_id[6:-2]

######################################################################
##
## API
##
##
######################################################################

@application.route('/<match_id>/turn', methods = ['POST'])
@crossdomain()
def _take_turn(match_id):
    try:
        ###################################################
        #
        # Extract data from request body
        #
        ###################################################

        data = request.get_json()

        if len(data) < 2:
            abort(400)

        if 'access_token' not in data:
            abort(400)
        access_token = data['access_token']

        if 'Actions' not in data:
            return jsonify({'success': False, 'errors':{'message':'no Actions supplied'}})

        actions = []

        my_factory = ActionFactory()

        for a in data['Actions']:
            actions.append(my_factory.make_action(a))

        ###################################################
        #
        # Authorization
        #
        ###################################################

        if access_token is None:
            abort(401)
        query = PLATFORM_ENDPOINT + 'users/me?access_token=' + access_token
        user_data = json.loads(MY_GET(query))

        if not user_data:
            abort(401)

        username = user_data['results']['data']['username']

        ###################################################
        #
        # Get turnBasedMatchResource from Database
        #
        ###################################################

        try:
            match = matchDatabase.get(get_id_from_match(match_id)).value
            turn_based_match_resource = TurnBasedMatchResource.fromdict(match)
        except Exception, e:
            return jsonify({'success': False, 'errors': {'message': str(e)}})

        ###################################################
        #
        # Validate Input
        #
        ###################################################

        for a in actions:
            if a.Type == 'Summon':
                # check cost vs. mana
                # check card not in hand
                # check target validity
                pass
            elif a.Type == 'Move':
                # check unit existence
                # check summoning sickness
                # check origin
                # check destination
                # check unit status
                pass
            elif a.Type == 'Attack':
                # check unit existence
                # check summoning sickness
                # check target(s) validity
                # check unit status
                pass
            elif a.Type == 'ActivatedAbility':
                # check unit existence
                # check summoning sickness
                # check unit status
                pass
            elif a.Type == 'Spell':
                # check card not in hand
                # check cost vs. mana
                # check target(s) validity
                pass
            elif a.Type == 'Enchantment':
                # check card not in hand
                # check cost vs. mana
                # check target(s) validity
                pass

        ###################################################
        #
        # Pre-turn bookkeeping
        #
        ###################################################

        ###################################################
        #
        # Apply Actions to current TurnBasedMatch Resource
        #
        ###################################################

        for a in actions:
            turn_based_match_resource.apply(username, a)

        ###################################################
        #
        # Post-turn bookkeeping
        #
        ###################################################

        # if turn_number >= 5:
        #     match['status'] = "MATCH_COMPLETE"

        turn_based_match_resource.increment_mana(username, 1)
        turn_based_match_resource.change_player_turn()
        turn_based_match_resource.increment_turn_number(1)

        ###################################################
        #
        # Format turnBasedMatchResource for response
        #
        ###################################################

        ###################################################
        #
        # Update turnBasedMatchResource in Database
        #
        ###################################################

        matchDatabase.set(str(get_id_from_match(match_id)), match)

        return jsonify({'success': True, 'results': {'match': match}})
    except Exception, e:
        return jsonify({'success': False,'errors': {'message': str(e)}})

from twisted.web.wsgi import WSGIResource
from twisted.web.server import Site
from twisted.internet import reactor

reactor.suggestThreadPoolSize(64)
resource = WSGIResource(reactor, reactor.getThreadPool(), application)
site = Site(resource)
reactor.listenTCP(10009, site,interface='127.0.0.1')
reactor.run(installSignalHandlers=0)
