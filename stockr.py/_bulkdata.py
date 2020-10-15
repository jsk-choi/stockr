import os
import glob
import _conf as cf

bulkfile = os.path.join(cf.bulk_file_path, cf.bulk_file)
datfiles = glob.glob(os.path.join(cf.bulk_file_path, '*.dat'))

print(bulkfile)
print(datfiles)

for dat in datfiles:

    os.rename(dat, dat + 'do')
    os.system(' > ' + dat)

#if os.path.exists(bulkfile):
#    os.system(' > ' & bulkfile) 

combinebash = 'cat %s*.datdo > %s'%(cf.bulk_file_path, bulkfile)
delfiles = 'rm %s*.datdo'%(cf.bulk_file_path)

print(combinebash)
print(delfiles)
os.system(combinebash)
os.system(delfiles)
