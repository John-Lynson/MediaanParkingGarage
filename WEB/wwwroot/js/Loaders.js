document.addEventListener("DOMContentLoaded", function () {
    var preloader = document.getElementById("preloader");
    var navbar = document.querySelector(".navbar");

    function managePreloaderVisibility(hide) {
        if (preloader) {
            preloader.style.display = hide ? 'none' : 'block';
        }
        if (navbar) {
            navbar.style.display = hide ? "flex" : "none";
        }
    }

    // Beheer preloader en navbar zichtbaarheid afhankelijk van navigatie binnen de app
    if (sessionStorage.getItem('isNavigatingWithinApp')) {
        managePreloaderVisibility(true);
    } else {
        // Markeren dat de preloader is getoond
        sessionStorage.setItem('isPreloaderShown', 'true');
        managePreloaderVisibility(false);

        // Minimale tijd dat de preloader zichtbaar moet blijven
        var minPreloaderTime = 4500;
        var startTime = new Date().getTime();

        function hidePreloader() {
            var elapsedTime = new Date().getTime() - startTime;
            // Verbergt de preloader na de minimale tijd, waarbij rekening wordt gehouden met de verstreken tijd sinds de pagina begon met laden
            setTimeout(() => managePreloaderVisibility(true), Math.max(minPreloaderTime - elapsedTime, 0));
        }

        // Luister naar het 'load' event om te verzekeren dat alle pagina-inhoud is geladen voordat de preloader wordt verborgen
        window.addEventListener('load', hidePreloader);

        // Fallback, verbergt de preloader na een maximale tijdlimiet, ongeacht of de pagina volledig geladen is
        setTimeout(hidePreloader, 9000); // Maximale tijdlimiet als backup
    }

    // Marker voor toekomstige navigaties binnen de app
    sessionStorage.setItem('isNavigatingWithinApp', 'true');
});
