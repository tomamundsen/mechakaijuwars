import json
import gspread
from oauth2client.client import SignedJwtAssertionCredentials

json_key = json.load(open('MKW.json'))
scope = ['https://spreadsheets.google.com/feeds']

credentials = SignedJwtAssertionCredentials(json_key['client_email'], json_key['private_key'], scope)

gc = gspread.authorize(credentials)

wks = gc.open("ThomasDecks1").sheet1

decks = {}

values = wks.get_all_values()
for r in values:
	for c in r:
		wks = gc.open(c).sheet1
		values = wks.get_all_values()

		keys = {}
		i = 0
		for v in values[0]:
			keys[v] = i
			i = i + 1

		keys_final = {}
		for k in keys:
			if k:
				keys_final[k] = keys[k]

		keys_reverse = {}
		for k in keys:
			keys_reverse[keys[k]] = k

		data = []
		for r in values:
			datum = {}

			i = 0
			for col in r:
				if col:
					try:
						datum[keys_reverse[i]] = col
					except Exception, e:
						pass
				else:
					try:
						if keys_reverse[i]:
							datum[keys_reverse[i]] = None
					except Exception, e:
						pass
				i = i + 1

			if datum['ID'] and datum['ID'] != 'ID':
				data.append(datum)

		decks[c] = data


import pprint
pp = pprint.PrettyPrinter(indent=4)
pp.pprint(decks)
