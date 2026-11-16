export function returnArrayAsync() {
    DotNet.invokeMethodAsync('BlazorSample', 'ReturnArrayAsync')
        .then(data => {
            console.log(data);
        });
}

export function addHandlers() {
    const btn = document.getElementById("btn");
    btn.addEventListener("click", returnArrayAsync);
}

function startYaMap(incomingDotNetHelper) {
    var dotNetHelper = incomingDotNetHelper;
    // Дождёмся загрузки API и готовности DOM.
    ymaps.ready(init);

    function init() {
        var myPlacemark;
        // Создание карты.
        var myMap = new ymaps.Map("map", {
            // Координаты центра карты.
            // Порядок по умолчанию: «широта, долгота».
            // Чтобы не определять координаты центра карты вручную,
            // воспользуйтесь инструментом Определение координат.
            center: [55.76, 37.64],
            // Уровень масштабирования. Допустимые значения:
            // от 0 (весь мир) до 19.
            zoom: 7
        },
            {
                balloonMaxWidth: 200,
                searchControlProvider: "yandex#search",
            });

        // Слушаем клик на карте.
        myMap.events.add("click", function (e) {
            var coords = e.get("coords");

            // Если метка уже создана – просто передвигаем ее.
            if (myPlacemark) {
                myPlacemark.geometry.setCoordinates(coords);
            }
            // Если нет – создаем.
            else {
                myPlacemark = createPlacemark(coords);
                myMap.geoObjects.add(myPlacemark);
                // Слушаем событие окончания перетаскивания на метке.
                myPlacemark.events.add("dragend", function () {
                    getAddress(myPlacemark.geometry.getCoordinates());
                });
            }
            getAddress(coords);
        });

        // Создание метки.
        function createPlacemark(coords) {
            return new ymaps.Placemark(
                coords,
                {
                    iconCaption: "поиск...",
                },
                {
                    preset: "islands#violetDotIconWithCaption",
                    draggable: true,
                }
            );
        }

        // Определяем адрес по координатам (обратное геокодирование).
        function getAddress(coords) {
            myPlacemark.properties.set("iconCaption", "поиск...");
            ymaps.geocode(coords).then(function (res) {
                var firstGeoObject = res.geoObjects.get(0);

                myPlacemark.properties.set({
                    // Формируем строку с данными об объекте.
                    iconCaption: [
                        // Название населенного пункта или вышестоящее административно-территориальное образование.
                        firstGeoObject.getLocalities().length
                            ? firstGeoObject.getLocalities()
                            : firstGeoObject.getAdministrativeAreas(),
                        // Получаем путь до топонима, если метод вернул null, запрашиваем наименование здания.
                        firstGeoObject.getThoroughfare() ||
                        firstGeoObject.getPremise(),
                    ]
                        .filter(Boolean)
                        .join(", "),
                    // В качестве контента балуна задаем строку с адресом объекта.
                    balloonContent: firstGeoObject.getAddressLine(),
                });

                dotNetHelper.invokeMethodAsync('InitPlacemarkYaMapsAsync')
                    .then(data => {
                        console.log(data);
                    });
            });
        }
    }
}