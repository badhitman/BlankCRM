function jivo_onLoadCallback() {
    console.log('Widget is active')
}

window.tryInitJivoSite = function () {
    if (window.jivo_version) {
        jivo_init();
        console.log('Widget re active')
    }
};
// window.addEventListener('change', (event) => { window.tryInitJivoSite(); });

(function (history) {
    var pushState = history.pushState;
    history.pushState = function (state, title, url) {
        // Call the original pushState function
        var ret = pushState.apply(this, arguments);
        // Dispatch a custom event after the history change
        window.dispatchEvent(new Event('locationchange'));
        return ret;
    };
    // Also handle replaceState
    var replaceState = history.replaceState;
    history.replaceState = function (state, title, url) {
        var ret = replaceState.apply(this, arguments);
        window.dispatchEvent(new Event('locationchange'));
        return ret;
    };
    // Listen for the native popstate event and dispatch the custom event
    window.addEventListener('popstate', function () {
        window.dispatchEvent(new Event('locationchange'));
    });
})(window.history);

// Now you can listen for the custom 'locationchange' event
window.addEventListener('locationchange', function () {
    console.log('URL has changed to:', window.location.href);
    window.tryInitJivoSite();
});