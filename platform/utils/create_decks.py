
import sys
import json
from couchbase import Couchbase

deckDatabase = Couchbase.connect(bucket='deck', host='localhost')

j = [
        {'deckId': '1x94Ww7sAsh4S79qsU-xkFg7O01F3mZohTsfMNzIaeiw', 'displayName': 'deck0', 'image':'image'},
        {'deckId': '1TuzaAB30Pw6uZUaH5kXrQvTZlF5vGw12-ev7Gm98GIM', 'displayName': 'deck1', 'image':'image'}
    ]

deckDatabase.set('thomas', j)
deckDatabase.set('bill', j)
deckDatabase.set('dick', j)
