importScripts("https://www.gstatic.com/firebasejs/12.11.0/firebase-app-compat.js");
importScripts("https://www.gstatic.com/firebasejs/12.11.0/firebase-messaging-compat.js");
importScripts("https://www.gstatic.com/firebasejs/12.11.0/firebase-analytics-compat.js");

// Initialize Firebase using the global firebase object exposed by compat libraries
window.firebaseConfig = {
    apiKey: "AIzaSyCPlUkq609DA2CpFZsP88v-FIfFBU6uGRI",
    authDomain: "evident-ethos-230204.firebaseapp.com",
    databaseURL: "https://evident-ethos-230204.firebaseio.com",
    projectId: "evident-ethos-230204",
    storageBucket: "evident-ethos-230204.firebasestorage.app",
    messagingSenderId: "1064563856635",
    appId: "1:1064563856635:web:5267f1a99da99ef9710c5e",
    measurementId: "G-HVJ38TKTDN"
};

const firebaseApp = firebase.initializeApp(window.firebaseConfig);
const firebaseMessaging = firebase.messaging();

firebaseMessaging.onBackgroundMessage(function (payload) {
    console.log("[firebase-messaging-sw.js] Received background message ", payload);
    // ... customize your notification
});

firebaseMessaging.onMessage(function (payload) {
    console.log('Message received. ', payload);
    new Notification(payload.notification.title, payload.notification);
});

