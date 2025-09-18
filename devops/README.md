## Startup - Ubuntu 24.04 LTS

Prepare
```
apt update -y && apt upgrade -y && apt dist-upgrade -y
apt install -y nmon wget nano ufw nginx git p7zip-rar p7zip-full emscripten zlib1g
```

UFW (Uncomplicated Firewall) simple configuration
```
ufw allow 22
ufw allow 443
ufw allow 80
ufw enable
```

#### .NET9
ref - https://learn.microsoft.com/ru-ru/dotnet/core/install/linux-ubuntu-install?tabs=dotnet9&pivots=os-linux-ubuntu-2404
```
sudo add-apt-repository ppa:dotnet/backports
sudo apt update && sudo apt install -y dotnet-sdk-9.0
```

#### Docker
ref - https://docs.docker.com/engine/install/ubuntu/
1. Set up Docker's apt repository.
```
sudo apt-get install ca-certificates curl
sudo install -m 0755 -d /etc/apt/keyrings
sudo curl -fsSL https://download.docker.com/linux/ubuntu/gpg -o /etc/apt/keyrings/docker.asc
sudo chmod a+r /etc/apt/keyrings/docker.asc

# Add the repository to Apt sources:
echo \
  "deb [arch=$(dpkg --print-architecture) signed-by=/etc/apt/keyrings/docker.asc] https://download.docker.com/linux/ubuntu \
  $(. /etc/os-release && echo "${UBUNTU_CODENAME:-$VERSION_CODENAME}") stable" | \
  sudo tee /etc/apt/sources.list.d/docker.list > /dev/null
sudo apt-get update
```
2. Install the Docker packages.
```
sudo apt-get install docker-ce docker-ce-cli containerd.io docker-buildx-plugin docker-compose-plugin
```

check
```
docker run hello-world
```

