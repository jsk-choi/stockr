import sys
import _conf as cf
import json
import os
import pprint
import sseclient
import pyodbc 

import urllib3
import requests
import threading

import db.log as log
from db.dbrep import dbrep

import atexit

whom = ''

def savecounter():
    log.logmsg('died : ' + ex)

def with_requests(url, idx):

    bulk_file = os.path.join(cf.bulk_file_path, 'bulk' + str(idx) + '.dat')    

    if not os.path.exists(bulk_file):
        open(bulk_file, "w+").close()    

    whom = url
    insVals = []
    jsn = ''

    try:
        messages = sseclient.SSEClient(url)
        dbsymins = dbrep('Quote_Stg')

        for msg in messages:

            if (msg.data):

                jsn = json.loads(msg.data)

                if not jsn:
                    continue                

                jjj = build_ins_bulk(jsn[0])

                #print(jjj)

                file_object = open(bulk_file, 'a')
                with file_object:
                    file_object.write(jjj + '\n')                    
                    file_object.close()

                #insCols, insVal = build_ins(jsn[0])
                #ffjfj = dbsymins.sql_ins_stmt(insCols, insVal)
                #log.logmsg(ffjfj)
                #dbsymins.insert_row(insCols, insVal)

                #insVals.append(insVal)

                #if len(insVals) > 500:
                #    dbsymins.insert_row(insCols, ("),(".join(insVals)))
                #    insVals = []

                ##jsn_str = json.dumps(jsn[0])
                ##dbsymins.sp_exec('spQuotesConsolidation', "@msg=?", (str(chunk)))

    except Exception as ex:
        log.logmsg(str(ex))
        log.logmsg(jsn)

def build_ins_bulk(s):

    vals_col = []

    try:
        vals_col.append(str(s.setdefault('avgTotalVolume', '')))
        vals_col.append(str(s.setdefault('calculationPrice', None).replace('`', '')))
        vals_col.append(str(s.setdefault('change', '')))
        vals_col.append(str(s.setdefault('changePercent', '')))
        vals_col.append(str(s.setdefault('close', '')))
        vals_col.append(str(s.setdefault('closeSource', None).replace('`', '')))
        vals_col.append(str(s.setdefault('closeTime', '')))
        vals_col.append(str(s.setdefault('companyName', None).replace('`', '')))
        vals_col.append(str(s.setdefault('delayedPrice', '')))
        vals_col.append(str(s.setdefault('delayedPriceTime', '')))
        vals_col.append(str(s.setdefault('extendedChange', '')))
        vals_col.append(str(s.setdefault('extendedChangePercent', '')))
        vals_col.append(str(s.setdefault('extendedPrice', '')))
        vals_col.append(str(s.setdefault('extendedPriceTime', '')))
        vals_col.append(str(s.setdefault('high', '')))
        vals_col.append(str(s.setdefault('highSource', None).replace('`', '')))
        vals_col.append(str(s.setdefault('highTime', '')))
        vals_col.append(str(s.setdefault('lastTradeTime', '')))
        vals_col.append(str(s.setdefault('latestPrice', '')))
        vals_col.append(str(s.setdefault('latestSource', None).replace('`', '')))
        vals_col.append(str(s.setdefault('latestTime', None).replace('`', '')))
        vals_col.append(str(s.setdefault('latestUpdate', '')))
        vals_col.append(str(s.setdefault('latestVolume', '')))
        vals_col.append(str(s.setdefault('low', '')))
        vals_col.append(str(s.setdefault('lowSource', None).replace('`', '')))
        vals_col.append(str(s.setdefault('lowTime', '')))
        vals_col.append(str(s.setdefault('marketCap', '')))
        vals_col.append(str(s.setdefault('oddLotDelayedPrice', '')))
        vals_col.append(str(s.setdefault('oddLotDelayedPriceTime', '')))
        vals_col.append(str(s.setdefault('open', '')))
        vals_col.append(str(s.setdefault('openSource', None).replace('`', '')))
        vals_col.append(str(s.setdefault('openTime', '')))
        vals_col.append(str(s.setdefault('peRatio', '')))
        vals_col.append(str(s.setdefault('previousClose', '')))
        vals_col.append(str(s.setdefault('previousVolume', '')))
        vals_col.append(str(s.setdefault('primaryExchange', None).replace('`', '')))
        vals_col.append(str(s.setdefault('symbol', None).replace('`', '')))
        vals_col.append(str(s.setdefault('volume', '')))
        vals_col.append(str(s.setdefault('week52High', '')))
        vals_col.append(str(s.setdefault('week52Low', '')))
        vals_col.append(str(s.setdefault('ytdChange', '')))
    except Exception as ex:
        log.logmsg(str(ex))
        log.logmsg(whom)



    return '`'.join(vals_col).replace('None', '')


