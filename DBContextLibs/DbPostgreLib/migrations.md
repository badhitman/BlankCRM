```
Add-Migration MainAppContext001 -Context MainAppContext -Project DbPostgreLib -StartupProject BlankBlazorApp
Update-Database -Context MainAppContext -Project DbPostgreLib -StartupProject BlankBlazorApp
```

```
Add-Migration RealtimeContext001 -Context RealtimeContext -Project DbPostgreLib -StartupProject RealtimeService
Update-Database -Context RealtimeContext -Project DbPostgreLib -StartupProject RealtimeService
```

```
Add-Migration CommerceContext001 -Context CommerceContext -Project DbPostgreLib -StartupProject CommerceService
Update-Database -Context CommerceContext -Project DbPostgreLib -StartupProject CommerceService
```

```
Add-Migration BankContext001 -Context BankContext -Project DbPostgreLib -StartupProject BankService
Update-Database -Context BankContext -Project DbPostgreLib -StartupProject BankService
```

```
Add-Migration ConstructorContext001 -Context ConstructorContext -Project DbPostgreLib -StartupProject ConstructorService
Update-Database -Context ConstructorContext -Project DbPostgreLib -StartupProject ConstructorService
```

```
Add-Migration HelpDeskPostgreContext001 -Context HelpDeskContext -Project DbPostgreLib -StartupProject HelpDeskService
Update-Database -Context HelpDeskContext -Project DbPostgreLib -StartupProject HelpDeskService
```

```
Add-Migration KladrContext001 -Context KladrContext -Project DbPostgreLib -StartupProject KladrService
Update-Database -Context KladrContext -Project DbPostgreLib -StartupProject KladrService
```

```
Add-Migration StorageContext001 -Context StorageContext -Project DbPostgreLib -StartupProject StorageService
Update-Database -Context StorageContext -Project DbPostgreLib -StartupProject StorageService
```

```
Add-Migration IndexingServiceContext001 -Context IndexingServiceContext -Project DbPostgreLib -StartupProject IndexingService
Update-Database -Context IndexingServiceContext -Project DbPostgreLib -StartupProject IndexingService
```

```
Add-Migration TelegramBotContext001 -Context TelegramBotContext -Project DbPostgreLib -StartupProject TelegramBotService
Update-Database -Context TelegramBotContext -Project DbPostgreLib -StartupProject TelegramBotService
```


```
Add-Migration NLogsContext001 -Context NLogsContext -Project DbPostgreLib -StartupProject StorageService
Update-Database -Context NLogsContext -Project DbPostgreLib -StartupProject StorageService
```



Исключены из решения
```
Add-Migration ApiBreezRuContext001 -Context ApiBreezRuContext -OutputDir Migrations/ApiBreezRu -Project DbPostgreLib -StartupProject ApiBreezRuService
Update-Database -Context ApiBreezRuContext -Project DbPostgreLib -StartupProject ApiBreezRuService
```
```
Add-Migration ApiDaichiBusinessContext001 -Context ApiDaichiBusinessContext -OutputDir Migrations/ApiDaichiBusiness -Project DbPostgreLib -StartupProject ApiDaichiBusinessService
Update-Database -Context ApiDaichiBusinessContext -Project DbPostgreLib -StartupProject ApiDaichiBusinessService
```
```
Add-Migration ApiRusklimatComContext001 -Context ApiRusklimatComContext -OutputDir Migrations/ApiRusklimatCom -Project DbPostgreLib -StartupProject ApiRusklimatComService
Update-Database -Context ApiRusklimatComContext -Project DbPostgreLib -StartupProject ApiRusklimatComService
```
```
Add-Migration FeedsHaierProffRuContext001 -Context FeedsHaierProffRuContext -OutputDir Migrations/FeedsHaierProffRu -Project DbPostgreLib -StartupProject FeedsHaierProffRuService
Update-Database -Context FeedsHaierProffRuContext -Project DbPostgreLib -StartupProject FeedsHaierProffRuService
```