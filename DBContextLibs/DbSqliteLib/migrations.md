```
Add-Migration MainContext001 -Context MainAppContext -Project DbSqliteLib -StartupProject BlankBlazorApp
Update-Database -Context MainAppContext -Project DbSqliteLib -StartupProject BlankBlazorApp
```

```
Add-Migration HelpDeskContext001 -Context HelpDeskContext -Project DbSqliteLib -StartupProject HelpDeskService
Update-Database -Context HelpDeskContext -Project DbSqliteLib -StartupProject HelpDeskService
```

```
Add-Migration TelegramBotContext001 -Context TelegramBotContext -Project DbSqliteLib -StartupProject TelegramBotService
Update-Database -Context TelegramBotContext -Project DbSqliteLib -StartupProject TelegramBotService
```

```
Add-Migration CommerceContext001 -Context CommerceContext -Project DbSqliteLib -StartupProject CommerceService
Update-Database -Context CommerceContext -Project DbSqliteLib -StartupProject CommerceService
```

```
Add-Migration StorageContext001 -Context StorageContext -Project DbSqliteLib -StartupProject StorageService
Update-Database -Context StorageContext -Project DbSqliteLib -StartupProject StorageService
```

```
Add-Migration NLogsContext001 -Context NLogsContext -Project DbSqliteLib -StartupProject StorageService
Update-Database -Context NLogsContext -Project DbSqliteLib -StartupProject StorageService
```