import json
import sys
import os
import base64
import zlib
import json
import sys
import shutil
import struct

archive_name = 'archive'

if __name__ == '__main__':
    if len(sys.argv) == 2:
        sandbox_name = sys.argv[1]
    else:
        print 'please input sandbox name and sandbox name only'
        sys.exit()

    try:
        os.mkdir(sandbox_name)
        os.mkdir(os.path.join(sandbox_name, archive_name))
    except Exception, e:
        pass

    j = json.load(open('tiled/test.json'))

    if 'layers' in j:
        for l in j['layers']:
            if 'data' in l:
                data = l['data']
                decoded = base64.b64decode(data)
                decompressed = zlib.decompress(decoded)

    the_list = []
    i = 0
    for y in range(0, j['height']):
        for x in range(0, j['width']):
            d = {}
            d['type'] = struct.unpack('<i', decompressed[i:i+4])
            the_list.append(d)
            d['location'] = {}
            d['location']['x'] = x
            d['location']['y'] = y
            i += 4

    target = open(os.path.join(sandbox_name, archive_name, 'map.json'), 'w')
    target.truncate()
    target.write(json.dumps(the_list))
    target.close()
