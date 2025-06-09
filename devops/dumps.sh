rm -r /srv/tmp/dumps/*
docker exec -t srv-postgres-1 pg_dump -c -U dev NLogsContext --format=p --encoding=UTF-8 --inserts | gzip > /srv/tmp/dumps/dump_NLogs_`date +%Y-%m-%d"_"%H_%M_%S`.sql.gz
docker exec -t srv-postgres-1 pg_dump -c -U dev KladrContext --format=p --encoding=UTF-8 --inserts | gzip > /srv/tmp/dumps/dump_Kladr_`date +%Y-%m-%d"_"%H_%M_%S`.sql.gz
docker exec -t srv-postgres-1 pg_dump -c -U dev MainAppContext --format=p --encoding=UTF-8 --inserts | gzip > /srv/tmp/dumps/dump_MainApp_`date +%Y-%m-%d"_"%H_%M_%S`.sql.gz
docker exec -t srv-postgres-1 pg_dump -c -U dev CommerceContext --format=p --encoding=UTF-8 --inserts | gzip > /srv/tmp/dumps/dump_Commerce_`date +%Y-%m-%d"_"%H_%M_%S`.sql.gz
docker exec -t srv-postgres-1 pg_dump -c -U dev HelpDeskContext --format=p --encoding=UTF-8 --inserts | gzip > /srv/tmp/dumps/dump_HelpDesk_`date +%Y-%m-%d"_"%H_%M_%S`.sql.gz
docker exec -t srv-postgres-1 pg_dump -c -U dev StorageContext --format=p --encoding=UTF-8 --inserts | gzip > /srv/tmp/dumps/dump_Storage_`date +%Y-%m-%d"_"%H_%M_%S`.sql.gz
docker exec -t srv-postgres-1 pg_dump -c -U dev TelegramBotContext --format=p --encoding=UTF-8 --inserts | gzip > /srv/tmp/dumps/dump_TelegramBot_`date +%Y-%m-%d"_"%H_%M_%S`.sql.gz
docker exec -t srv-postgres-1 pg_dump -c -U dev IdentityContext --format=p --encoding=UTF-8 --inserts | gzip > /srv/tmp/dumps/dump_Identity_`date +%Y-%m-%d"_"%H_%M_%S`.sql.gz
docker exec -t srv-postgres-1 pg_dump -c -U dev ConstructorContext --format=p --encoding=UTF-8 --inserts | gzip > /srv/tmp/dumps/dump_Constructor_`date +%Y-%m-%d"_"%H_%M_%S`.sql.gz
docker exec -t srv_mongo_1 mongodump --db files-system -u dev -p dev --authenticationDatabase admin --out /srv/tmp/dumps/mongodump/`date +"%Y-%m-%d"_"%H_%M_%S"`
7z a /srv/Cloud.Disk/db-backups/all_dumps_`date +%Y-%m-%d"_"%H_%M_%S`.7z /srv/tmp/dumps/*