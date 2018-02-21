
from flask import current_app, request, make_response
from datetime import timedelta
from functools import update_wrapper
import time
from couchbase import Couchbase

GAMEHOST_ENDPOINT = 'http://localhost:10009/'

tokenDatabase = Couchbase.connect(bucket='token', host='localhost')
userDatabase = Couchbase.connect(bucket='user', host='localhost')
matchDatabase = Couchbase.connect(bucket='match', host='localhost')
deckDatabase = Couchbase.connect(bucket='deck', host='localhost')

sandbox = 'firsttest'
version = '8bfcb86d7fd59afb56b05061b8fb248c3c59c96b'

import zipfile

with zipfile.ZipFile('data/' + sandbox + '/' + version + '.tgz', "r") as z:
    z.extractall('data/' + sandbox + '/')

import json
txt = open('data/' + sandbox + '/' + version + '/manifest.json').read().replace("'", "\"")
manifest = json.loads(txt)

card_data = {}
card_sheets = []
if 'Cards' in manifest and manifest['Cards'] and 'Cards' in manifest['Cards'] and manifest['Cards']['Cards']:
    for c in manifest['Cards']['Cards']:
        card_sheets.append(c)

for s in card_sheets:
    filename = 'data/' + sandbox + '/' + version + '/' + s + '.json'
    t = open(filename).read()
    card_sheet = json.loads(t)
    for c in card_sheet:
        card_data[c] = card_sheet[c]
    print str(card_sheet)

deck_data = {}
deck_sheets = []
if 'Decks' in manifest and manifest['Decks']:
    for d in manifest['Decks']:
        deck_sheets.append(d)

for s in deck_sheets:
    filename = 'data' + '/' + sandbox + '/' + version + '/' + s + '.json'
    t = open(filename).read()
    deck_sheet = json.loads(t)
    deck = []
    for d in deck_sheet:
        deck.append(d)
    deck_data[s] = deck



# def MY_GET(url, headers):
#     return MY_POST(url, None, headers)

def MY_POST(url, data, headers=None):
    try:
        import urllib2
        import json

        if headers is None:
            headers = {'Content-Type': 'application/json'}
        req = urllib2.Request(url, json.dumps(data), headers=headers)
        conn = urllib2.urlopen(req,timeout=10)
        resp = conn.read()
        conn.close()
        print 'Response: ' + resp
        return resp
    except Exception, e:
        print e

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

def is_valid_token(token):
    print 'is_valid_token(' + token + ')'
    try:
        token_data = tokenDatabase.get(token).value
        t = time.time()
        if ((t - float(token_data['time'])) > 30 * 60):
            try:
                tokenDatabase.delete(token)
            except Exception, e:
                print str(e)
            return False
        else:
            return True
    except Exception, e:
        return False

def get_username_from_token(token):
    print 'get_username_from_token(' + token + ')'
    try:
        t = tokenDatabase.get(token).value
        return t['username']
    except Exception, e:
        return None

def get_match_id(id):
    return 'match-' + str(id) + 'Id'

def get_id_from_match(match_id):
    return match_id[6:-2]

def get_deck_id(username, deck_id):
    decks = deckDatabase.get(username).value
    for d in decks:
        if 'deckId' in d and d['deckId'] == deck_id[6:]:
            return d['deckId']
    return None

