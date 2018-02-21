
import base64
import zlib
import json
import sys

j = json.load(sys.stdin)

if 'layers' in j:
    for l in j['layers']:
        if 'data' in l:
            data = l['data']
            decoded = base64.b64decode(data)
            decompressed = zlib.decompress(decoded)
            result = []
            for b in decompressed:
                result.append(ord(b))
            l['data'] = result

print json.dumps(j, indent=4, sort_keys=False)
