
from Action import Action as Action

class SummonAction(Action):

    def __init__(self, card_level_sheet_id, location):
        super(SummonAction, self).__init__('Summon')
        self.Id = card_level_sheet_id
        self.Location = location

    @classmethod
    def is_action_for(cls, d):
        if 'Type' not in d:
            return False
        else:
            return d['Type'] == 'Summon'

    @classmethod
    def fromdict(cls, d):
        self = cls(d['Id'], d['Location'])
        return self

    def get_json(self):
        return {
            'Id': self.Id,
            'Type': self.Type,
            'Location': self.Location
        }
