
import sys
import json
from couchbase import Couchbase

decklistDatabase = Couchbase.connect(bucket='decklist', host='localhost')

j = [
        {'deckId': '1x94Ww7sAsh4S79qsU-xkFg7O01F3mZohTsfMNzIaeiw', 'displayName': 'Deck1'},
        {'deckId': '1TuzaAB30Pw6uZUaH5kXrQvTZlF5vGw12-ev7Gm98GIM', 'displayName': 'Deck2'}
    ] 
decklistDatabase.set('thomas', j)
decklistDatabase.set('bill', j)
decklistDatabase.set('dick', j)
