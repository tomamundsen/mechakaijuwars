
import sys
from couchbase import Couchbase

matchDatabase = Couchbase.connect(bucket='match', host='localhost')

for i in range(0,1000):
	try:
		matchDatabase.delete(str(i))
	except Exception, e:
		print str(i) + '\n' + str(e)
try:
	matchDatabase.delete('current')
except Exception, e:
	print str(e)