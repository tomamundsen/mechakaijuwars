
#######################################################################
#
# Sample Usage:
# cat ~/mechakaijuwars/platform/utils/gdrive/firsttest/9dd77e0db40f5262ec013d393b79ce04c61b2bf8/Cards_1sdpY_5hqjIe2lC64PAJrpi4PyXhvzlFC_bf9cEuYhmQ_Cards.json | python pp_json.py > ~/Desktop/output.txt
#
#
#######################################################################
import json
import sys

obj = json.load(sys.stdin)

print json.dumps(obj, sort_keys=False, indent=4, separators=(',', ': '))