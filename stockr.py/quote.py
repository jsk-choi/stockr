import os
import sys
import _conf as cf
import json
import random
import pprint
import sseclient

import urllib3
import requests
import threading

import db.log as log
from db.dbrep import dbrep

def with_requests(url, chunk):

    messages = sseclient.SSEClient(url)
    for msg in messages:

        if (msg.data):

            try:
                jsn = json.loads(msg.data)

                if not jsn:
                    continue

                print(build_ins_bulk(jsn[0]))
                
                #insCols, insVals = build_ins(jsn[0])

                ##jsn_str = json.dumps(jsn[0])
                #dbsymins = dbrep('Quote_Stg')
                #ins_stmt = dbsymins.sql_ins_stmt(insCols, insVals)
                #dbsymins.insert_row(insCols, insVals)
                ##dbsymins.sp_exec('spQuotesConsolidation', "@msg=?", (str(chunk)))
            except:
                log.logmsg('error : ' + ins_stmt)
                log.logmsg('error : ' + msg.data)


def build_ins_bulk(s):

    vals_col = []
    vals_col.append(s.setdefault('avgTotalVolume', 'NULL').replace('`', ''))
    vals_col.append(s.setdefault('calculationPrice', 'NULL').replace('`', ''))
    vals_col.append(s.setdefault('change', 'NULL').replace('`', ''))
    vals_col.append(s.setdefault('changePercent', 'NULL').replace('`', ''))
    vals_col.append(s.setdefault('[close]', 'NULL').replace('`', ''))
    vals_col.append(s.setdefault('closeSource', 'NULL').replace('`', ''))
    vals_col.append(s.setdefault('closeTime', 'NULL').replace('`', ''))
    vals_col.append(s.setdefault('companyName', 'NULL').replace('`', ''))
    vals_col.append(s.setdefault('delayedPrice', 'NULL').replace('`', ''))
    vals_col.append(s.setdefault('delayedPriceTime', 'NULL').replace('`', ''))
    vals_col.append(s.setdefault('extendedChange', 'NULL').replace('`', ''))
    vals_col.append(s.setdefault('extendedChangePercent', 'NULL').replace('`', ''))
    vals_col.append(s.setdefault('extendedPrice', 'NULL').replace('`', ''))
    vals_col.append(s.setdefault('extendedPriceTime', 'NULL').replace('`', ''))
    vals_col.append(s.setdefault('high', 'NULL').replace('`', ''))
    vals_col.append(s.setdefault('highSource', 'NULL').replace('`', ''))
    vals_col.append(s.setdefault('highTime', 'NULL').replace('`', ''))
    vals_col.append(s.setdefault('lastTradeTime', 'NULL').replace('`', ''))
    vals_col.append(s.setdefault('latestPrice', 'NULL').replace('`', ''))
    vals_col.append(s.setdefault('latestSource', 'NULL').replace('`', ''))
    vals_col.append(s.setdefault('latestTime', 'NULL').replace('`', ''))
    vals_col.append(s.setdefault('latestUpdate', 'NULL').replace('`', ''))
    vals_col.append(s.setdefault('latestVolume', 'NULL').replace('`', ''))
    vals_col.append(s.setdefault('low', 'NULL').replace('`', ''))
    vals_col.append(s.setdefault('lowSource', 'NULL').replace('`', ''))
    vals_col.append(s.setdefault('lowTime', 'NULL').replace('`', ''))
    vals_col.append(s.setdefault('marketCap', 'NULL').replace('`', ''))
    vals_col.append(s.setdefault('oddLotDelayedPrice', 'NULL').replace('`', ''))
    vals_col.append(s.setdefault('oddLotDelayedPriceTime', 'NULL').replace('`', ''))
    vals_col.append(s.setdefault('[open]', 'NULL').replace('`', ''))
    vals_col.append(s.setdefault('openSource', 'NULL').replace('`', ''))
    vals_col.append(s.setdefault('openTime', 'NULL').replace('`', ''))
    vals_col.append(s.setdefault('peRatio', 'NULL').replace('`', ''))
    vals_col.append(s.setdefault('previousClose', 'NULL').replace('`', ''))
    vals_col.append(s.setdefault('previousVolume', 'NULL').replace('`', ''))
    vals_col.append(s.setdefault('primaryExchange', 'NULL').replace('`', ''))
    vals_col.append(s.setdefault('symbol', 'NULL').replace('`', ''))
    vals_col.append(s.setdefault('volume', 'NULL').replace('`', ''))
    vals_col.append(s.setdefault('week52High', 'NULL').replace('`', ''))
    vals_col.append(s.setdefault('week52Low', 'NULL').replace('`', ''))
    vals_col.append(s.setdefault('ytdChange', 'NULL').replace('`', ''))
    return '`'.join(vals_col)


