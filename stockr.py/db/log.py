import _secrets as sec
import pyodbc 
import time
import os

def logtext(msg):

    date_time = time.strftime("%Y-%m-%d %H:%M:%S")
    date_str = time.strftime("%Y%m%d")
    flename = f"log_{date_str}.txt"

    if not os.path.exists(flename):
        open(flename, "w+").close()

    open(flename, "a+").write(f'{date_time} : {msg}\n')


def logmsg(msg):

    # msg = msg if len(msg) < 200 else msg[0:195] + ' ...'
    msg = msg.replace("'", "''")[0:2500]

    try:
        conn = pyodbc.connect(sec.db_conn, timeout = 2)
        with conn:
            cursor = conn.cursor()
            with cursor:
                insert_stmt = f"insert into [dbo].[Log] (SystemTime, Msg, Catg) values (getdate(), '{msg}', 'pyfo')"
                cursor.execute(insert_stmt)
                cursor.commit()

    except Exception as ex:
        logtext(f"error: {ex}\n\t{msg}")



logmsg('durrf')
logmsg('again')
