cd /srv/git
rm -r /srv/git/*
git clone https://github.com/badhitman/BlankCRM.git
git clone https://github.com/badhitman/HtmlGenerator.git
git clone https://github.com/badhitman/TinkofClientApi.git
git clone https://github.com/badhitman/DataFileScanerLib.git
cd /srv/git/BlankCRM/BlankBlazorApp/BlankBlazorApp
dotnet workload install wasm-tools
dotnet workload restore
dotnet tool install -g Microsoft.Web.LibraryManager.Cli
libman restore
dotnet publish -c Debug --output /srv/git/builds/ApiRestService /srv/git/BlankCRM/micro-services/ApiRestService/ApiRestService.csproj
dotnet publish -c Debug --output /srv/git/builds/StorageService /srv/git/BlankCRM/micro-services/StorageService/StorageService.csproj
dotnet publish -c Debug --output /srv/git/builds/IndexingService /srv/git/BlankCRM/micro-services/IndexingService/IndexingService.csproj
dotnet publish -c Debug --output /srv/git/builds/RealtimeService /srv/git/BlankCRM/micro-services/RealtimeService/RealtimeService.csproj
dotnet publish -c Debug --output /srv/git/builds/CommerceService /srv/git/BlankCRM/micro-services/CommerceService/CommerceService.csproj
dotnet publish -c Debug --output /srv/git/builds/BankService /srv/git/BlankCRM/micro-services/BankService/BankService.csproj
dotnet publish -c Debug --output /srv/git/builds/HelpDeskService /srv/git/BlankCRM/micro-services/HelpDeskService/HelpDeskService.csproj
dotnet publish -c Debug --output /srv/git/builds/ConstructorService /srv/git/BlankCRM/micro-services/ConstructorService/ConstructorService.csproj
dotnet publish -c Debug --output /srv/git/builds/TelegramBotService /srv/git/BlankCRM/micro-services/TelegramBotService/TelegramBotService.csproj
dotnet publish -c Debug --output /srv/git/builds/KladrService /srv/git/BlankCRM/micro-services/KladrService/KladrService.csproj
dotnet publish -c Debug --output /srv/git/builds/LdapService /srv/git/BlankCRM/micro-services/LdapService/LdapService.csproj
dotnet publish -c Debug --output /srv/git/builds/IdentityService /srv/git/BlankCRM/micro-services/IdentityService/IdentityService.csproj
dotnet publish -c Debug --output /srv/git/builds/BlankBlazorApp /srv/git/BlankCRM/BlankBlazorApp/BlankBlazorApp/BlankBlazorApp.csproj