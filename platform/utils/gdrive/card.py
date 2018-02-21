class Card:
    def __init__(self, card_data):
        self.data = card_data
        # values = card_data.get_all_values()
        # # print values[0]
        # keys = {}
        # i = 0
        # for v in values[0]:
        #     keys[v] = i
        #     i = i + 1
        #
        #     # print 'keys: ' + str(keys)
        #
        # keys_final = {}
        # for k in keys:
        #     if k:
        #         keys_final[k] = keys[k]
        #
        # # print 'keys_final: ' + str(keys_final)
        #
        # keys_reverse = {}
        # for k in keys:
        #     keys_reverse[keys[k]] = k
        #
        # # print 'keys_reverse: ' + str(keys_reverse)
        #
        # data = []
        # for r in values:
        #     datum = {}
        #
        #     i = 0
        #     for col in r:
        #         if col:
        #             try:
        #                 datum[keys_reverse[i]] = col
        #             except Exception, e:
        #                 pass
        #         else:
        #             try:
        #                 if keys_reverse[i]:
        #                     datum[keys_reverse[i]] = None
        #             except Exception, e:
        #                 pass
        #         i = i + 1
        #
        #     if datum['ID'] and datum['ID'] != 'ID':
        #         data.append(datum)
        #         # data[datum['ID']] = datum

    # import pprint
    # pp = pprint.PrettyPrinter(indent=4)
    # pp.pprint(data)
