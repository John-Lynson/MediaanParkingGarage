document.addEventListener("DOMContentLoaded", function () {
    var preloader = document.getElementById("preloader");

    // Check if navigating within the app
    if (sessionStorage.getItem('isNavigatingWithinApp')) {
        // Hide preloader immediately if we're navigating within the app
        preloader.style.display = 'none';
    } else {
        // This is either the first load or a reload
        if (!sessionStorage.getItem('isPreloaderShown')) {
            preloader.style.display = 'block';
            sessionStorage.setItem('isPreloaderShown', 'true');

            var minPreloaderTime = 1500; // Minimale weergavetijd van de preloader in milliseconden
            var startTime = new Date().getTime(); // Starttijd

            function hidePreloader() {
                var elapsedTime = new Date().getTime() - startTime;
                if (elapsedTime < minPreloaderTime) {
                    setTimeout(function () {
                        preloader.style.display = 'none';
                    }, minPreloaderTime - elapsedTime);
                } else {
                    preloader.style.display = 'none';
                }
            }

            window.addEventListener('load', hidePreloader);
            setTimeout(hidePreloader, 5000); // Maximale weergavetijd als backup
        } else {
            preloader.style.display = 'none';
        }
    }

    // Mark future navigations as within-app navigations
    sessionStorage.setItem('isNavigatingWithinApp', 'true');
});
