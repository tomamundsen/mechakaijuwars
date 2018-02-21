from flask import Blueprint, request, jsonify

from lib.Server.Utils import crossdomain, userDatabase, tokenDatabase


auth_api = Blueprint('auth_api', __name__)

@auth_api.route('/auth/login', methods=['POST'])
@crossdomain()
def _login():
    try:
        import base64
        credentials = base64.b64decode(request.headers['Authorization'][6:]).split(':')
        username = credentials[0]
        password = credentials[1]

        result = None
        try:
            result = userDatabase.get(username)
        except Exception, e:
            return jsonify({'success': False, 'errors': {'message': 'invalid username and/or password'}})

        if result.value is not None:
            if password == result.value['password']:
                import time
                import hashlib
                t = time.time()
                access_token = base64.b64encode(hashlib.sha1(str(t) + ':' + username).hexdigest())
                real_username = result.value['username']
                tokenDatabase.set(access_token, {'username': real_username, 'time': t, 'access_token': access_token})
                return jsonify({'results': {'access_token': access_token, 'player_id': real_username}})
            else:
                return jsonify({'success': False, 'errors': {'message': 'invalid username and/or password'}})
        else:
            return jsonify({'success': False, 'errors': {'message': 'no database result'}})
    except Exception, e:
        return jsonify({'success': False, 'errors': {'message': str(e)}})

@auth_api.route('/auth/token', methods=['POST'])
@crossdomain()
def _check_token():
    return jsonify({'success': False, 'errors': {'message': 'failure'}})

@auth_api.route('/auth/logout', methods=['POST'])
@crossdomain()
def _logout():
    data = request.get_json()
    access_token = data['access_token']

    try:
        tokenDatabase.delete(access_token)
        return jsonify({'results':{'success': True}})
    except Exception, e:
        return jsonify({'errors':{'message':'invalid access token'}})

