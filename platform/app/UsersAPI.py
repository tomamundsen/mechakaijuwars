from flask import Blueprint, jsonify, request, abort

from lib.Server.Utils import crossdomain, userDatabase, get_username_from_token, is_valid_token


users_api = Blueprint('users_api', __name__)

@users_api.route('/api/users', methods = ['POST'])
@crossdomain()
def _new_user():
    data = request.get_json()
    if len(data) < 2:
        return jsonify({'success': False, 'errors': {'message': 'input username and password'}})

    username = data['username']
    password = data['password']

    if username is None or password is None:
        return jsonify({'success': False, 'errors': {'message': 'input username and password'}})

    new_user = {
        'username': username,
        'password': password
    }

    try:
        userDatabase.get(username)
        return jsonify({'success': False, 'errors': {'message': 'username \'' + username + '\' already exists'}})
    except Exception, e:
        pass

    try:
        userDatabase.set(username, new_user)
    except Exception, e:
        return jsonify({'success': False, 'errors': {'message': str(e)}})

    try:
        result = userDatabase.get(username)
        return jsonify({'success': True, 'results': {'message': 'it was successfsul', 'data': result.value}})
    except Exception, e:
        return jsonify({'success': False, 'results': {'message': str(e)}})

@users_api.route('/users/me', methods = ['GET'])
@crossdomain()
def _users_me():
    token = request.args.get('access_token')

    if token is not None:
        if is_valid_token(token):
            username = get_username_from_token(token)
            try:
                result = userDatabase.get(username)
                return jsonify({'success': True, 'results': {'data': result.value}})
            except Exception, e:
                return jsonify({'success': False, 'errors': {'message': 'database error: ' + str(e)}})
    abort(401)
