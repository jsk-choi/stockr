import _secrets as sec
import pyodbc 

def logmsg(msg):

    # msg = msg if len(msg) < 200 else msg[0:195] + ' ...'
    msg = msg.replace("'", "''")

    conn = pyodbc.connect(sec.db_conn)
    with conn:
        cursor = conn.cursor()
        with cursor:
            insert_stmt = f"insert into [dbo].[Log] (SystemTime, Msg) values (getdate(), '{msg}')"
            cursor.execute(insert_stmt)
            cursor.commit()
