## b2b.rusklimat - REST API каталога ИП (Интернет Партнер)
Интеграция b2b.rusklimat.com
#### Авторизация
В автоматическом режиме через метод авторизации б2б портала
POST https://b2b.rusklimat.com/api/v1/auth/jwt/

### Получение данных каталога
Адрес сервиса: https://internet-partner.rusklimat.com
Онлайн документация: https://internet-partner.rusklimat.com/swagger/index.html

#### Получение идентификатора
GET /api/v1/InternetPartner/XXX-XXX-XXX/requestKey/
где `XXX-XXX-XXX` это ваш идентификатор партнёра

#### Получение категорий товаров
GET /api/v1/InternetPartner/categories/{requestKey}
`requestKey` - необходимо получить в запросе, описанном выше ["Получение идентификатора"]

#### Получение свойств товаров
GET /api/v1/InternetPartner/properties/{requestKey}
`requestKey` - необходимо получить в запросе, описанном выше ["Получение идентификатора"]

#### Получение товаров каталога
POST /api/v1/InternetPartner/XXX-XXX-XXX/products/{requestKey}/?pageSize=500&page=2
где `XXX-XXX-XXX` это ваш идентификатор партнёра
`requestKey` - необходимо получить в запросе, описанном выше ["Получение идентификатора"]

#### Получение единиц измерения
GET /api/v1/InternetPartner/units