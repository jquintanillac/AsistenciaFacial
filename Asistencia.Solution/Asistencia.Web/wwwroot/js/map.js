let map;
let marker;
let dotNetHelper;

function initMap(lat, lng) {
    const defaultLocation = { lat: lat || -12.046374, lng: lng || -77.042793 }; // Lima, Peru default

    map = new google.maps.Map(document.getElementById("map"), {
        zoom: 15,
        center: defaultLocation,
    });

    if (lat && lng) {
        placeMarker(defaultLocation);
    }

    map.addListener("click", (e) => {
        placeMarker(e.latLng);
        if (dotNetHelper) {
            dotNetHelper.invokeMethodAsync("UpdateCoordinates", e.latLng.lat(), e.latLng.lng());
        }
    });
}

function placeMarker(location) {
    if (marker) {
        marker.setPosition(location);
    } else {
        marker = new google.maps.Marker({
            position: location,
            map: map,
        });
    }
}

function setDotNetHelper(helper) {
    dotNetHelper = helper;
}

// Global initialization if needed, though we call initMap from Blazor usually
window.initMap = initMap;
window.setDotNetHelper = setDotNetHelper;