def build_ins(s):

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
    val_tmp = val_fix(s.setdefault('week52Low', None))
    ins_val += str('NULL' if val_tmp is None else val_tmp) + ','

    ins_col += '[week52High],'
    val_tmp = val_fix(s.setdefault('week52High', None))
    ins_val += str('NULL' if val_tmp is None else val_tmp) + ','

    ins_col += '[latestUpdate],'
    val_tmp = val_fix(s.setdefault('latestUpdate', None))
    ins_val += str('NULL' if val_tmp is None else val_tmp) + ','

    ins_col += '[latestVolume],'
    val_tmp = val_fix(s.setdefault('latestVolume', None))
    ins_val += str('NULL' if val_tmp is None else val_tmp) + ','

    ins_col += '[openTime],'
    val_tmp = val_fix(s.setdefault('openTime', None))
    ins_val += str('NULL' if val_tmp is None else val_tmp) + ','

    ins_col += '[peRatio],'
    val_tmp = val_fix(s.setdefault('peRatio', None))
    ins_val += str('NULL' if val_tmp is None else val_tmp) + ','

    ins_col += '[close],'
    val_tmp = val_fix(s.setdefault('close', None))
    ins_val += str('NULL' if val_tmp is None else val_tmp) + ','

    ins_col += '[marketCap],'
    val_tmp = val_fix(s.setdefault('marketCap', None))
    ins_val += str('NULL' if val_tmp is None else val_tmp) + ','

    ins_col += '[latestPrice],'
    val_tmp = val_fix(s.setdefault('latestPrice', None))
    ins_val += str('NULL' if val_tmp is None else val_tmp) + ','

    ins_col += '[low],'
    val_tmp = val_fix(s.setdefault('low', None))
    ins_val += str('NULL' if val_tmp is None else val_tmp) + ','

    ins_col += '[lowTime],'
    val_tmp = val_fix(s.setdefault('lowTime', None))
    ins_val += str('NULL' if val_tmp is None else val_tmp) + ','

    ins_col += '[extendedPriceTime],'
    val_tmp = val_fix(s.setdefault('extendedPriceTime', None))
    ins_val += str('NULL' if val_tmp is None else val_tmp) + ','

    ins_col += '[high],'
    val_tmp = val_fix(s.setdefault('high', None))
    ins_val += str('NULL' if val_tmp is None else val_tmp) + ','

    ins_col += '[extendedChangePercent],'
    val_tmp = val_fix(s.setdefault('extendedChangePercent', None))
    ins_val += str('NULL' if val_tmp is None else val_tmp) + ','

    ins_col += '[oddLotDelayedPriceTime],'
    val_tmp = val_fix(s.setdefault('oddLotDelayedPriceTime', None))
    ins_val += str('NULL' if val_tmp is None else val_tmp) + ','

    ins_col += '[open],'
    val_tmp = val_fix(s.setdefault('open', None))
    ins_val += str('NULL' if val_tmp is None else val_tmp) + ','

    ins_col += '[change],'
    val_tmp = val_fix(s.setdefault('change', None))
    ins_val += str('NULL' if val_tmp is None else val_tmp) + ','

    ins_col += '[avgTotalVolume],'
    val_tmp = val_fix(s.setdefault('avgTotalVolume', None))
    ins_val += str('NULL' if val_tmp is None else val_tmp) + ','

    ins_col += '[changePercent],'
    val_tmp = val_fix(s.setdefault('changePercent', None))
    ins_val += str('NULL' if val_tmp is None else val_tmp) + ','

    ins_col += '[previousVolume],'
    val_tmp = val_fix(s.setdefault('previousVolume', None))
    ins_val += str('NULL' if val_tmp is None else val_tmp) + ','

    ins_col += '[volume],'
    val_tmp = val_fix(s.setdefault('volume', None))
    ins_val += str('NULL' if val_tmp is None else val_tmp) + ','

    ins_col += '[ytdChange],'
    val_tmp = val_fix(s.setdefault('ytdChange', None))
    ins_val += str('NULL' if val_tmp is None else val_tmp) + ','

    ins_col += '[extendedPrice],'
    val_tmp = val_fix(s.setdefault('extendedPrice', None))
    ins_val += str('NULL' if val_tmp is None else val_tmp) + ','

    ins_col += '[highTime],'
    val_tmp = val_fix(s.setdefault('highTime', None))
    ins_val += str('NULL' if val_tmp is None else val_tmp) + ','

    ins_col += '[delayedPriceTime],'
    val_tmp = val_fix(s.setdefault('delayedPriceTime', None))
    ins_val += str('NULL' if val_tmp is None else val_tmp) + ','

    ins_col += '[extendedChange],'
    val_tmp = val_fix(s.setdefault('extendedChange', None))
    ins_val += str('NULL' if val_tmp is None else val_tmp) + ','

    ins_col += '[closeTime],'
    val_tmp = val_fix(s.setdefault('closeTime', None))
    ins_val += str('NULL' if val_tmp is None else val_tmp) + ','

    ins_col += '[delayedPrice],'
    val_tmp = val_fix(s.setdefault('delayedPrice', None))
    ins_val += str('NULL' if val_tmp is None else val_tmp) + ','

    ins_col += '[oddLotDelayedPrice],'
    val_tmp = val_fix(s.setdefault('oddLotDelayedPrice', None))
    ins_val += str('NULL' if val_tmp is None else val_tmp) + ','

    ins_col += '[previousClose],'
    val_tmp = val_fix(s.setdefault('previousClose', None))
    ins_val += str('NULL' if val_tmp is None else val_tmp) + ','

    ins_col += '[lastTradeTime]'
    val_tmp = val_fix(s.setdefault('lastTradeTime', None))
    ins_val += str('NULL' if val_tmp is None else val_tmp)


    return ins_col, ins_val

def val_fix(val):

    fxval = val

    if val is None:
        fxval = 'NULL'

    fxval = str(fxval).replace(' ', '')

    return fxval

def quote_surr(val):
    return "'" + str(val).replace("'", "''").replace(' ', '') + "'"

def abbr(str):
    return ''.join(list(map(lambda x: x[0], str.split(' '))))

def chunks(lst, n):
    for i in range(0, len(lst), n):
        yield lst[i:i + n]


if (len(sys.argv) > 1):

    with_requests(sys.argv[1], sys.argv[2])

