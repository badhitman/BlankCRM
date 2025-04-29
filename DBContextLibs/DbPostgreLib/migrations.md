```
Add-Migration MainPostgreContext002 -Context MainAppContext -Project DbPostgreLib -StartupProject BlankBlazorApp
Update-Database -Context MainAppContext -Project DbPostgreLib -StartupProject BlankBlazorApp
```

```
Add-Migration CommerceContext002 -Context CommerceContext -Project DbPostgreLib -StartupProject CommerceService
Update-Database -Context CommerceContext -Project DbPostgreLib -StartupProject CommerceService
```

```
Add-Migration ConstructorContext002 -Context ConstructorContext -Project DbPostgreLib -StartupProject ConstructorService
Update-Database -Context ConstructorContext -Project DbPostgreLib -StartupProject ConstructorService
```

```
Add-Migration HelpDeskPostgreContext002 -Context HelpDeskContext -Project DbPostgreLib -StartupProject HelpDeskService
Update-Database -Context HelpDeskContext -Project DbPostgreLib -StartupProject HelpDeskService
```

```
Add-Migration KladrContext002 -Context KladrContext -Project DbPostgreLib -StartupProject KladrService
Update-Database -Context KladrContext -Project DbPostgreLib -StartupProject KladrService
```

```
Add-Migration StorageContext002 -Context StorageContext -Project DbPostgreLib -StartupProject StorageService
Update-Database -Context StorageContext -Project DbPostgreLib -StartupProject StorageService
```

```
Add-Migration NLogsContext002 -Context NLogsContext -Project DbPostgreLib -StartupProject StorageService
Update-Database -Context NLogsContext -Project DbPostgreLib -StartupProject StorageService
```

```
Add-Migration TelegramBotContext005 -Context TelegramBotContext -Project DbPostgreLib -StartupProject TelegramBotService
Update-Database -Context TelegramBotContext -Project DbPostgreLib -StartupProject TelegramBotService
```


```
Add-Migration ApiBreezRuContext002 -Context ApiBreezRuContext -Project DbPostgreLib -StartupProject ApiBreezRuService
Update-Database -Context ApiBreezRuContext -Project DbPostgreLib -StartupProject ApiBreezRuService
```
```
Add-Migration ApiDaichiBusinessContext002 -Context ApiDaichiBusinessContext -Project DbPostgreLib -StartupProject ApiDaichiBusinessService
Update-Database -Context ApiDaichiBusinessContext -Project DbPostgreLib -StartupProject ApiDaichiBusinessService
```
```
Add-Migration ApiRusklimatComContext002 -Context ApiRusklimatComContext -Project DbPostgreLib -StartupProject ApiRusklimatComService
Update-Database -Context ApiRusklimatComContext -Project DbPostgreLib -StartupProject ApiRusklimatComService
```
```
Add-Migration FeedsHaierProffRuContext002 -Context FeedsHaierProffRuContext -Project DbPostgreLib -StartupProject FeedsHaierProffRuService
Update-Database -Context FeedsHaierProffRuContext -Project DbPostgreLib -StartupProject FeedsHaierProffRuService
```