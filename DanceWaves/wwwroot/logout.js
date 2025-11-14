window.logoutRedirect = function (url) {
    if (url) {
        window.location.href = url;
    } else {
        window.location.href = '/login';
    }
};

