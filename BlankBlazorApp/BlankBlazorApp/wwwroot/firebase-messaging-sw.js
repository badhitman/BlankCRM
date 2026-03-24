importScripts("https://www.gstatic.com/firebasejs/12.11.0/firebase-app-compat.js");
importScripts("https://www.gstatic.com/firebasejs/12.11.0/firebase-messaging-compat.js");
importScripts("https://www.gstatic.com/firebasejs/12.11.0/firebase-analytics-compat.js");

// Initialize Firebase using the global firebase object exposed by compat libraries
const firebaseConfig = {
    apiKey: "AIzaSyCPlUkq609DA2CpFZsP88v-FIfFBU6uGRI",
    authDomain: "evident-ethos-230204.firebaseapp.com",
    databaseURL: "https://evident-ethos-230204.firebaseio.com",
    projectId: "evident-ethos-230204",
    storageBucket: "evident-ethos-230204.firebasestorage.app",
    messagingSenderId: "1064563856635",
    appId: "1:1064563856635:web:5267f1a99da99ef9710c5e",
    measurementId: "G-HVJ38TKTDN"
};

window.FirebaseMessagingToken = null;
window.RealtimeCoreComponent = null;
window.PublicMessagingToken = null;

const firebaseApp = firebase.initializeApp(firebaseConfig);
const firebaseMessaging = firebase.messaging();
const firebaseAnalytics = firebase.analytics();

firebaseMessaging.onBackgroundMessage(function (payload) {
    console.log("[firebase-messaging-sw.js] Received background message ", payload);
    // ... customize your notification
});

firebaseMessaging.onMessage(function (payload) {
    console.log('Message received. ', payload);
    new Notification(payload.notification.title, payload.notification);
});

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