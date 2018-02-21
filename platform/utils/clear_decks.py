
import sys
from couchbase import Couchbase

try:
	deckDatabase = Couchbase.connect(bucket='deck', host='localhost')
	deckDatabase.delete('bill')
	deckDatabase.delete('thomas')
	deckDatabase.delete('dick')
except Exception, e:
	print str(e)