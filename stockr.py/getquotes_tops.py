import sys
import time
import _conf as cf
import urllib.request, json 

import db.log as log
from db.dbrep import dbrep

dbsymins = dbrep('Quote_Stg')

def logwrite(msg, logit = True):
    
    print(f'{time.strftime("%Y-%m-%d %H:%M:%S")} : {msg}')

    if logit: 
        log.logmsg(msg)

def quote_surr(val):
    return "'" + str(val).replace("'", "''").replace(' ', '') + "'"

def build_ins(q):

    ins_val = '('

    val_tmp = q.setdefault('symbol', None)
    ins_val += quote_surr(str('NULL' if val_tmp is None else val_tmp.strip())) + ','

    val_tmp = q.setdefault('sector', None)
    ins_val += quote_surr(str('NULL' if val_tmp is None else val_tmp.strip())) + ','

    val_tmp = q.setdefault('securityType', None)
    ins_val += quote_surr(str('NULL' if val_tmp is None else val_tmp.strip())) + ','

    val_tmp = q.setdefault('bidPrice', None)
    ins_val += str('NULL' if val_tmp is None else val_tmp) + ','

    val_tmp = q.setdefault('bidSize', None)
    ins_val += str('NULL' if val_tmp is None else val_tmp) + ','

    val_tmp = q.setdefault('askPrice', None)
    ins_val += str('NULL' if val_tmp is None else val_tmp) + ','

    val_tmp = q.setdefault('askSize', None)
    ins_val += str('NULL' if val_tmp is None else val_tmp) + ','

    val_tmp = q.setdefault('lastUpdated', None)
    ins_val += str('NULL' if val_tmp is None else val_tmp) + ','

    val_tmp = q.setdefault('lastSalePrice', None)
    ins_val += str('NULL' if val_tmp is None else val_tmp) + ','

    val_tmp = q.setdefault('lastSaleSize', None)
    ins_val += str('NULL' if val_tmp is None else val_tmp) + ','

    val_tmp = q.setdefault('lastSaleTime', None)
    ins_val += str('NULL' if val_tmp is None else val_tmp) + ','

    val_tmp = q.setdefault('volume', None)
    ins_val += str('NULL' if val_tmp is None else val_tmp) + ','

    val_tmp = q.setdefault('marketPercent', None)
    ins_val += str('NULL' if val_tmp is None else val_tmp)

    ins_val += ')'

    return ins_val 

def build_ins_topslast(q):

    ins_val = '('

    val_tmp = q.setdefault('symbol', None)
    ins_val += quote_surr(str('NULL' if val_tmp is None else val_tmp.strip())) + ','

    val_tmp = q.setdefault('price', None)
    ins_val += str('NULL' if val_tmp is None else val_tmp) + ','

    val_tmp = q.setdefault('time', None)
    ins_val += str('NULL' if val_tmp is None else val_tmp) + ','

    val_tmp = q.setdefault('size', None)
    ins_val += str('NULL' if val_tmp is None else val_tmp)

    ins_val += ')'

    return ins_val 

def insert_data(cols, qs):
    vals = ','.join(qs)    
    dbsymins.insert_row(cols, vals[1:-1])

def tops_quote():

    ii = 0

    with urllib.request.urlopen("https://api.iextrading.com/1.0/tops") as url:
        
        quotes = json.loads(url.read().decode())

        msg = f'tops ct {len(quotes)}'
        logwrite(msg)
    
        qs = []

        for q in quotes:
            
            qs.append(build_ins(q))
            ii += 1

            if ii == 999:

                insert_data('', qs)
                logwrite(f'  {len(qs)}', False)

                qs = []
                ii = 0

        if len(qs) > 0:
            insert_data('', qs)
            logwrite(f'  {len(qs)}', False)

    dbsymins.sp_exec('EXEC spQuotesConsolidation_Tops')


def topslast_quote():

    ii = 0

    ins_cols = 'symbol, lastSalePrice, lastUpdated, volume'

    with urllib.request.urlopen("https://api.iextrading.com/1.0/tops/last") as url:
        
        quotes = json.loads(url.read().decode())

        msg = f'tops last ct {len(quotes)}'
        logwrite(msg)

        qs = []

        for q in quotes:
            
            qs.append(build_ins_topslast(q))
            ii += 1

            if ii == 999:

                insert_data(ins_cols, qs)
                logwrite(f'  {len(qs)}', False)

                qs = []
                ii = 0

        if len(qs) > 0:
            insert_data(ins_cols, qs)
            logwrite(f'  {len(qs)}', False)

    # dbsymins.sp_exec('EXEC spQuotesConsolidation_TopsLast')


#if __name__ == '__main__':
#    tops_quote()

topsct = 0
while True:
    
    try:

        if topsct == 0:
            # tops_quote()
            topsct = cf.tops_freq

        topslast_quote()
        
        topsct -= 1

    except:
        err = f'error : {sys.exc_info()}'
        logwrite(err)

    time.sleep(cf.interval)

