systemctl stop web.app.stage.service comm.app.stage.service tg.app.stage.service api.app.stage.service bus.app.stage.service filesindexing.app.stage.service constructor.app.stage.service identity.app.stage.service hd.app.stage.service ldap.app.stage.service kladr.app.stage.service bank.app.service
rm -r /srv/services.stage/*
rsync -av --exclude='appsettings.*' --exclude='nlog.config' /srv/git/builds/ /srv/services.stage/
chown -R www-data:www-data /srv/services.stage
chmod -R 777 /srv/services.stage
systemctl start comm.app.stage.service web.app.stage.service bus.app.stage.service filesindexing.app.stage.service tg.app.stage.service api.app.stage.service hd.app.stage.service constructor.app.stage.service identity.app.stage.service ldap.app.stage.service kladr.app.stage.service bank.app.service