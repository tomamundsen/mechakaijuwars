import json
import gspread
import sys
from card import Card
from oauth2client.client import SignedJwtAssertionCredentials

files = []
archive_name = ''

import hashlib
import time

def get_keys_reverse(values):
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

    return keys_reverse


def get_sandbox_names(values, keys_reverse):
    sandboxes = []
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

        if datum['Name'] and datum['Name'] != 'Name':
            sandboxes.append(datum)
    return sandboxes

def extract_card_data(values):
    keys_reverse = get_keys_reverse(values)
    # data = []
    cards = []
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
            cards.append(Card(datum))

    return cards

def get_card_sheets_data(key):
    global files

    cards_sheets = gc.open_by_key(key).sheet1.get_all_values()
    all_cards = {}
    all_cards['Cards'] = {}
    all_cards['ActivatedAbilities'] = {}
    all_cards['TriggeredAbilities'] = {}
    all_cards['ContinuousEffects'] = {}
    keys_reverse = get_keys_reverse(cards_sheets)
    for row in cards_sheets:
        if row[1] == 'ID':
            continue

        values = gc.open_by_key(row[1]).get_worksheet(0).get_all_values()
        cards = extract_card_data(values)
        cards_json = {}
        for c in cards:
            cards_json[c.data['ID']] = c.data

        triggered_abilities = gc.open_by_key(row[1]).get_worksheet(1).get_all_values()
        triggered_data = extract_card_data(triggered_abilities)
        triggered_json = {}
        for c in triggered_data:
            triggered_json[c.data['ID']] = c.data

        continuous_effects = gc.open_by_key(row[1]).get_worksheet(2).get_all_values()
        continuous_data = extract_card_data(continuous_effects)
        continuous_json = {}
        for c in continuous_data:
            continuous_json[c.data['ID']] = c.data

        activated_abilities = gc.open_by_key(row[1]).get_worksheet(3).get_all_values()
        activated_data = extract_card_data(activated_abilities)
        activated_json = {}
        for c in activated_data:
            activated_json[c.data['ID']] = c.data

        if row[0] == 'Cards':
            all_cards['Cards'][row[1]] = cards_json
            all_cards['ActivatedAbilities'][row[1]] = activated_json
            all_cards['TriggeredAbilities'][row[1]] = triggered_json
            all_cards['ContinuousEffects'][row[1]] = continuous_json
        elif row[0] == 'ActivatedAbilities':
            pass
        elif row[0] == 'TriggeredAbilities':
            pass
        elif row[0] == 'ContinuousEffects':
            pass
        else:
            print '!!! Warn - unknown CardSheet type'
            sys.exit()

        import os
        file_name = os.path.join(sandbox_name, archive_name, 'Cards_' + row[1] + '_Cards' + '.json')
        target = open(file_name, 'w')
        files.append(file_name)
        target.truncate()
        target.write(json.dumps(cards_json))

        file_name = os.path.join(sandbox_name, archive_name, 'Cards_' + row[1] + '_TriggeredAbilities' + '.json')
        target = open(file_name, 'w')
        files.append(file_name)
        target.truncate()
        target.write(json.dumps(triggered_json))

        file_name = os.path.join(sandbox_name, archive_name, 'Cards_' + row[1] + '_ContinuousEffects' + '.json')
        target = open(file_name, 'w')
        files.append(file_name)
        target.truncate()
        target.write(json.dumps(continuous_json))

        file_name = os.path.join(sandbox_name, archive_name, 'Cards_' + row[1] + '_ActivatedAbilities' + '.json')
        target = open(file_name, 'w')
        files.append(file_name)
        target.truncate()
        target.write(json.dumps(activated_json))

    return all_cards


def get_deck_data(key):
    global files
    values = gc.open_by_key(key).sheet1.get_all_values()

    keys_reverse = get_keys_reverse(values)
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

    file_name = os.path.join(sandbox_name, archive_name, 'Decks_' + key + '.json')
    target = open(file_name, 'w')
    files.append(file_name)
    target.truncate()
    target.write(json.dumps(data))

    return data

if __name__ == '__main__':
    if len(sys.argv) == 2:
        sandbox_name = sys.argv[1]
    else:
        print 'please input sandbox name and sandbox name only'
        sys.exit()

    hash_object = hashlib.sha1(sandbox_name + str(time.time()))
    archive_name = hash_object.hexdigest()

    import os
    import shutil

    try:
        os.mkdir(sandbox_name)
    except Exception, e:
        pass

    try:
        shutil.rmtree(os.path.join(sandbox_name, archive_name))
    except Exception, e:
        pass
    os.mkdir(os.path.join(sandbox_name, archive_name))
    json_key = json.load(open('MKW.json'))
    scope = ['https://spreadsheets.google.com/feeds']

    credentials = SignedJwtAssertionCredentials(json_key['client_email'], json_key['private_key'], scope)

    gc = gspread.authorize(credentials)

    wks = gc.open("Sandboxes").sheet1

    values = wks.get_all_values()

    keys_reverse = get_keys_reverse(values)
    sandboxes = get_sandbox_names(values, keys_reverse)

    import pprint

    pp = pprint.PrettyPrinter(indent=4)

    sandbox = None

    for s in sandboxes:
        if s['Name'] == sys.argv[1]:
            sandbox = s
            break

    if not sandbox:
        print 'no sandbox found: ' + sandbox_name
        sys.exit()

    cards_data = get_card_sheets_data(sandbox['CardsSheets'])

    decks_list = gc.open_by_key(s['DecksList']).sheet1.get_all_values()
    reverse_keys = get_keys_reverse(decks_list)

    manifest = {}
    manifest['Decks'] = []
    decks = {}
    deck_list_trimmed = decks_list[1:]
    for row in deck_list_trimmed:
        for v in reverse_keys:
            if reverse_keys[v] == 'Key':
                deck = get_deck_data(row[v])
                manifest['Decks'].append('Decks_' + row[v])

    manifest['Cards'] = {}
    manifest['Cards']['Cards'] = []
    for k in cards_data['Cards']:
        manifest['Cards']['Cards'].append('Cards_' + k + '_Cards')

    manifest['Cards']['ActivatedAbilities'] = []
    for k in cards_data['ActivatedAbilities']:
        manifest['Cards']['ActivatedAbilities'].append('Cards_' + k + '_ActivatedAbilities')

    manifest['Cards']['TriggeredAbilities'] = []
    for k in cards_data['TriggeredAbilities']:
        manifest['Cards']['TriggeredAbilities'].append('Cards_' + k + '_TriggeredAbilities')

    manifest['Cards']['ContinuousEffects'] = []
    for k in cards_data['ContinuousEffects']:
        manifest['Cards']['ContinuousEffects'].append('Cards_' + k + '_ContinuousEffects')

    target = open(os.path.join(sandbox_name, archive_name, 'manifest.json'), 'w')
    target.truncate()
    target.write(json.dumps(manifest))
    target.close()

    def zipdir(path, ziph):
        # ziph is zipfile handle
        for root, dirs, files in os.walk(path):
            for file in files:
                ziph.write(os.path.join(root, file))
    import zipfile
    os.chdir(sandbox_name)
    zipf = zipfile.ZipFile(archive_name + '.tgz', 'w')
    zipdir(archive_name, zipf)
    zipf.close()