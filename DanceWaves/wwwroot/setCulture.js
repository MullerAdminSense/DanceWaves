window.setCulture = function (culture) {
    document.cookie = ".AspNetCore.Culture=c=" + culture + "|uic=" + culture + "; path=/";
    window.location = window.location.pathname;
};