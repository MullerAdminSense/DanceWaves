window.logoutRedirect = function (url) {
    localStorage.clear();
    sessionStorage.clear();
    if (url) {
        window.location.href = url;
    } else {
        window.location.href = '/login';
    }
};

