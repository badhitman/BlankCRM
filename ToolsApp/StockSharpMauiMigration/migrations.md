```
Add-Migration StockSharpAppContext002 -Context StockSharpAppContext -Project StockSharpMauiMigration -StartupProject StockSharpMauiMigration
```

вручную миграции не накатывать (`Update-Database -Context StockSharpAppContext -Project StockSharpMauiMigration -StartupProject StockSharpMauiMigration`)
они создают проблемный файл -> ef core: c:\Users\User\AppData\Roaming\StockSharpAppContext\ef.db3