import db.log as log
import threading
  
def print_cube(i):
    strrr = 'proc ' + str(i)
    print(strrr)
    log.logmsg(strrr)

for i in range(10):
    threading.Thread(target=print_cube, args=(i,)).start()

#drrr = ''
#for i in range(1000):
#    drrr += str(i)

#print(drrr[0:400])
