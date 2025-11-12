window.setCulture = function (culture) {
    var expires = new Date();
    expires.setFullYear(expires.getFullYear() + 1);
    var cookieString = ".AspNetCore.Culture=c=" + culture + "|uic=" + culture + "; path=/; expires=" + expires.toUTCString();
    // Não adiciona Secure em HTTP (IIS Express)
    document.cookie = cookieString;
    console.log("Culture cookie set to:", culture);
    window.location.reload();
};