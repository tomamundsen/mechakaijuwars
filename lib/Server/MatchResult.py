
# from enum import Enum
#
#
# class MatchResult(Enum):
#
#     MATCH_RESULT_WIN = 0
#     MATCH_RESULT_LOSS = 1
#     MATCH_RESULT_TIE = 2
#     MATCH_RESULT_NONE = 3
#     MATCH_RESULT_DISCONNECT = 4
#     MATCH_RESULT_DISAGREED = 5

from Enum import enum as enum

MatchResult= enum(MATCH_RESULT_WIN='MATCH_RESULT_WIN',
                   MATCH_RESULT_LOSS='MATCH_RESULT_LOSS',
                   MATCH_RESULT_TIE='MATCH_RESULT_TIE',
                   MATCH_RESULT_NONE='MATCH_RESULT_NONE',
                   MATCH_RESULT_DISCONNECT='MATCH_RESULT_DISCONNECT',
                   MATCH_RESULT_DISAGREED='MATCH_RESULT_DISAGREED')
