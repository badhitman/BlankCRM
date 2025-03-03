////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov
////////////////////////////////////////////////

//function SetDotNetHelper(dotNetHelper) {
//    window.dotNetHelper = dotNetHelper;
//}

//function js_upload_handler(blobInfo, success, failure, progress) {
//    // console.log(JSON.stringify(blobInfo));
//    window.dotNetHelper.invokeMethodAsync('UploadHandler', blobInfo.base64(), blobInfo.filename())
//        .then((data) => {
//            success(data);
//        });
//}

class DOMCleanup {
    static observer;

    static createObserver() {
        const target = document.querySelector(`#cleanupDiv`);

        this.observer = new MutationObserver(function (mutations) {
            const targetRemoved = mutations.some(function (mutation) {
                const nodes = Array.from(mutation.removedNodes);
                return nodes.indexOf(target) !== -1;
            });

            if (targetRemoved) {
                // Cleanup resources here
                // ...

                // Disconnect and delete MutationObserver
                this.observer && this.observer.disconnect();
                delete this.observer;
            }
        });

        this.observer.observe(target.parentNode, { childList: true });
    }

    static async captureImage(imageId, imageStream) {
        const arrayBuffer = await imageStream.arrayBuffer();
        const blob = new Blob([arrayBuffer]);
        const url = URL.createObjectURL(blob);
        document.getElementById(imageId).src = url;
        document.getElementById(imageId).style.display = 'block';
    }
}

window.DOMCleanup = DOMCleanup;