def build_ins(s):

    #ins_col = 'INSERT INTO dbo.Quote_stag ('
    #ins_val = 'VALUES ('

    val_tmp = 0

    ins_col = '[lowSource],'
    val_tmp = s.setdefault('lowSource', None)
    ins_val = quote_surr(str('NULL' if val_tmp is None else val_tmp)) + ','

    ins_col += '[closeSource],'
    val_tmp = s.setdefault('closeSource', None)
    ins_val += quote_surr(str('NULL' if val_tmp is None else val_tmp)) + ','

    ins_col += '[latestSource],'
    val_tmp = s.setdefault('latestSource', None)
    ins_val += quote_surr(str('NULL' if val_tmp is None else val_tmp)) + ','

    ins_col += '[latestTime],'
    val_tmp = s.setdefault('latestTime', None)
    ins_val += quote_surr(str('NULL' if val_tmp is None else val_tmp)) + ','

    ins_col += '[openSource],'
    val_tmp = s.setdefault('openSource', None)
    ins_val += quote_surr(str('NULL' if val_tmp is None else val_tmp)) + ','

    ins_col += '[primaryExchange],'
    val_tmp = s.setdefault('primaryExchange', None)
    ins_val += quote_surr(str('NULL' if val_tmp is None else val_tmp)) + ','

    ins_col += '[symbol],'
    val_tmp = s.setdefault('symbol', None)
    ins_val += quote_surr(str('NULL' if val_tmp is None else val_tmp)) + ','

    ins_col += '[companyName],'
    val_tmp = s.setdefault('companyName', None)
    ins_val += quote_surr(str('NULL' if val_tmp is None else val_tmp)) + ','

    ins_col += '[highSource],'
    val_tmp = s.setdefault('highSource', None)
    ins_val += quote_surr(str('NULL' if val_tmp is None else val_tmp)) + ','

    ins_col += '[calculationPrice],'
    val_tmp = s.setdefault('calculationPrice', None)
    ins_val += quote_surr(str('NULL' if val_tmp is None else val_tmp)) + ','





    ins_col += '[week52Low],'
    val_tmp = s.setdefault('week52Low', None)
    ins_val += str('NULL' if val_tmp is None else val_tmp) + ','

    ins_col += '[week52High],'
    val_tmp = s.setdefault('week52High', None)
    ins_val += str('NULL' if val_tmp is None else val_tmp) + ','

    ins_col += '[latestUpdate],'
    val_tmp = s.setdefault('latestUpdate', None)
    ins_val += str('NULL' if val_tmp is None else val_tmp) + ','

    ins_col += '[latestVolume],'
    val_tmp = s.setdefault('latestVolume', None)
    ins_val += str('NULL' if val_tmp is None else val_tmp) + ','

    ins_col += '[openTime],'
    val_tmp = s.setdefault('openTime', None)
    ins_val += str('NULL' if val_tmp is None else val_tmp) + ','

    ins_col += '[peRatio],'
    val_tmp = s.setdefault('peRatio', None)
    ins_val += str('NULL' if val_tmp is None else val_tmp) + ','

    ins_col += '[close],'
    val_tmp = s.setdefault('close', None)
    ins_val += str('NULL' if val_tmp is None else val_tmp) + ','

    ins_col += '[marketCap],'
    val_tmp = s.setdefault('marketCap', None)
    ins_val += str('NULL' if val_tmp is None else val_tmp) + ','

    ins_col += '[latestPrice],'
    val_tmp = s.setdefault('latestPrice', None)
    ins_val += str('NULL' if val_tmp is None else val_tmp) + ','

    ins_col += '[low],'
    val_tmp = s.setdefault('low', None)
    ins_val += str('NULL' if val_tmp is None else val_tmp) + ','

    ins_col += '[lowTime],'
    val_tmp = s.setdefault('lowTime', None)
    ins_val += str('NULL' if val_tmp is None else val_tmp) + ','

    ins_col += '[extendedPriceTime],'
    val_tmp = s.setdefault('extendedPriceTime', None)
    ins_val += str('NULL' if val_tmp is None else val_tmp) + ','

    ins_col += '[high],'
    val_tmp = s.setdefault('high', None)
    ins_val += str('NULL' if val_tmp is None else val_tmp) + ','

    ins_col += '[extendedChangePercent],'
    val_tmp = s.setdefault('extendedChangePercent', None)
    ins_val += str('NULL' if val_tmp is None else val_tmp) + ','

    ins_col += '[oddLotDelayedPriceTime],'
    val_tmp = s.setdefault('oddLotDelayedPriceTime', None)
    ins_val += str('NULL' if val_tmp is None else val_tmp) + ','

    ins_col += '[open],'
    val_tmp = s.setdefault('open', None)
    ins_val += str('NULL' if val_tmp is None else val_tmp) + ','

    ins_col += '[change],'
    val_tmp = s.setdefault('change', None)
    ins_val += str('NULL' if val_tmp is None else val_tmp) + ','

    ins_col += '[avgTotalVolume],'
    val_tmp = s.setdefault('avgTotalVolume', None)
    ins_val += str('NULL' if val_tmp is None else val_tmp) + ','

    ins_col += '[changePercent],'
    val_tmp = s.setdefault('changePercent', None)
    ins_val += str('NULL' if val_tmp is None else val_tmp) + ','

    ins_col += '[previousVolume],'
    val_tmp = s.setdefault('previousVolume', None)
    ins_val += str('NULL' if val_tmp is None else val_tmp) + ','

    ins_col += '[volume],'
    val_tmp = s.setdefault('volume', None)
    ins_val += str('NULL' if val_tmp is None else val_tmp) + ','

    ins_col += '[ytdChange],'
    val_tmp = s.setdefault('ytdChange', None)
    ins_val += str('NULL' if val_tmp is None else val_tmp) + ','

    ins_col += '[extendedPrice],'
    val_tmp = s.setdefault('extendedPrice', None)
    ins_val += str('NULL' if val_tmp is None else val_tmp) + ','

    ins_col += '[highTime],'
    val_tmp = s.setdefault('highTime', None)
    ins_val += str('NULL' if val_tmp is None else val_tmp) + ','

    ins_col += '[delayedPriceTime],'
    val_tmp = s.setdefault('delayedPriceTime', None)
    ins_val += str('NULL' if val_tmp is None else val_tmp) + ','

    ins_col += '[extendedChange],'
    val_tmp = s.setdefault('extendedChange', None)
    ins_val += str('NULL' if val_tmp is None else val_tmp) + ','

    ins_col += '[closeTime],'
    val_tmp = s.setdefault('closeTime', None)
    ins_val += str('NULL' if val_tmp is None else val_tmp) + ','

    ins_col += '[delayedPrice],'
    val_tmp = s.setdefault('delayedPrice', None)
    ins_val += str('NULL' if val_tmp is None else val_tmp) + ','

    ins_col += '[oddLotDelayedPrice],'
    val_tmp = s.setdefault('oddLotDelayedPrice', None)
    ins_val += str('NULL' if val_tmp is None else val_tmp) + ','

    ins_col += '[previousClose],'
    val_tmp = s.setdefault('previousClose', None)
    ins_val += str('NULL' if val_tmp is None else val_tmp) + ','

    ins_col += '[lastTradeTime]'
    val_tmp = s.setdefault('lastTradeTime', None)
    ins_val += str('NULL' if val_tmp is None else val_tmp)


    return ins_col, ins_val

