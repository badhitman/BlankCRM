## DevOps

```
sudo apt-get update -y && sudo apt-get upgrade -y && sudo apt-get dist-upgrade -y && sudo apt-get install git -y

sudo apt-get install wget
sudo apt-get install nano
sudo apt-get install sqlite3

apt-get install ufw
ufw allow 22
ufw allow 443
ufw allow 80
ufw enable

# our .net8

chown -R www-data:www-data /root/.vs-debugger
chmod -R 777 /root/.vs-debugger

chown -R www-data:www-data /srv/db-backups
chmod -R 777 /srv/db-backups

chown -R www-data:www-data /srv/git
chmod -R 777 /srv/git

chown -R www-data:www-data /srv/services
chmod -R 777 /srv/services

chown -R www-data:www-data /srv/services.stage
chmod -R 777 /srv/services.stage

chown -R www-data:www-data /srv/Cloud.Disk
chmod -R 777 /srv/Cloud.Disk

chown -R www-data:www-data /srv/tmp
chmod -R 777 /srv/tmp

# PROD
ln -s /etc/nginx/sites-available/web.app /etc/nginx/sites-enabled/
ln -s /etc/nginx/sites-available/api.app /etc/nginx/sites-enabled/

# STAGING
ln -s /etc/nginx/sites-available/web.staging.app /etc/nginx/sites-enabled/
ln -s /etc/nginx/sites-available/api.staging.app /etc/nginx/sites-enabled/


systemctl reload nginx
docker compose up -d

apt update -y && apt upgrade -y && apt dist-upgrade -y && apt install git -y

cd /srv/git
rm -r *
git clone https://github.com/badhitman/BlankCRM.git
git clone https://github.com/badhitman/HtmlGenerator.git
cd /srv/git/BlankCRM
dotnet tool install -g Microsoft.Web.LibraryManager.Cli
dotnet workload restore
libman restore

# Debug/Release

dotnet publish -c Release --output /srv/git/builds/ApiRestService /srv/git/BlankCRM/micro-services/ApiRestService/ApiRestService.csproj
dotnet publish -c Release --output /srv/git/builds/StorageService /srv/git/BlankCRM/micro-services/StorageService/StorageService.csproj
dotnet publish -c Release --output /srv/git/builds/CommerceService /srv/git/BlankCRM/micro-services/CommerceService/CommerceService.csproj
dotnet publish -c Release --output /srv/git/builds/HelpDeskService /srv/git/BlankCRM/micro-services/HelpDeskService/HelpDeskService.csproj
dotnet publish -c Release --output /srv/git/builds/ConstructorService /srv/git/BlankCRM/micro-services/ConstructorService/ConstructorService.csproj
dotnet publish -c Release --output /srv/git/builds/TelegramBotService /srv/git/BlankCRM/micro-services/TelegramBotService/TelegramBotService.csproj
dotnet publish -c Release --output /srv/git/builds/KladrService /srv/git/BlankCRM/micro-services/KladrService/KladrService.csproj
dotnet publish -c Release --output /srv/git/builds/LdapService /srv/git/BlankCRM/micro-services/LdapService/LdapService.csproj
dotnet publish -c Release --output /srv/git/builds/ApiBreezRuService /srv/git/BlankCRM/micro-services/outer/ApiBreezRuService/ApiBreezRuService.csproj
dotnet publish -c Release --output /srv/git/builds/ApiDaichiBusinessService /srv/git/BlankCRM/micro-services/outer/ApiDaichiBusinessService/ApiDaichiBusinessService.csproj
dotnet publish -c Release --output /srv/git/builds/ApiRusklimatComService /srv/git/BlankCRM/micro-services/outer/ApiRusklimatComService/ApiRusklimatComService.csproj
dotnet publish -c Release --output /srv/git/builds/FeedsHaierProffRuService /srv/git/BlankCRM/micro-services/outer/FeedsHaierProffRuService/FeedsHaierProffRuService.csproj
dotnet publish -c Release --output /srv/git/builds/IdentityService /srv/git/BlankCRM/micro-services/IdentityService/IdentityService.csproj
dotnet publish -c Release --output /srv/git/builds/BlankBlazorApp /srv/git/BlankCRM/BlankBlazorApp/BlankBlazorApp/BlankBlazorApp.csproj

#  *** этот билд требует значительной мощьности железа. на стоковом сервере не соберЄтс€ (ресурсоЄмкий процесс, который веро€тно не сможет корректно завершитьс€)
#  cd /srv/git/BlankCRM/BlankBlazorApp/BlankBlazorApp/
#  dotnet tool install -g Microsoft.Web.LibraryManager.Cli
#  dotnet workload restore
#  libman restore
#  dotnet publish -c Release --output /srv/git/builds/BlankBlazorApp /srv/git/BlankCRM/BlankBlazorApp/BlankBlazorApp/BlankBlazorApp.csproj
#  *** поэтому € его отдельно собираю локально, отправл€ю через sftp, распаковываю и продолжаю дальше буд-то команды корректно отработали

journalctl -e -f -u web.app.stage.service
journalctl -f -u docker-compose-app.service

systemctl status kladr.app.stage.service
systemctl status ldap.app.stage.service
systemctl status identity.app.stage.service
systemctl status constructor.app.stage.service
systemctl status hd.app.stage.service
systemctl status api.app.stage.service
systemctl status tg.app.stage.service
systemctl status bus.app.stage.service
systemctl status web.app.stage.service
systemctl status comm.app.stage.service
systemctl status feeds-haier-proff-ru-service.app.stage.service
systemctl status api-rusklimat-com-service.app.stage.service
systemctl status api-daichi-business-service.app.stage.service
systemctl status api-breez-ru-service.app.stage.service

```

#### Win
```
docker run -e RABBITMQ_DEFAULT_USER=guest -e RABBITMQ_DEFAULT_PASS=guest -p 15671:15671  -p 15672:15672  -p 15691:15691  -p 15692:15692  -p 25672:25672  -p 4369:4369  -p 5671:5671  -p 5672:5672  -p 5670:5670 rabbitmq:3-management
```