

class Point(object):

    def __init__(self, x, y):
        self.x = x
        self.y = y

    def get_json(self):
        return {
            'x': self.x,
            'y': self.y
        }
