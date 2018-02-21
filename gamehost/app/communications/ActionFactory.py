

# class ActionFactory(object):
#
#    def __init__(self, theClass):
#        self.theClass = theClass
#
#    def create(self, **kwargs):
#        self.theClass(**kwargs)

from Action import Action as Action
from SummonAction import SummonAction as SummonAction

class ActionFactory(object):

    def __init__(self):
        pass
    #
    # @staticmethod
    # def create(d):
    #     for cls in Action.__subclasses__():
    #         if cls.is_action_for(d):
    #             return cls(d)
    #     raise ValueError

    @staticmethod
    def make_action(a):
        if 'Type' in a:
            if a['Type'] == 'Summon' and 'Id' in a and 'Location' in a:
                return SummonAction(a['Id'], a['Location'])
        raise ValueError
