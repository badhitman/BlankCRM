## Startup

```
apt update -y && apt upgrade -y && apt dist-upgrade -y && apt install git -y && apt install -y emscripten
apt install -y nmon && apt install -y wget && apt install -y nano && apt install -y ufw && apt install -y nginx
```

```
ufw allow 22
ufw allow 443
ufw allow 80
ufw enable
```

#### .NET9 - Ubuntu 24.04 LTS
install - https://learn.microsoft.com/ru-ru/dotnet/core/install/linux-ubuntu-install?tabs=dotnet9&pivots=os-linux-ubuntu-2404
```
sudo add-apt-repository ppa:dotnet/backports
sudo apt-get update && \
  sudo apt-get install -y dotnet-sdk-9.0
sudo apt install zlib1g
```

#### Docker
install - https://docs.docker.com/engine/install/ubuntu/
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

fix error `Package docker-ce-cli is not available, but is referred to by another package.` (origin doc - https://forums.docker.com/t/installing-docker-on-buster-e-package-docker-ce-has-no-installation-candidate/108397/16)
```
curl -fsSL https://download.docker.com/linux/ubuntu/gpg | sudo gpg --dearmor -o  /usr/share/keyrings/docker-archive-keyring.gpg
echo "deb [arch=amd64 signed-by=/usr/share/keyrings/docker-archive-keyring.gpg] https://download.docker.com/linux/ubuntu  $(lsb_release -cs) stable" | sudo tee /etc/apt/sources.list.d/docker.list > /dev/null
sudo apt-get update
```

check
```
docker run hello-world
```

#### Lazydocker
install - https://lindevs.com/install-lazydocker-on-ubuntu
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
doc - https://github.com/chaifeng/ufw-docker?tab=readme-ov-file#install
```
sudo wget -O /usr/local/bin/ufw-docker \
  https://github.com/chaifeng/ufw-docker/raw/master/ufw-docker
sudo chmod +x /usr/local/bin/ufw-docker
ufw-docker install
```

check
```
ufw-docker check
```

#### Directories

```
mkdir /root/.vs-debugger /srv/db-backups /srv/git /srv/services /srv/services.stage /srv/Cloud.Disk /srv/tmp
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
```

#### Sources + Builds

```
cd /srv/git
rm -r *
git clone https://github.com/badhitman/BlankCRM.git
git clone https://github.com/badhitman/HtmlGenerator.git
cd /srv/git/BlankCRM/BlankBlazorApp/BlankBlazorApp
dotnet workload install wasm-tools
dotnet workload restore
dotnet tool install -g Microsoft.Web.LibraryManager.Cli
libman restore
cp /srv/git/BlankCRM/devops/etc/nginx/api.app /srv/git/BlankCRM/devops/etc/nginx/mongo.express /srv/git/BlankCRM/devops/etc/nginx/web.app /etc/nginx/sites-available
cp /srv/git/BlankCRM/devops/etc/nginx/stage/api.staging.app /srv/git/BlankCRM/devops/etc/nginx/stage/web.staging.app /etc/nginx/sites-available
ln -s /etc/nginx/sites-available/web.app /etc/nginx/sites-enabled/
ln -s /etc/nginx/sites-available/api.app /etc/nginx/sites-enabled/
ln -s /etc/nginx/sites-available/web.staging.app /etc/nginx/sites-enabled/
ln -s /etc/nginx/sites-available/api.staging.app /etc/nginx/sites-enabled/
ln -s /etc/nginx/sites-available/mongo.express /etc/nginx/sites-enabled/
systemctl reload nginx
```
```
dotnet publish -c Debug --output /srv/git/builds/ApiRestService /srv/git/BlankCRM/micro-services/ApiRestService/ApiRestService.csproj
dotnet publish -c Debug --output /srv/git/builds/StorageService /srv/git/BlankCRM/micro-services/StorageService/StorageService.csproj
dotnet publish -c Debug --output /srv/git/builds/CommerceService /srv/git/BlankCRM/micro-services/CommerceService/CommerceService.csproj
dotnet publish -c Debug --output /srv/git/builds/HelpDeskService /srv/git/BlankCRM/micro-services/HelpDeskService/HelpDeskService.csproj
dotnet publish -c Debug --output /srv/git/builds/ConstructorService /srv/git/BlankCRM/micro-services/ConstructorService/ConstructorService.csproj
dotnet publish -c Debug --output /srv/git/builds/TelegramBotService /srv/git/BlankCRM/micro-services/TelegramBotService/TelegramBotService.csproj
dotnet publish -c Debug --output /srv/git/builds/KladrService /srv/git/BlankCRM/micro-services/KladrService/KladrService.csproj
dotnet publish -c Debug --output /srv/git/builds/LdapService /srv/git/BlankCRM/micro-services/LdapService/LdapService.csproj
dotnet publish -c Debug --output /srv/git/builds/IdentityService /srv/git/BlankCRM/micro-services/IdentityService/IdentityService.csproj
dotnet publish -c Debug --output /srv/git/builds/BlankBlazorApp /srv/git/BlankCRM/BlankBlazorApp/BlankBlazorApp/BlankBlazorApp.csproj
```
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

```
cd /srv
docker compose up -d
```