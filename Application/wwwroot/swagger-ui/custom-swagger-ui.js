var googleLogInRouteNode;
var theExecuteButtonOfGoogleLogIn;

$(document).ready(function () {
    setTimeout(onSwaggerUIReady, 1500)
});

function onSwaggerUIReady() {
    observeGoogleRoute()
    onDocumentStructureChanged()
    observeDOM(document.documentElement, onDocumentStructureChanged)
}

function observeGoogleRoute() {
    googleLogInRouteNode = document.getElementById("operations-User-get_v0_1_users_open_google_log_in_page")
    observeDOM(googleLogInRouteNode, fixOpenGoogleLogInPageButton);
}

function onDocumentStructureChanged() {
    setCsrfToken()
}

function setCsrfToken() {
    var csrfToken = getCookie('X-CSRF-TOKEN')
    var csrfTokenInputs = $('input[placeholder="X-CSRF-TOKEN"]');
    if (csrfTokenInputs == null || csrfTokenInputs.length === 0) {
        return
    }
    for (let i = 0; i < csrfTokenInputs.length; i++) {
        var csrfTokenInput = csrfTokenInputs[i]
        // @Grin https://stackoverflow.com/a/46012210/3449140
        var nativeInputValueSetter = Object.getOwnPropertyDescriptor(window.HTMLInputElement.prototype, "value").set;
        nativeInputValueSetter.call(csrfTokenInput, csrfToken);
        var event = new Event('input', { bubbles: true });
        csrfTokenInput.dispatchEvent(event);
    }
}

function fixOpenGoogleLogInPageButton(m) {
    var elements = googleLogInRouteNode.getElementsByClassName("execute")
    if (elements == null || elements.length === 0) {
        return
    }
    var theButton = elements[0]
    if (theExecuteButtonOfGoogleLogIn === theButton)
        return

    var theButtonClone = theButton.cloneNode(true);
    theButton.parentNode.replaceChild(theButtonClone, theButton);
    theExecuteButtonOfGoogleLogIn = theButtonClone;
    theButtonClone.onclick = function () {
        this.href = window.location.origin + "/users/open-google-log-in-page";
        window.open(this.href, '_blank').focus();
    };
}

// @vsync https://stackoverflow.com/a/14570614/3449140
var observeDOM = (function () {
    var MutationObserver = window.MutationObserver || window.WebKitMutationObserver;

    return function (obj, callback) {
        if (!obj || obj.nodeType !== 1) return;

        if (MutationObserver) {
            // define a new observer
            var mutationObserver = new MutationObserver(callback)

            // have the observer observe foo for changes in children
            mutationObserver.observe(obj, { childList: true, subtree: true })
            return mutationObserver
        }

        // browser support fallback
        else if (window.addEventListener) {
            obj.addEventListener('DOMNodeInserted', callback, false)
            obj.addEventListener('DOMNodeRemoved', callback, false)
        }
    }
})()

function getCookie(c_name) {
    if (document.cookie.length > 0) {
        c_start = document.cookie.indexOf(c_name + "=");
        if (c_start != -1) {
            c_start = c_start + c_name.length + 1;
            c_end = document.cookie.indexOf(";", c_start);
            if (c_end == -1) c_end = document.cookie.length;
            return unescape(document.cookie.substring(c_start, c_end));
        }
    }
    return "";
}

