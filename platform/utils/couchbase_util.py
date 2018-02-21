
from couchbase import Couchbase
import sys
matchDatabase = Couchbase.connect(bucket='match', host='localhost')

try:
	matchDatabase.set(sys.argv[1], sys.argv[2])
	print matchDatabase.get(sys.argv[1]).value
except Exception, e:
	print repr(e)

