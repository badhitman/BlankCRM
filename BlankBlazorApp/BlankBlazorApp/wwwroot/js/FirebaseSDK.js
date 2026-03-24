import { initializeApp } from "https://www.gstatic.com/firebasejs/12.11.0/firebase-app.js";
import { getAnalytics, logEvent } from "https://www.gstatic.com/firebasejs/12.11.0/firebase-analytics.js";
import { getMessaging, getToken } from "https://www.gstatic.com/firebasejs/12.11.0/firebase-messaging.js";

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

async function enableNotifications() {
    // Insert your firebase project config here
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

    const app = initializeApp(firebaseConfig);
    const messaging = getMessaging(app);

    const permission = await Notification.requestPermission();
    if (permission !== 'granted') {
        console.log("user denied notifications")
    }

    const token = await getToken(messaging, { vapidKey: "BHNHODqpqbAdxcZYiEV9Suelf4DsT0mn1MT41P1YkUkCjNNLExbgzGvazLAdweupi3xhOYDwVEzA4gT6G7VCgAU" });

    window.document.getElementById("pushTokenLayer").removeAttribute("hidden");

    const pushTokenValue = window.document.getElementById("pushTokenValue");
    pushTokenValue.innerText = token
}

// Wait for the DOM to be fully loaded before attaching listeners
document.addEventListener('DOMContentLoaded', () => {
    const button = document.getElementById('enableNotificationsBtn');
    if (button) {
        button.addEventListener('click', enableNotifications);
    }
});