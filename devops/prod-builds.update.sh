/srv/dumps.sh
systemctl stop web.app.service comm.app.service tg.app.service api.app.service bus.app.service filesindexing.app.service constructor.app.service hd.app.service identity.app.service kladr.app.service ldap.app.service bank.app.service
7z a /srv/Cloud.Disk/services-snapshots/all_services_`date +%Y-%m-%d"_"%H_%M_%S`.7z /srv/services/*
rm -r /srv/services/*
rsync -av --exclude='appsettings.*' /srv/git/builds/ /srv/services/
chown -R www-data:www-data /srv/services
chmod -R 777 /srv/services
systemctl start comm.app.service web.app.service bus.app.service filesindexing.app.service tg.app.service api.app.service hd.app.service constructor.app.service identity.app.service kladr.app.service ldap.app.service bank.app.service