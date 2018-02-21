
import sqlite3
from flask import Flask, request, session, g, redirect, url_for, \
     abort, render_template, flash, make_response, current_app, jsonify
from contextlib import closing
from datetime import timedelta
from functools import update_wrapper
import logging
import time

# from flask.ext.httpauth import HTTPBasicAuth
# from passlib.apps import custom_app_context as pwd_context
# from flask.ext.sqlalchemy import SQLAlchemy
# from flask.ext.httpauth import HTTPBasicAuth
# from itsdangerous import (TimedJSONWebSignatureSerializer
#                           as Serializer, BadSignature, SignatureExpired)

from couchbase import Couchbase
from pprint import pprint
import urllib2

from UsersAPI import users_api
from AuthAPI import auth_api
from PlayersAPI import players_api
from TurnbasedmatchesAPI import turnbasedmatches_api

from twisted.web.wsgi import WSGIResource
from twisted.web.server import Site
from twisted.internet import reactor


######################################################################
##
## initialization
##
##
######################################################################

application = Flask(__name__)
application.threaded = True
application.config.from_object(__name__)
application.config.from_envvar('PLATFORM_SETTINGS', silent=True)

GAMEHOST_ENDPOINT = 'http://localhost:10009/'

# auth = HTTPBasicAuth()

######################################################################
##
## authentication
##
##
######################################################################

# @auth.get_password
# def get_password(username):
#     if username == 'admin':
#         return 'default'
#     return None

# @auth.error_handler
# def unauthorized():
#     return make_response(jsonify({'success': False, 'errors': 'unauthorized access'}), 401)

######################################################################
##
## framework
##
##
######################################################################

@application.before_request
def before_request():
    t = time.time()
    # con = engine.connect()
    # con.execute(users.insert(), name='admin' + str(t), email='admin@localhost' + str(t))
    
# @application.teardown_appcontext
# def shutdown_session(exception=None):
#   db_session.remove()

@application.teardown_request
def teardown_request(exception):
    print 'teardown'

@application.errorhandler(404)
def not_found(error):
    return make_response(jsonify({'success': False, 'errors': {'message': 'not found'}}), 404)

######################################################################
##
## register API blueprints
##
##
######################################################################

application.register_blueprint(auth_api)
application.register_blueprint(users_api)
application.register_blueprint(players_api)
application.register_blueprint(turnbasedmatches_api)


######################################################################
##
## run application as multi-threaded Twisted application
##
##
######################################################################
reactor.suggestThreadPoolSize(64)
resource = WSGIResource(reactor, reactor.getThreadPool(), application)
site = Site(resource)
reactor.listenTCP(10008, site,interface='127.0.0.1')
reactor.run(installSignalHandlers=0)
