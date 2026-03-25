importScripts("https://www.gstatic.com/firebasejs/12.11.0/firebase-app-compat.js");
importScripts("https://www.gstatic.com/firebasejs/12.11.0/firebase-messaging-compat.js");
importScripts("https://www.gstatic.com/firebasejs/12.11.0/firebase-analytics-compat.js");
importScripts("./js/FirebaseConfigSDK.js");

const firebaseApp = firebase.initializeApp(firebaseConfig);
const firebaseMessaging = firebase.messaging();

firebaseMessaging.onBackgroundMessage(function (payload) {
    console.log("[firebase-messaging-sw.js] Received background message ", payload);
    // ... customize your notification
});

firebaseMessaging.onMessage(function (payload) {
    console.log('Message received. ', payload);
    new Notification(payload.notification.title, payload.notification);
});

