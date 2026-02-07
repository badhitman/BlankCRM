function updateHeight(domId, setHeight) {
    // console.warn(`domId:${domId}; setHeight:${setHeight};`);
    //let sender = $(`#iframe:${domId}`);
    //sender.height(setHeight);
}

function SetDotNetHelper(dotNetHelper) {
    window.dotNetHelper = dotNetHelper;
}

function js_upload_handler(blobInfo, success, failure, progress) {
    // console.log(JSON.stringify(blobInfo));
    window.dotNetHelper.invokeMethodAsync('UploadHandler', blobInfo.base64(), blobInfo.filename())
        .then((data) => {
            success(data);
        });
}

window.FrameHeightUpdate = (() => {
    return {
        Reload(id) {
            let iFrame = document.getElementById(`frame:${id}`);
            $(iFrame).css('height', iFrame.contentWindow.document.body.scrollHeight * 1.1);
            return iFrame.contentWindow.document.body.scrollHeight;
        }
    };
})();

window.HighlightBlock = (() => {
    return {
        Init(dom_id, dotNetReference) {
            hljs.highlightAll();
            /*hljs.highlightBlock(document.querySelector('code'));*/
        }
    };
})();

window.BoundingClientRect = (() => {
    return {
        Height(id) {
            var _d = $(`#${id}`);
            if (_d.length == 0)
                return 0;

            let height = _d.height();
            if (isNaN(height)) {
                return 0;
            }

            return height;
        },
        Width(id) {
            var _d = $(`#${id}`);
            if (_d.length == 0)
                return 0;

            let width = _d.width();
            if (isNaN(width)) {
                return 0;
            }

            return width;
        },
        X(id) {
            var _d = $(`#${id}`);
            if (_d.length == 0)
                return 0;

            var _p = _d.position();
            let left = _p.left;
            if (isNaN(left)) {
                return 0;
            }

            return left;
        },
        Y(id) {
            var _d = $(`#${id}`);
            if (_d.length == 0)
                return 0;

            var _p = _d.position();
            let top = _p.top;

            if (isNaN(top)) {
                return 0;
            }

            return top;
        }
    };
})();
function autoGrow(el) {
    if (el.style == undefined)
        return;

    el.style.height = '5px';
    el.style.height = el.scrollHeight + 'px';
}

window.autoGrowManage = (() => {
    return {
        registerGrow(dom_id, dotNetReference) {
            autoGrow(this);
            if (this.scrollHeight !== undefined)
                dotNetReference.invokeMethodAsync('EditorDataChanged', this.scrollHeight);
        }
    };
})();

window.methods = {
    CreateCookie: function (name, value, seconds, path) {
        // console.warn(`call -> methods.CreateCookie(name:${name}, value:${value}, seconds:${seconds}, path:${path})`);
        var expires;
        if (seconds) {
            var date = new Date();
            date.setTime(date.getTime() + (seconds * 1000));
            expires = "; expires=" + date.toGMTString();
        }
        else {
            expires = "";
        }
        window.methods.DeleteCookie(name);
        document.cookie = name + "=" + value + expires + `; path=${path}`;
    },
    UpdateCookie: function (name, seconds, path) {
        let value = window.methods.ReadCookie(name);
        //console.warn(`call -> methods.UpdateCookie(name:${name}, seconds:${seconds}, path:${path}); set:${value}`);
        window.methods.CreateCookie(name, value, seconds, path);
    },
    ReadCookie: function (cname) {
        // console.warn(`call -> methods.ReadCookie(cname:${cname})`);
        var name = cname + "=";
        var decodedCookie = decodeURIComponent(document.cookie);
        var ca = decodedCookie.split(';');
        var resValue = "";
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
    },
    DeleteCookie: function (name) {
        // console.warn(`call -> methods.DeleteCookie(name:${name})`);
        if (window.methods.ReadCookie(name)) {
            document.cookie = name + "=" +
                //((path) ? ";path=" + path : "") +
                ";expires=Thu, 01 Jan 1970 00:00:01 GMT";
        }
    },
    AboutUserAgent: function () {
        const UserAgent = navigator.userAgent;
        const Language = navigator.language;
        const CookieEnabled = navigator.cookieEnabled;
        return { UserAgent, Language, CookieEnabled };
    },
    PlayAudio: function (domId) {
        var audio = document.getElementById(`${domId}`);
        if (audio) {
            audio.play();
        }
    }
}

