```
Add-Migration StockSharpAppContext002 -Context StockSharpAppContext -Project StockSharpDriver -StartupProject StockSharpDriver
Update-Database -Context StockSharpAppContext -Project StockSharpDriver -StartupProject StockSharpDriver
```

```
Add-Migration NLogsContext002 -Context NLogsContext -Project StockSharpDriver -StartupProject StockSharpDriver
Update-Database -Context NLogsContext -Project StockSharpDriver -StartupProject StockSharpDriver
```