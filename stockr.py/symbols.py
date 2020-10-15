import _conf as cf
import _secrets as sec

import urllib.request, json
import pyodbc 

import db.log as log

def symbols_load():

    counter = 0
    response = urllib.request.urlopen(cf.url_symbols_all)
    symbols = json.loads(response.read())

    log.logmsg("sym ct - " + str(len(symbols)))

    conn = pyodbc.connect(sec.db_conn)
    cursor = conn.cursor()

    cursor.execute('TRUNCATE TABLE dbo.Symbol_Stg')
    cursor.commit()


    insert_stmt = 'insert into [dbo].[Symbol_Stg] ([symbol], [exchange], [name], [date], [type], [iexId], [region], [currency], [isEnabled], [cik], [figi]) values '
    insert_vals = []

    for s in symbols:

        insert = "('%s', '%s', '%s', '%s', '%s', '%s', '%s', '%s', %s, '%s', '%s')" %(
            s.setdefault('symbol', ''),
            s.setdefault('exchange', ''),
            s.setdefault('name', '').replace("'", "''"),
            s.setdefault('date', ''),
            s.setdefault('type', ''),
            s.setdefault('iexId', ''),
            s.setdefault('region', ''),
            s.setdefault('currency', ''),
            1 if s["isEnabled"] else 0,
            s.setdefault('cik', ''),
            s.setdefault('figi', ''))

        insert_vals.append(insert)
    
        if counter == 998:

            cursor.execute(insert_stmt + (",".join(insert_vals)))
            cursor.commit()

            log.logmsg("load sym - " + str(len(insert_vals)))

            counter = 0
            insert_vals = []

        counter+=1

    if len(insert_vals) > 0:
        cursor.execute(insert_stmt + (",".join(insert_vals)))
        cursor.commit()

        log.logmsg("load sym - " + str(len(insert_vals)))

    cursor.execute("EXEC dbo.spSymbolsConsolidation")
    cursor.commit()
    conn.close()

symbols_load()