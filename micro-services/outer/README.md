## Внешние источники данных (api/rest etc...)
каждый источник внешних данных - это отдельный микро-сервис со своей собственной базой данных.

 - https://breez.ru - api
 - https://daichi.ru - b2b
 - https://rusklimat.com - api
 - https://haierproff.ru - partners feeds (публичный)

#### Настройки
пример в [файле](outers-credentials-example.json).
haietprof - предоставляет публичный rss поток без наличия.
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
Загрузка некоторых источников данных может длиться длительное время. В случае если в данный момоент выполняется загрузка - система отображает журнал логов:
![dd](FeedsHaierProffRuService/img/download-progress.png)