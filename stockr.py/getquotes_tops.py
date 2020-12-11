import sys
import time
import _conf as cf
import urllib.request, json 

import db.log as log
from db.dbrep import dbrep

dbsymins = dbrep('Quote_Stg')
zzz = 1

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

def insert_data(qs):
    vals = ','.join(qs)    
    dbsymins.insert_row('', vals[1:-1])

def tops_quote():

    ii = 0

    with urllib.request.urlopen("https://api.iextrading.com/1.0/tops") as url:
        
        quotes = json.loads(url.read().decode())

        msg = f'tops ct {len(quotes)}'
        print(f'{zzz} : {msg}')
        log.logmsg(msg)
        zzz += 1

        qs = []

        for q in quotes:
            
            qs.append(build_ins(q))
            ii += 1

            if ii == 990:

                insert_data(qs)

                qs = []
                ii = 0

        if len(qs) > 0:
            insert_data(qs)

    dbsymins.sp_exec('EXEC spQuotesConsolidation')

#if __name__ == '__main__':
#    tops_quote()

while True:
    #print(f"{str(zzz).zfill(5)} :: {cf.interval}")
    #zzz += 1   

    try:
        tops_quote()
    except:
        err = f'error : {sys.exc_info()}'
        log.logmsg(err)

    time.sleep(cf.interval)

