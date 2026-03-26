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
    let req = {
        message: payload,
        ticket: ReadCookie("ticket/session")
    };

    const response = fetch("/firebase/onBackgroundMessage", {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
        },
        body: JSON.stringify(req),
    });
});

function ReadCookie(cname) {
    let name = cname + "=";
    let decodedCookie = decodeURIComponent(document.cookie);
    let ca = decodedCookie.split(';');
    let resValue = "";
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') {
            c = c.substring(1);
        }
        if (c.indexOf(name) == 0) {
            resValue = c.substring(name.length, c.length);
        }
    }
    return resValue;
}

firebaseMessaging.onMessage(function (payload) {
    let req = {
        message: payload,
        ticket: ReadCookie("ticket/session")
    };
    const response = fetch("/firebase/onMessage", {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
        },
        body: JSON.stringify(req),
    });

    navigator.serviceWorker.register('messaging-sw.js');
    navigator.serviceWorker.ready.then(function (registration) {
        payload.notification.data = payload.notification;
        return registration.showNotification(payload.notification.title, payload.notification);
    }).catch(function (error) {
        console.log('ServiceWorker registration failed', error);
    });
});

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