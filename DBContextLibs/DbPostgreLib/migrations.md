```
Add-Migration MainPostgreContext001 -Context MainAppContext -Project DbPostgreLib -StartupProject BlankBlazorApp
Update-Database -Context MainAppContext -Project DbPostgreLib -StartupProject BlankBlazorApp
```

```
Add-Migration CommerceContext001 -Context CommerceContext -Project DbPostgreLib -StartupProject CommerceService
Update-Database -Context CommerceContext -Project DbPostgreLib -StartupProject CommerceService
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
Add-Migration TelegramBotContext001 -Context TelegramBotContext -Project DbPostgreLib -StartupProject TelegramBotService
Update-Database -Context TelegramBotContext -Project DbPostgreLib -StartupProject TelegramBotService
```


```
Add-Migration NLogsContext001 -Context NLogsContext -Project DbPostgreLib -StartupProject StorageService
Update-Database -Context NLogsContext -Project DbPostgreLib -StartupProject StorageService
```



Исключены из решения
```
Add-Migration ApiBreezRuContext001 -Context ApiBreezRuContext -Project DbPostgreLib -StartupProject ApiBreezRuService
Update-Database -Context ApiBreezRuContext -Project DbPostgreLib -StartupProject ApiBreezRuService
```
```
Add-Migration ApiDaichiBusinessContext001 -Context ApiDaichiBusinessContext -Project DbPostgreLib -StartupProject ApiDaichiBusinessService
Update-Database -Context ApiDaichiBusinessContext -Project DbPostgreLib -StartupProject ApiDaichiBusinessService
```
```
Add-Migration ApiRusklimatComContext001 -Context ApiRusklimatComContext -Project DbPostgreLib -StartupProject ApiRusklimatComService
Update-Database -Context ApiRusklimatComContext -Project DbPostgreLib -StartupProject ApiRusklimatComService
```
```
Add-Migration FeedsHaierProffRuContext001 -Context FeedsHaierProffRuContext -Project DbPostgreLib -StartupProject FeedsHaierProffRuService
Update-Database -Context FeedsHaierProffRuContext -Project DbPostgreLib -StartupProject FeedsHaierProffRuService
```