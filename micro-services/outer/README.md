## Внешние источники данных (api/rest etc...)
каждый источник внешних данных - это отдельный микро-сервис со своей собственной базой данных.

 - api.breez.ru - [breez api](https://github.com/badhitman/BlankCRM/tree/main/micro-services/outer/ApiBreezRuService)
 - daichi.ru - [b2b Daichi Business api](https://github.com/badhitman/BlankCRM/tree/main/micro-services/outer/ApiDaichiBusinessService)
 - rusklimat.com - [api Русклимат](https://github.com/badhitman/BlankCRM/tree/main/micro-services/outer/ApiRusklimatComService)
 - haierproff.ru - [partners feeds](https://github.com/badhitman/BlankCRM/tree/main/micro-services/outer/FeedsHaierProffRuService) (публичный)

#### Настройки
Пример в [файле](outers-credentials-example.json).

HaierProff - предоставляет публичный rss поток без наличия.
остальные источники требуют авторизации.
```json
"RusklimarApiConfig": {
    "UserId": "0001112233",
    "Password": "123123",
    "Version": "v1"
},
"BreezApiConfig": {
    "UserId": "",
    "Password": "",
    "Version": "v1"
  },
  "DaichiApiConfig": {
    "Token": "",
    "Version": "v1"
  },
```

#### Процесс обработки
Загрузка некоторых источников данных может длиться значительное время. В случае если в данный момент выполняется загрузка данных - система отображает журнал логов:
![download-progress](FeedsHaierProffRuService/img/download-progress.png)