for fix error (if an error occurs) `Package docker-ce-cli is not available, but is referred to by another package.` (origin doc - https://forums.docker.com/t/installing-docker-on-buster-e-package-docker-ce-has-no-installation-candidate/108397/16)
```
curl -fsSL https://download.docker.com/linux/ubuntu/gpg | sudo gpg --dearmor -o  /usr/share/keyrings/docker-archive-keyring.gpg
echo "deb [arch=amd64 signed-by=/usr/share/keyrings/docker-archive-keyring.gpg] https://download.docker.com/linux/ubuntu  $(lsb_release -cs) stable" | sudo tee /etc/apt/sources.list.d/docker.list > /dev/null
sudo apt-get update
```

and retry check `docker run hello-world` (if required)

#### Lazydocker
ref - https://lindevs.com/install-lazydocker-on-ubuntu
```
LAZYDOCKER_VERSION=$(curl -s "https://api.github.com/repos/jesseduffield/lazydocker/releases/latest" | grep -Po '"tag_name": "v\K[0-9.]+')
curl -Lo lazydocker.tar.gz "https://github.com/jesseduffield/lazydocker/releases/latest/download/lazydocker_${LAZYDOCKER_VERSION}_Linux_x86_64.tar.gz"
mkdir lazydocker-temp
tar xf lazydocker.tar.gz -C lazydocker-temp
sudo mv lazydocker-temp/lazydocker /usr/local/bin
```

check Lazydocker version:
```
lazydocker --version
```

Archive and temporary directory is no longer necessary, remove them:
```
rm -rf lazydocker.tar.gz lazydocker-temp
```

#### ufw-docker
ref - https://github.com/chaifeng/ufw-docker?tab=readme-ov-file#install
```
sudo wget -O /usr/local/bin/ufw-docker \
  https://github.com/chaifeng/ufw-docker/raw/master/ufw-docker
sudo chmod 755 /usr/local/bin/ufw-docker
ufw-docker install
```

check
```
sudo ufw-docker check
```

#### Directories
```
mkdir -p /srv/secrets /srv/db-backups /srv/git /srv/services /srv/services.stage /srv/Cloud.Disk /srv/tmp/dumps /root/.vs-debugger
```

```
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
chown -R www-data:www-data /srv/secrets
chmod -R 777 /srv/secrets
```

#### Secrets base (sample files)
```
cd /srv/secrets
wget https://raw.githubusercontent.com/badhitman/BlankCRM/refs/heads/main/devops/secrets/api-access.json
wget https://raw.githubusercontent.com/badhitman/BlankCRM/refs/heads/main/devops/secrets/connections-strings.Development.json
wget https://raw.githubusercontent.com/badhitman/BlankCRM/refs/heads/main/devops/secrets/email-conf.Development.json
wget https://raw.githubusercontent.com/badhitman/BlankCRM/refs/heads/main/devops/secrets/mongo-conf.json
wget https://raw.githubusercontent.com/badhitman/BlankCRM/refs/heads/main/devops/secrets/rabbitmq-conf.Development.json
wget https://raw.githubusercontent.com/badhitman/BlankCRM/refs/heads/main/devops/secrets/telegram-bot.Development.json
wget https://raw.githubusercontent.com/badhitman/BlankCRM/refs/heads/main/devops/secrets/top-menu.Development.json
```
edit these files for your infrastructure - logins, passwords, etc.

#### Sources + Builds
```
cd /srv/git
rm -r *
git clone https://github.com/badhitman/BlankCRM.git
git clone https://github.com/badhitman/HtmlGenerator.git
cd /srv/git/BlankCRM/BlankBlazorApp/BlankBlazorApp
dotnet workload install wasm-tools
dotnet workload restore
# sudo ln -s ~/.dotnet/tools/libman /usr/local/bin/libman
dotnet tool install -g Microsoft.Web.LibraryManager.Cli
libman restore
# Debug/Release
dotnet publish -c Debug --output /srv/git/builds/ApiRestService /srv/git/BlankCRM/micro-services/ApiRestService/ApiRestService.csproj
dotnet publish -c Debug --output /srv/git/builds/StorageService /srv/git/BlankCRM/micro-services/StorageService/StorageService.csproj
dotnet publish -c Debug --output /srv/git/builds/CommerceService /srv/git/BlankCRM/micro-services/CommerceService/CommerceService.csproj
dotnet publish -c Debug --output /srv/git/builds/BankService /srv/git/BlankCRM/micro-services/BankService/BankService.csproj
dotnet publish -c Debug --output /srv/git/builds/HelpDeskService /srv/git/BlankCRM/micro-services/HelpDeskService/HelpDeskService.csproj
dotnet publish -c Debug --output /srv/git/builds/ConstructorService /srv/git/BlankCRM/micro-services/ConstructorService/ConstructorService.csproj
dotnet publish -c Debug --output /srv/git/builds/TelegramBotService /srv/git/BlankCRM/micro-services/TelegramBotService/TelegramBotService.csproj
dotnet publish -c Debug --output /srv/git/builds/KladrService /srv/git/BlankCRM/micro-services/KladrService/KladrService.csproj
dotnet publish -c Debug --output /srv/git/builds/LdapService /srv/git/BlankCRM/micro-services/LdapService/LdapService.csproj
dotnet publish -c Debug --output /srv/git/builds/IdentityService /srv/git/BlankCRM/micro-services/IdentityService/IdentityService.csproj
dotnet publish -c Debug --output /srv/git/builds/BlankBlazorApp /srv/git/BlankCRM/BlankBlazorApp/BlankBlazorApp/BlankBlazorApp.csproj
```

#### Scripts
```
cd /srv/git/BlankCRM/devops/
cp docker-compose.yml dumps.sh prod-builds.update.sh stage-builds.update.sh /srv/
chown -R www-data:www-data /srv/dumps.sh
chmod -R 755 /srv/dumps.sh
chown -R www-data:www-data /srv/prod-builds.update.sh
chmod -R 755 /srv/prod-builds.update.sh
chown -R www-data:www-data /srv/stage-builds.update.sh
chmod -R 755 /srv/stage-builds.update.sh
```

#### Systemd
```
cd /srv/git/BlankCRM/devops/etc/systemd/system/
cp docker-compose-app.service api.app.stage.service bus.app.stage.service comm.app.stage.service bank.app.stage.service constructor.app.stage.service hd.app.stage.service identity.app.stage.service kladr.app.stage.service ldap.app.stage.service tg.app.stage.service web.app.stage.service /etc/systemd/system/

systemctl daemon-reload

systemctl enable docker-compose-app.service
systemctl start docker-compose-app.service

systemctl enable api.app.stage.service
systemctl enable web.app.stage.service
systemctl enable comm.app.stage.service
systemctl enable bank.app.stage.service
systemctl enable tg.app.stage.service
systemctl enable bus.app.stage.service
systemctl enable constructor.app.stage.service
systemctl enable identity.app.stage.service
systemctl enable hd.app.stage.service
systemctl enable ldap.app.stage.service
systemctl enable kladr.app.stage.service

systemctl start api.app.stage.service
systemctl start bus.app.stage.service
systemctl start comm.app.stage.service
systemctl start bank.app.stage.service
systemctl start constructor.app.stage.service
systemctl start hd.app.stage.service
systemctl start identity.app.stage.service
systemctl start kladr.app.stage.service
systemctl start ldap.app.stage.service
systemctl start tg.app.stage.service
systemctl start web.app.stage.service
```

`journalctl -f -u api.app.stage.service`

#### Nginx
```
cd /srv/git/BlankCRM/devops/etc/nginx/
cp mongo.express /etc/nginx/sites-available
cd /srv/git/BlankCRM/devops/etc/nginx/stage/
cp api.staging.app web.staging.app /etc/nginx/sites-available
ln -s /etc/nginx/sites-available/web.staging.app /etc/nginx/sites-enabled/
ln -s /etc/nginx/sites-available/api.staging.app /etc/nginx/sites-enabled/
ln -s /etc/nginx/sites-available/mongo.express /etc/nginx/sites-enabled/
systemctl reload nginx
```

#### Update services (stage + prod)
```
/srv/stage-builds.update.sh
/srv/prod-builds.update.sh
```

#### Win
```
docker run -e RABBITMQ_DEFAULT_USER=guest -e RABBITMQ_DEFAULT_PASS=guest -p 15671:15671  -p 15672:15672  -p 15691:15691  -p 15692:15692  -p 25672:25672  -p 4369:4369  -p 5671:5671  -p 5672:5672  -p 5670:5670 rabbitmq:3-management
```
