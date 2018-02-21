
from Utils import deckDatabase, deck_data

class Deck:
    def __init__(self, username, deck_id):
        self.deck = deck_data['Decks_' + deck_id]
