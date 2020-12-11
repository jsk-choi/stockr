import _secrets as sec

url_symbols_all = 'https://cloud.iexapis.com/stable/ref-data/symbols?token=' + sec.iex_key
#url_quote_sse = 'https://cloud-sse.iexapis.com/stable/stocksUSNoUTP?symbols={}&token=' + sec.iex_key
url_quote_sse = 'https://sandbox-sse.iexapis.com/stable/stocksUSNoUTP?symbols={}&token=' + sec.iex_key


job_kill = 'pkill -f "python3 /mnt/jfs*"'
script_loc = '/mnt/jfs/a/stockr-py/stockr-py'
script_name = 'runall.sh'
#script_command = 'nohup python3 %s/getquotes.py "%s" > /dev/null 2>&1 &'
script_command = 'python3 %s/getquotes.py "%s" %s &'
bulk_file_path = '/tmp/bulk/'
bulk_file = 'sqlbulk.dat'

interval = 20