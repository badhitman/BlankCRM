// Import the functions you need from the SDKs you need
import { initializeApp } from "https://www.gstatic.com/firebasejs/12.11.0/firebase-app.js";
import { getAnalytics, logEvent } from "https://www.gstatic.com/firebasejs/12.11.0/firebase-analytics.js";
import { getMessaging, getToken } from "https://www.gstatic.com/firebasejs/12.11.0/firebase-messaging.js";

// TODO: Add SDKs for Firebase products that you want to use
// https://firebase.google.com/docs/web/setup#available-libraries

window.FirebaseConfig = null;
window.FirebaseApp = null;
window.FirebaseAnalytics = null;
window.FirebaseMessaging = null;
window.FirebaseMessagingToken = null;
window.RealtimeCoreComponent = null;

window.FirebaseSDK = {
    Initialize: function (apiKey, authDomain, databaseURL, projectId, storageBucket, messagingSenderId, appId, measurementId, publicMessagingToken) {
        window.FirebaseConfig = {
            apiKey: apiKey,
            authDomain: authDomain,
            databaseURL: databaseURL,
            projectId: projectId,
            storageBucket: storageBucket,
            messagingSenderId: messagingSenderId,
            appId: appId,
            measurementId: measurementId
        };

        window.FirebaseApp = initializeApp(window.FirebaseConfig);
        window.FirebaseAnalytics = getAnalytics(window.FirebaseApp);
        window.FirebaseMessaging = getMessaging(window.FirebaseApp);
        //window.FirebaseMessaging.onMessage(function (payload) {
        //    console.log('Message received. ', payload);
        //    new Notification(payload.notification.title, payload.notification);
        //});

        //if ("serviceWorker" in navigator) {
        //    navigator.serviceWorker.register("./firebase-messaging-sw.js").then((registration) => {
        //        getToken(messaging, { serviceWorkerRegistration: registration, vapidKey: publicMessagingToken });
        //        // The scope is now determined by the registration
        //    });
        //}

        window.FirebaseMessagingToken = getToken(window.FirebaseMessaging, { vapidKey: publicMessagingToken }).then((currentToken) => {
            if (currentToken) {
                // Send the token to your server and update the UI if necessary
                sendTokenToServer(currentToken);
            } else {
                // Show permission request UI
                console.log('No registration token available. Request permission to generate one.');
                setTokenSentToServer(false);
            }
        }).catch((err) => {
            console.log('An error occurred while retrieving token. ', err);
            logEvent(window.FirebaseAnalytics, JSON.stringify(err));
            window.effects.Toast("Новое сообщение в чате", JSON.stringify(err), "info", true, "#9EC600");
            setTokenSentToServer(false);
        });
        window.FirebaseSDK.RequestPermission();
    },
    RequestPermission: function () {
        console.log('Requesting permission...');
        Notification.requestPermission().then((permission) => {
            if (permission === 'granted') {
                console.log('Notification permission granted.');
                const notification = new Notification("Hi there!");
            }
        });
    },
    RealtimeRegister: function (dotNetReference) {
        window.RealtimeCoreComponent = dotNetReference;
    }
}

/*if ('Notification' in window) {
    var messaging = firebase.messaging();

    // пользователь уже разрешил получение уведомлений
    // подписываем на уведомления если ещё не подписали
    if (Notification.permission === 'granted') {
        const notification = new Notification("Hi there!");
        subscribe();
    }

    // по клику, запрашиваем у пользователя разрешение на уведомления
    // и подписываем его
    $('#subscribe').on('click', function () {
        subscribe();
    });
}*/

/*function subscribe() {
    // запрашиваем разрешение на получение уведомлений
    messaging.requestPermission()
        .then(function () {
            // получаем ID устройства
            messaging.getToken()
                .then(function (currentToken) {
                    console.log(currentToken);

                    if (currentToken) {
                        sendTokenToServer(currentToken);
                    } else {
                        console.warn('Не удалось получить токен.');
                        setTokenSentToServer(false);
                    }
                })
                .catch(function (err) {
                    console.warn('При получении токена произошла ошибка.', err);
                    setTokenSentToServer(false);
                });
        })
        .catch(function (err) {
            console.warn('Не удалось получить разрешение на показ уведомлений.', err);
        });
}*/

function sendTokenToServer(currentToken) {
    if (!isTokenSentToServer(currentToken)) {
        console.log('Отправка токена на сервер...');
        if (window.RealtimeCoreComponent)
            window.RealtimeCoreComponent.invokeMethodAsync('FirebaseTokenSave', currentToken);

        setTokenSentToServer(currentToken);
    } else {
        console.log('Токен уже отправлен на сервер.');
    }
}

function isTokenSentToServer(currentToken) {
    return window.localStorage.getItem('sentFirebaseMessagingToken') == currentToken;
}

function setTokenSentToServer(currentToken) {
    window.localStorage.setItem('sentFirebaseMessagingToken', currentToken ? currentToken : '');
}

self.addEventListener('notificationclick', function (event) {
    const target = event.notification.data.click_action || '/';
    event.notification.close();

    event.waitUntil(clients.matchAll({
        type: 'window',
        includeUncontrolled: true
    }).then(function (clientList) {
        for (var i = 0; i < clientList.length; i++) {
            var client = clientList[i];
            if (client.url == target && 'focus' in client) {
                return client.focus();
            }
        }

        return clients.openWindow(target);
    }));
});