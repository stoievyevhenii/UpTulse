function isDevice() {
    return /android|iphone|ipad|ipod|iemobile|opera mini|mobile/i.test(navigator.userAgent);
}

function isLandscape() {
    return window.innerWidth > window.innerHeight;
}