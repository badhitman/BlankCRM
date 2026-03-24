// Import the functions you need from the SDKs you need
import { initializeApp } from "https://www.gstatic.com/firebasejs/12.11.0/firebase-app.js";
import { getAnalytics, logEvent } from "https://www.gstatic.com/firebasejs/12.11.0/firebase-analytics.js";
import { getMessaging, getToken } from "https://www.gstatic.com/firebasejs/12.11.0/firebase-messaging.js";

// TODO: Add SDKs for Firebase products that you want to use
// https://firebase.google.com/docs/web/setup#available-libraries

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
const firebaseApp = initializeApp(firebaseConfig);
const firebaseMessaging = getMessaging(firebaseApp);
const firebaseAnalytics = getAnalytics(firebaseApp);

window.FirebaseMessagingToken = null;
window.RealtimeCoreComponent = null;
window.PublicMessagingToken = null;

window.FirebaseSDK = {
    Initialize: function (publicMessagingToken) {
        window.PublicMessagingToken = publicMessagingToken;
        //firebaseMessaging.onMessage(function (payload) {
        //    console.log('Message received. ', payload);
        //    new Notification(payload.notification.title, payload.notification);
        //});

        //if ("serviceWorker" in navigator) {
        //    navigator.serviceWorker.register("./firebase-messaging-sw.js").then((registration) => {
        //        getToken(messaging, { serviceWorkerRegistration: registration, vapidKey: window.PublicMessagingToken });
        //        // The scope is now determined by the registration
        //    });
        //}
                
        window.FirebaseSDK.RequestPermission();
    },
    RequestPermission: function () {
        console.log('Requesting permission...');
        Notification.requestPermission().then((permission) => {
            if (permission === 'granted') {
                console.info('Notification permission granted.');
                // const notification = new Notification("Приветсвую!");

                //window.FirebaseMessagingToken = getToken(firebaseMessaging, { vapidKey: window.PublicMessagingToken }).then((currentToken) => {
                //    if (currentToken) {
                //        sendTokenToServer(currentToken);
                //    } else {
                //        console.warn('No registration token available. Request permission to generate one.');
                //        setTokenSentToServer(false);
                //    }
                //}).catch((err) => {
                //    console.warn('An error occurred while retrieving token. ', err);
                //    logEvent(firebaseAnalytics, JSON.stringify(err));
                //    setTokenSentToServer(false);
                //});
            }
        })
        .catch(function (err) {
            console.warn('Не удалось получить разрешение на показ уведомлений.', err);
        });
    },
    RealtimeRegister: function (dotNetReference) {
        window.RealtimeCoreComponent = dotNetReference;
    }
}

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