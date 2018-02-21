
# from enum import Enum
from Enum import enum as enum
# class UserStatus(Enum):
#
#     USER_INVITED = 0
#     USER_AWAITING_TURN = 1
#     USER_TURN = 2
#     USER_MATCH_COMPLETED = 3


UserStatus = enum(USER_INVITED='USER_INVITED',
                   USER_AWAITING_TURN='USER_AWAITING_TURN',
                   USER_TURN='USER_TURN',
                   USER_MATCH_COMPLETED='USER_MATCH_COMPLETED',)
