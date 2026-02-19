/srv/dumps.sh
systemctl stop web.app.service comm.app.service tg.app.service api.app.service bus.app.service realtime.app.service indexing.app.service constructor.app.service hd.app.service identity.app.service kladr.app.service ldap.app.service bank.app.service
7z a /srv/Cloud.Disk/services-snapshots/all_services_`date +%Y-%m-%d"_"%H_%M_%S`.7z /srv/services.prod/*
find /srv/services.prod/ -type f -name '*.dll' -delete
find /srv/services.prod/ -type f -name '*.pdb' -delete
find /srv/services.prod/ -type f -name '*.exe' -delete
find /srv/services.prod/ -type f -name '*.log' -delete
rsync -av --exclude='appsettings.*' --exclude='nlog.config' /srv/git/builds/ /srv/services.prod/
chown -R www-data:www-data /srv/services.prod
chmod -R 777 /srv/services.prod
systemctl start comm.app.service web.app.service bus.app.service realtime.app.service indexing.app.service tg.app.service api.app.service hd.app.service constructor.app.service identity.app.service kladr.app.service ldap.app.service bank.app.service