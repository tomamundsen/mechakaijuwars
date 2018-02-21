

class Unit(object):

    def __init__(self, id, location):
        self.Id = id
        if hasattr(location, 'get_json'):
            self.Location = location.get_json()
        else:
            self.Location = location

    # @property
    # def id(self):
    #     pass
    #
    # @property
    # def location(self):
    #     pass

    def get_json(self):
        return {
            'Id': self.Id,
            'Location': self.Location
        }
