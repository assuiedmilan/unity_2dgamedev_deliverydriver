call bokken-cli job create -i %1
call bokken-cli job wait booted .
call bokken-cli connect rdp .