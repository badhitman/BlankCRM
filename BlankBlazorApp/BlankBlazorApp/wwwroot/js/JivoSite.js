function jivo_onLoadCallback() {
    console.log('Widget is active')
}

window.tryInitJivoSite = function () {
    if (window.jivo_version) {
        jivo_init();
        console.log('Widget re active')
    }
};
window.addEventListener('change', (event) => { window.tryInitJivoSite(); });