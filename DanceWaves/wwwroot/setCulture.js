window.setCulture = function (culture) {
    var expires = new Date();
    expires.setFullYear(expires.getFullYear() + 1);
    // Remove Secure em desenvolvimento (HTTP) - será adicionado automaticamente em produção (HTTPS)
    var isSecure = window.location.protocol === 'https:';
    var cookieString = ".AspNetCore.Culture=c=" + culture + "|uic=" + culture + "; path=/; expires=" + expires.toUTCString();
    if (isSecure) {
        cookieString += "; Secure";
    }
    document.cookie = cookieString;
    console.log("Culture cookie set to:", culture);
    window.location.href = "/setculture/" + culture;
};