## Интеграция API https://api.breez.ru

- [Контентный API](https://api.breez.ru/api): [brands](https://api.breez.ru/api#brands), [categories](https://api.breez.ru/api#categories), [products](https://api.breez.ru/api#products) и [tech`s](https://api.breez.ru/api#tech)
- [Leftovers](https://api.breez.ru/lo): [все остатки](https://api.breez.ru/lo#full) на доступных клиенту складах.

#### Конечная точка
Взаимодействие с API breez.ru. Базовый энд-поинт API, относительно которого строятся запросы: https://api.breez.ru/v1/. Используемый HTTP-метод: `GET`. Тип запроса указывается в качестве части пути. Параметры запроса передаются в параметрах URL.


#### Авторизация
Авторизация происходит посредством **Базовой** схемы HTTP-аутентификации. Учётные данные для доступа к API передаются с каждым запросом в заголовке `Authorization`. Заголовок имеет следующий формат:
```
Authorization: Basic <credentials>
```