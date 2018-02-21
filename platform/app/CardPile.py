
import json
import random

class CardPile:
    def __init__(self, deck=None):
        self.cards = []
        if deck:
            for d in deck.deck:
                for i in range(0, int(d['Quantity'])):
                    self.cards.append(d['ID'])

    def __repr__(self):
        return self.cards

    def __str__(self):
        return json.dumps(self.cards)

    def pop(self):
        return self.cards.pop()

    def push(self, c):
        self.cards.append(c)

    def shuffle(self):
        shuffled = []
        while len(self.cards) > 0:
            i = random.randint(0, len(self.cards) - 1)
            shuffled.append(self.cards[i])
            self.cards.pop(i)
        self.cards = shuffled