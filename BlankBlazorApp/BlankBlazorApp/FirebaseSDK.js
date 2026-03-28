import { initializeApp } from "https://www.gstatic.com/firebasejs/12.11.0/firebase-app.js";
import { getAnalytics, logEvent } from "https://www.gstatic.com/firebasejs/12.11.0/firebase-analytics.js";
import { getMessaging, getToken } from "https://www.gstatic.com/firebasejs/12.11.0/firebase-messaging.js";

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

                let messagingTokenGet = getToken(firebaseMessaging, { vapidKey: window.PublicMessagingToken }).then((currentToken) => {
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
    ReadPermission: async function () {
        console.log('Read permission...');
        let res;
        await Notification.requestPermission().then((permission) => {
            res = permission;
        })
        .catch(function (err) {
            console.warn('Не удалось получить разрешение на показ уведомлений.', err);
            res = null;
        });
        return res;
    }
}

function sendTokenToServer(currentToken) {
    const dataToSend = {
        token: currentToken
    };
    $.ajax({
        url: '/firebase/FirebaseTokenHandle',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify(dataToSend),
        success: function (response) {
            console.log(`Токен [${currentToken}] отправлен на сервер: ${response}`);
        },
        error: function (xhr, status, error) {
            console.log(`Не удалось отправить токен [[${currentToken}]]: [status: ${status}] [error: ${error}].`);
        }
    });

    if (!isTokenSentToServer(currentToken)) {
        console.log('Отправка токена на сервер...');

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