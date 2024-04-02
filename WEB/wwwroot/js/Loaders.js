document.addEventListener("DOMContentLoaded", function () {
    var preloader = document.getElementById("preloader");
    var navbar = document.querySelector(".navbar"); // Pas deze selector aan aan jouw HTML

    // Check if navigating within the app
    if (sessionStorage.getItem('isNavigatingWithinApp')) {
        // Hide preloader immediately if we're navigating within the app
        preloader.style.display = 'none';
        navbar.style.display = "flex"; // Zorg ervoor dat de navbar weer wordt getoond
    } else {
        // This is either the first load or a reload
        if (!sessionStorage.getItem('isPreloaderShown')) {
            preloader.style.display = 'block';
            navbar.style.display = "none"; // Verberg de navbar tijdelijk
            sessionStorage.setItem('isPreloaderShown', 'true');

            var minPreloaderTime = 1500; // Minimale weergavetijd van de preloader in milliseconden
            var startTime = new Date().getTime(); // Starttijd

            function hidePreloader() {
                var elapsedTime = new Date().getTime() - startTime;
                if (elapsedTime < minPreloaderTime) {
                    setTimeout(function () {
                        preloader.style.display = 'none';
                        navbar.style.display = "flex"; // Toon de navbar weer
                    }, minPreloaderTime - elapsedTime);
                } else {
                    preloader.style.display = 'none';
                    navbar.style.display = "flex"; // Toon de navbar weer
                }
            }

            window.addEventListener('load', hidePreloader);
            setTimeout(hidePreloader, 5000); // Maximale weergavetijd als backup
        } else {
            preloader.style.display = 'none';
            navbar.style.display = "flex"; // Zorg ervoor dat de navbar wordt getoond
        }
    }

    // Mark future navigations as within-app navigations
    sessionStorage.setItem('isNavigatingWithinApp', 'true');
});
