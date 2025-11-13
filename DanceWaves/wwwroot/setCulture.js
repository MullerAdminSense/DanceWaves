window.setCulture = function (culture) {
    if (!culture) {
        console.warn("setCulture: cultura inválida");
        return;
    }

    const currentPath = window.location.pathname + window.location.search + window.location.hash;
    const returnUrl = encodeURIComponent(currentPath);
    const targetUrl = `/setculture/${culture}?returnUrl=${returnUrl}`;

    console.debug("Alterando cultura para:", culture, "retornando para:", currentPath);
    window.location.href = targetUrl;
};