window.effects = {
    JQuery: function (selectedEffect, domId) {
        // Most effect types need no options passed by default
        var options = {};
        // some effects have required parameters
        if (selectedEffect === "scale") {
            options = { percent: 50 };
        } else if (selectedEffect === "size") {
            options = { to: { width: 200, height: 60 } };
        }

        // Run the effect
        $(`#${domId}`).effect(selectedEffect, options, 500);
    },
    Toast: (heading, text, icon, loader, loaderBg) => {
        $.toast({
            heading: 'Information',
            text: 'Loaders are enabled by default. Use `loader`, `loaderBg` to change the default behavior',
            icon: 'info',
            loader: true,        // Change it to false to disable loader
            loaderBg: '#9EC600'  // To change the background
        })
    }
}

window.clipboardCopy = {
    copyText: function (text) {
        parent.navigator.clipboard.writeText(text).then(function () {
            //alert("Copied to clipboard!");
        })
            .catch(function (error) {
                alert(error);
            });
    }
}

window.getSelectedText = (element) => {
    // Check if the element is a textarea or input
    if (element && typeof element.selectionStart === 'number' && typeof element.selectionEnd === 'number') {
        const Start = element.selectionStart;
        const End = element.selectionEnd;
        const StringValue = element.value.slice(start, end)
        return { Start, End, StringValue };
    }
    let gs = window.getSelection();
    //console.warn(JSON.stringify());
    // Fallback for other elements, though not strictly needed for textarea
    return gs.toString(gs);
};

window.bootstrapTheme = {
    IsDark: function () {
        let attrName = 'data-bs-theme';
        var is_dark = document.documentElement.getAttribute(attrName);
        if (is_dark == 'light')
            document.documentElement.setAttribute(attrName, 'dark');
        else
            document.documentElement.setAttribute(attrName, 'light');
    }
}

function DOMContentLoaded() {
    if (window.Telegram != undefined && window.Telegram != null) {
        var tg = window.Telegram;
        if (window.Telegram.WebApp.initData == "" || window.Telegram.WebApp.initData == undefined || window.Telegram.WebApp.initData == null) {
            //console.warn(JSON.stringify(tg));
            //Blazor.start().then(function () { console.warn("Blazor started!") });
            return;
        }
        //console.warn(JSON.stringify(window.Telegram.WebApp.initData));
        $.ajax({
            type: 'GET',
            url: '/authorize/ping',
            success: function (response) {
                console.warn(JSON.stringify(response));
            },
            error: function (response) {
                console.warn(JSON.stringify(response));
                let tg = window.Telegram.WebApp;
                let model = {};
                model.InitData = tg.initData;
                if (model.InitData != undefined && model.InitData != null) {
                    tg.BackButton.isVisible = false;
                    tg.BackButton.hide();
                    tg.expand();
                    tg.disableVerticalSwipes();
                    $.ajax({
                        type: 'POST',
                        url: '/telegram/authorize',
                        data: JSON.stringify(model),
                        success: function (response) {
                            // if (response.Success) {
                            //console.warn(JSON.stringify(response));
                            window.location.reload();
                            // }
                        },
                        error: function (response) {
                            console.warn(JSON.stringify(response));
                        },
                        contentType: "application/json"
                    });
                }
            },
            contentType: "application/json"
        });
    }

    // console.warn(JSON.stringify(tg));
    //Blazor.start().then(function () { console.warn("Blazor started!") });

    /*
    Blazor.start({ssr: { }, circuit: { }, webAssembly: { }});
    */
}