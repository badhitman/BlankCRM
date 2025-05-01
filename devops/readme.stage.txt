STAGING

systemctl stop api.app.stage.service web.app.stage.service comm.app.stage.service tg.app.stage.service bus.app.stage.service constructor.app.stage.service hd.app.stage.service

# rm -r /srv/git/builds/*
rm -r /srv/services.stage/*

cp -r /srv/git/builds/IdentityService /srv/services.stage/IdentityService
cp -r /srv/git/builds/ApiRestService /srv/services.stage/ApiRestService
cp -r /srv/git/builds/StorageService /srv/services.stage/StorageService
cp -r /srv/git/builds/CommerceService /srv/services.stage/CommerceService
cp -r /srv/git/builds/HelpDeskService /srv/services.stage/HelpDeskService
cp -r /srv/git/builds/ConstructorService /srv/services.stage/ConstructorService
cp -r /srv/git/builds/TelegramBotService /srv/services.stage/TelegramBotService
cp -r /srv/git/builds/KladrService /srv/services.stage/KladrService
cp -r /srv/git/builds/LdapService /srv/services.stage/LdapService
cp -r /srv/git/builds/BlankBlazorApp /srv/services.stage/BlankBlazorApp
cp -r /srv/git/builds/ApiBreezRuService /srv/services.stage/ApiBreezRuService
cp -r /srv/git/builds/ApiDaichiBusinessService /srv/services.stage/ApiDaichiBusinessService
cp -r /srv/git/builds/ApiRusklimatComService /srv/services.stage/ApiRusklimatComService
cp -r /srv/git/builds/FeedsHaierProffRuService /srv/services.stage/FeedsHaierProffRuService

chown -R www-data:www-data /srv/services.stage
chmod -R 777 /srv/services.stage

chown -R www-data:www-data /srv/git/builds
chmod -R 777 /srv/git/builds

systemctl start api.app.stage.service comm.app.stage.service web.app.stage.service bus.app.stage.service tg.app.stage.service hd.app.stage.service constructor.app.stage.service


journalctl -f -u web.app.stage.service
systemctl status constructor.app.stage.service