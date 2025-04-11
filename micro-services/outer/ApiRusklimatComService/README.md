## b2b.rusklimat - REST API каталога ИП (Интернет Партнер)
Интеграция b2b.rusklimat.com
#### Авторизация
В автоматическом режиме через метод авторизации б2б портала
POST https://b2b.rusklimat.com/api/v1/auth/jwt/

### Получение данных каталога
Адрес сервиса: https://internet-partner.rusklimat.com

Онлайн документация: https://internet-partner.rusklimat.com/swagger/index.html

#### Получение идентификатора (`requestKey`)
GET /api/v1/InternetPartner/`XXX-XXX-XXX`/requestKey/
где `XXX-XXX-XXX` это *клиентский* идентификатор партнёра

#### Получение категорий товаров
GET /api/v1/InternetPartner/categories/{`requestKey`}

`requestKey` - необходимо получить в запросе, описанном выше ["Получение идентификатора"](#%D0%BF%D0%BE%D0%BB%D1%83%D1%87%D0%B5%D0%BD%D0%B8%D0%B5-%D0%B8%D0%B4%D0%B5%D0%BD%D1%82%D0%B8%D1%84%D0%B8%D0%BA%D0%B0%D1%82%D0%BE%D1%80%D0%B0)

#### Получение свойств товаров
GET /api/v1/InternetPartner/properties/{`requestKey`}

`requestKey` - необходимо получить в запросе, описанном выше ["Получение идентификатора"](#%D0%BF%D0%BE%D0%BB%D1%83%D1%87%D0%B5%D0%BD%D0%B8%D0%B5-%D0%B8%D0%B4%D0%B5%D0%BD%D1%82%D0%B8%D1%84%D0%B8%D0%BA%D0%B0%D1%82%D0%BE%D1%80%D0%B0)

#### Получение товаров каталога
POST /api/v1/InternetPartner/`XXX-XXX-XXX`/products/{`requestKey`}/?pageSize=500&page=2

где `XXX-XXX-XXX` это *клиентский* идентификатор партнёра

`requestKey` - необходимо получить в запросе, описанном выше ["Получение идентификатора"](#%D0%BF%D0%BE%D0%BB%D1%83%D1%87%D0%B5%D0%BD%D0%B8%D0%B5-%D0%B8%D0%B4%D0%B5%D0%BD%D1%82%D0%B8%D1%84%D0%B8%D0%BA%D0%B0%D1%82%D0%BE%D1%80%D0%B0)

#### Получение единиц измерения
GET /api/v1/InternetPartner/units