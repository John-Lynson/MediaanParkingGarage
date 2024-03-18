document.addEventListener("DOMContentLoaded", function () {
    var preloader = document.getElementById("preloader");
    var minPreloaderTime = 1500; // Minimale weergavetijd van de preloader in milliseconden (500ms = 0.5 seconde)
    var startTime = new Date().getTime(); // Tijdstip waarop de pagina begint te laden

    function hidePreloader() {
        var elapsedTime = new Date().getTime() - startTime; // Hoe lang de pagina al laadt
        if (elapsedTime < minPreloaderTime) {
            // Als de pagina te snel laadt, wacht dan tot de minimale weergavetijd is bereikt
            setTimeout(function () {
                preloader.style.display = 'none';
            }, minPreloaderTime - elapsedTime);
        } else {
            // Als de minimale weergavetijd al is bereikt of overschreden, verberg de preloader onmiddellijk
            preloader.style.display = 'none';
        }
    }

    window.addEventListener('load', hidePreloader);

    // Optioneel: een maximale tijd instellen voor het geval de pagina extreem langzaam laadt,
    // kunt u de preloader forceren om te verbergen na bijvoorbeeld 5 seconden.
    setTimeout(hidePreloader, 5000);
});
