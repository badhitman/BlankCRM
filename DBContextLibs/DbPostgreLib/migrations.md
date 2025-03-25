```
Add-Migration MainPostgreContext002 -Context MainDbAppContext -Project DbPostgreLib -StartupProject BlankBlazorApp
Update-Database -Context MainDbAppContext -Project DbPostgreLib -StartupProject BlankBlazorApp
```

```
Add-Migration StorageContext002 -Context StorageContext -Project DbPostgreLib -StartupProject StorageService
Update-Database -Context StorageContext -Project DbPostgreLib -StartupProject StorageService
```

```
Add-Migration KladrContext002 -Context KladrContext -Project DbPostgreLib -StartupProject KladrService
Update-Database -Context KladrContext -Project DbPostgreLib -StartupProject KladrService
```

```
Add-Migration NLogsContext002 -Context NLogsContext -Project DbPostgreLib -StartupProject StorageService
Update-Database -Context NLogsContext -Project DbPostgreLib -StartupProject StorageService
```

```
Add-Migration HelpdeskPostgreContext002 -Context HelpdeskContext -Project DbPostgreLib -StartupProject HelpdeskService
Update-Database -Context HelpdeskContext -Project DbPostgreLib -StartupProject HelpdeskService
```

```
Add-Migration TelegramBotContext005 -Context TelegramBotContext -Project DbPostgreLib -StartupProject TelegramBotService
Update-Database -Context TelegramBotContext -Project DbPostgreLib -StartupProject TelegramBotService
```

```
Add-Migration CommerceContext003 -Context CommerceContext -Project DbPostgreLib -StartupProject CommerceService
Update-Database -Context CommerceContext -Project DbPostgreLib -StartupProject CommerceService
```

```
Add-Migration ConstructorContext004 -Context ConstructorContext -Project DbPostgreLib -StartupProject ConstructorService
Update-Database -Context ConstructorContext -Project DbPostgreLib -StartupProject ConstructorService
```