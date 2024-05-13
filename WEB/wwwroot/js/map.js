function initMap() {
    // Probeer de huidige locatie van de gebruiker te verkrijgen
    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(function (position) {
            var pos = {
                lat: position.coords.latitude,
                lng: position.coords.longitude
            };

            // Maak een kaart en centreer het op de huidige locatie
            var map = new google.maps.Map(document.getElementById('map'), {
                center: pos,
                zoom: 15
            });

            // Voeg een marker toe op de huidige locatie
            var marker = new google.maps.Marker({
                position: pos,
                map: map,
                title: 'Uw locatie'
            });

        }, function () {
            handleLocationError(true, map, map.getCenter());
        });
    } else {
        // Browser ondersteunt Geolocation niet
        handleLocationError(false, map, map.getCenter());
    }
}

function handleLocationError(browserHasGeolocation, map, pos) {
    var infoWindow = new google.maps.InfoWindow({ map: map });
    infoWindow.setPosition(pos);
    infoWindow.setContent(browserHasGeolocation ?
        'Fout: De Geolocatie service is mislukt.' :
        'Fout: Uw browser ondersteunt geen geolocatie.');
}
