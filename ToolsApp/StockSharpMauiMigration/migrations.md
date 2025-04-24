```
Add-Migration StockSharpAppContext002 -Context StockSharpAppContext -Project StockSharpMauiMigration -StartupProject StockSharpMauiMigration
Update-Database -Context StockSharpAppContext -Project StockSharpMauiMigration -StartupProject StockSharpMauiMigration
```