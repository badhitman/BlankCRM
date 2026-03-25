import { initializeApp } from "https://www.gstatic.com/firebasejs/12.11.0/firebase-app.js";
import { getAnalytics, logEvent } from "https://www.gstatic.com/firebasejs/12.11.0/firebase-analytics.js";
import { getMessaging, getToken } from "https://www.gstatic.com/firebasejs/12.11.0/firebase-messaging.js";

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

window.FirebaseMessagingToken = null;
window.RealtimeCoreComponent = null;
window.PublicMessagingToken = null;

const firebaseApp = initializeApp(firebaseConfig);
const firebaseMessaging = getMessaging(firebaseApp);
const firebaseAnalytics = getAnalytics(firebaseApp);

window.FirebaseSDK = {
    Initialize: function (publicMessagingToken) {
        window.PublicMessagingToken = publicMessagingToken;
        window.FirebaseSDK.RequestPermission();
    },
    RequestPermission: function () {
        console.log('Requesting permission...');
        Notification.requestPermission().then((permission) => {
            if (permission === 'granted') {
                console.info('Notification permission granted.');
                // const notification = new Notification("Приветсвую!");

                window.FirebaseMessagingToken = getToken(firebaseMessaging, { vapidKey: window.PublicMessagingToken }).then((currentToken) => {
                    logEvent(firebaseAnalytics, 'token_received');
                    if (currentToken) {
                        sendTokenToServer(currentToken);
                    } else {
                        console.warn('No registration token available. Request permission to generate one.');
                        setTokenSentToServer(false);
                    }
                }).catch((err) => {
                    console.warn('An error occurred while retrieving token. ', err);
                    logEvent(firebaseAnalytics, JSON.stringify(err));
                    setTokenSentToServer(false);
                });
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