def quote_surr(val):
    return "'" + str(val).replace("'", "''") + "'"

def abbr(str):
    return ''.join(list(map(lambda x: x[0], str.split(' '))))

def chunks(lst, n):
    for i in range(0, len(lst), n):
        yield lst[i:i + n]

print('done two')

syms = []
sym_urls = []
sym_commands = []
url_templ = cf.url_quote_sse

dbsym = dbrep('vSymbols')
syms = dbsym.get_rows('symbol, [type]', "[type] = 'cs'")
syms_list = list(map(lambda x: x[0], syms))

chuncked = list(chunks(syms_list, 49))
#chuncked = random.shuffle(chuncked)

sym_commands.append(cf.job_kill)

ct = 1

for chk in chuncked:
    sym_csv = ','.join(chk)
    sym_commands.append(cf.script_command%(cf.script_loc, url_templ.format(sym_csv), ct))
    sym_commands.append('echo ' + str(ct) + '/' + str(len(chuncked)))
    sym_commands.append('sleep 0.5')
    ct += 1

sym_commands.append('')

print('\n'.join(sym_commands))

script_file = os.path.join(cf.script_loc, cf.script_name)

if os.path.exists(script_file):
    os.remove(script_file)

text_file = open(script_file, "w")
n = text_file.write('\n'.join(sym_commands))
text_file.close()

