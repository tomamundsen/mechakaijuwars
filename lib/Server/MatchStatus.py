
from Enum import enum as enum


# def enum(**enums):
#     return type('Enum', (), enums)

MatchStatus = enum(MATCH_AUTO_MATCHING='MATCH_AUTO_MATCHING',
                   MATCH_ACTIVE='MATCH_ACTIVE',
                   MATCH_COMPLETE='MATCH_COMPLETE',
                   MATCH_EXPIRED='MATCH_EXPIRED',
                   MATCH_DELETED='MATCH_DELETED')

#
# class MatchStatus(Enum):
#
#     MATCH_AUTO_MATCHING = 0
#     MATCH_ACTIVE = 1
#     MATCH_COMPLETE = 2
#     MATCH_CANCELLED = 3
#     MATCH_EXPIRED = 4
#     MATCH_DELETED = 5
#
#     def __str__(self):
#         if self.value == 0:
#             return 'MATCH_AUTO_MATCHING'