/srv/dumps.sh
systemctl stop web.app.service comm.app.service tg.app.service api.app.service bus.app.service constructor.app.service hd.app.service identity.app.service kladr.app.service ldap.app.service
7z a /srv/Cloud.Disk/services-snapshots/all_services_`date +%Y-%m-%d"_"%H_%M_%S`.7z /srv/services/*
rm -r /srv/services/*
cp -r /srv/git/builds/ApiRestService /srv/services/ApiRestService
cp -r /srv/git/builds/KladrService /srv/services/KladrService
cp -r /srv/git/builds/LdapService /srv/services/LdapService
cp -r /srv/git/builds/StorageService /srv/services/StorageService
cp -r /srv/git/builds/CommerceService /srv/services/CommerceService
cp -r /srv/git/builds/HelpDeskService /srv/services/HelpDeskService
cp -r /srv/git/builds/ConstructorService /srv/services/ConstructorService
cp -r /srv/git/builds/TelegramBotService /srv/services/TelegramBotService
cp -r /srv/git/builds/BlankBlazorApp /srv/services/BlankBlazorApp
cp -r /srv/git/builds/IdentityService /srv/services/IdentityService

chown -R www-data:www-data /srv/services
chmod -R 777 /srv/services
systemctl start comm.app.service web.app.service bus.app.service tg.app.service api.app.service hd.app.service constructor.app.service identity.app.service kladr.app.service ldap.app.service