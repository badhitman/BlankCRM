```
Add-Migration MainMySQLContext001 -Context MainAppContext -Project DbMySQLLib -StartupProject BlankBlazorApp
Update-Database -Context MainAppContext -Project DbMySQLLib -StartupProject BlankBlazorApp
```

```
Add-Migration StorageContext001 -Context StorageContext -Project DbMySQLLib -StartupProject StorageService
Update-Database -Context StorageContext -Project DbMySQLLib -StartupProject StorageService
```

```
Add-Migration NLogsContext001 -Context NLogsContext -Project DbMySQLLib -StartupProject StorageService
Update-Database -Context NLogsContext -Project DbMySQLLib -StartupProject StorageService
```

```
Add-Migration HelpDeskMySQLContext001 -Context HelpDeskContext -Project DbMySQLLib -StartupProject HelpDeskService
Update-Database -Context HelpDeskContext -Project DbMySQLLib -StartupProject HelpDeskService
```

```
Add-Migration TelegramBotContext001 -Context TelegramBotContext -Project DbMySQLLib -StartupProject TelegramBotService
Update-Database -Context TelegramBotContext -Project DbMySQLLib -StartupProject TelegramBotService
```

```
Add-Migration CommerceContext001 -Context CommerceContext -Project DbMySQLLib -StartupProject CommerceService
Update-Database -Context CommerceContext -Project DbMySQLLib -StartupProject CommerceService
```