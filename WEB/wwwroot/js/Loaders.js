document.addEventListener("DOMContentLoaded", function () {
    var preloader = document.getElementById("preloader");
    var navbar = document.querySelector(".navbar");

    function managePreloaderVisibility(hide) {
        if (preloader && navbar) {
            preloader.style.display = hide ? 'none' : 'block';
            navbar.style.display = hide ? "flex" : "none";
        }
    }

    if (sessionStorage.getItem('isNavigatingWithinApp')) {
        managePreloaderVisibility(true);
    } else {
        sessionStorage.setItem('isPreloaderShown', 'true');
        managePreloaderVisibility(false);

        var minPreloaderTime = 1500;
        var startTime = new Date().getTime();

        function hidePreloader() {
            var elapsedTime = new Date().getTime() - startTime;
            setTimeout(() => managePreloaderVisibility(true), Math.max(minPreloaderTime - elapsedTime, 0));
        }

        window.addEventListener('load', hidePreloader);
        setTimeout(hidePreloader, 5000); // Maximum time limit as a fallback
    }

    sessionStorage.setItem('isNavigatingWithinApp', 'true');
});
