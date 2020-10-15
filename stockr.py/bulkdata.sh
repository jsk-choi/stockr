for file in /tmp/bulk/*.dat
do
	mv $file $file'do'
	> $file
done

cat /tmp/bulk/*.datdo > /mnt/jfs/bulk/sqlbulk.sqldat
rm /tmp/bulk/*.datdo
