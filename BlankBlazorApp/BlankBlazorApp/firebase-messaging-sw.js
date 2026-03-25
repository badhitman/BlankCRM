importScripts("https://www.gstatic.com/firebasejs/12.11.0/firebase-app-compat.js");
importScripts("https://www.gstatic.com/firebasejs/12.11.0/firebase-messaging-compat.js");
importScripts("https://www.gstatic.com/firebasejs/12.11.0/firebase-analytics-compat.js");

// Initialize Firebase using the global firebase object exposed by compat libraries
const firebaseConfig = {
    apiKey: "**apiKey**",
    authDomain: "**authDomain**",
    databaseURL: "**databaseURL**",
    projectId: "**projectId**",
    storageBucket: "**storageBucket**",
    messagingSenderId: "**messagingSenderId**",
    appId: "**appId**",
    measurementId: "**measurementId**"
};

const firebaseApp = firebase.initializeApp(firebaseConfig);
const firebaseMessaging = firebase.messaging();

firebaseMessaging.onBackgroundMessage(function (payload) {
    console.log("[firebase-messaging-sw.js] Received background message ", payload);
    if (window.RealtimeCoreComponent) {
        window.effects.Toast("Новое сообщение Firebase", payload, "info", true, "#9EC600");
    }

    new Notification(payload.notification.title, payload.notification);
});

firebaseMessaging.onMessage(function (payload) {
    console.log('Message received. ', payload);
    if (window.RealtimeCoreComponent) {
        window.effects.Toast("Новое сообщение Firebase", payload, "info", true, "#9EC600");
    }

    navigator.serviceWorker.register('messaging-sw.js');
    navigator.serviceWorker.ready.then(function (registration) {
        payload.notification.data = payload.notification;
        return registration.showNotification(payload.notification.title, payload.notification);
    }).catch(function (error) {
        console.log('ServiceWorker registration failed', error);
    });
});

//firebase.setBackgroundMessageHandler(function (payload) {
//    if (typeof payload.data.time != 'undefined') {
//        var time = new Date(payload.data.time * 1000);
//        var now = new Date();

//        if (time < now) {
//            return null;
//        }

//        var diff = Math.round((time.getTime() - now.getTime()) / 1000);

//        payload.data.body = 'Начало через ' +
//            Math.round(diff / 60) + ' минут, в ' + time.getHours() + ':' +
//            (time.getMinutes() > 9 ? time.getMinutes() : '0' + time.getMinutes())
//            ;
//    }

//    payload.data.data = payload.data;

//    return self.registration.showNotification(payload.data.title, payload.data);
//});

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