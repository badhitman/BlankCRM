```
Add-Migration StockSharpAppContext002 -Context StockSharpAppContext -Project StockSharpDriver -StartupProject StockSharpDriver
Update-Database -Context StockSharpAppContext -Project StockSharpDriver -StartupProject StockSharpDriver
```