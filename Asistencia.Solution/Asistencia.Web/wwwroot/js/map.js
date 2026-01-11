let map;
let marker;
let dotNetHelper;

function initMap(lat, lng) {
    const mapElement = document.getElementById("map");
    if (!mapElement) {
        console.warn("Map element not found. Waiting for DOM...");
        return;
    }

    // Usar coordenadas proporcionadas o default (Lima)
    // Importante: Validar explícitamente undefined/null porque 0 es una coordenada válida
    const hasCoords = lat !== undefined && lat !== null && lng !== undefined && lng !== null;
    const defaultLocation = hasCoords ? { lat: parseFloat(lat), lng: parseFloat(lng) } : { lat: -12.046374, lng: -77.042793 };

    map = new google.maps.Map(mapElement, {
        zoom: 15,
        center: defaultLocation,
        clickableIcons: false, // Desactiva popups de negocios al hacer clic
        streetViewControl: false, // Simplifica la interfaz
    });

    // Siempre reiniciar el marcador para la nueva instancia del mapa
    marker = null;
    placeMarker(defaultLocation);

    map.addListener("click", (e) => {
        placeMarker(e.latLng);
        updateBlazor(e.latLng.lat(), e.latLng.lng());
    });
}

function placeMarker(location) {
    if (marker) {
        marker.setPosition(location);
    } else {
        marker = new google.maps.Marker({
            position: location,
            map: map,
            draggable: true, // Permitir arrastrar el pin
            animation: google.maps.Animation.DROP // Animación para que se note al aparecer
        });

        // Evento al terminar de arrastrar el pin
        marker.addListener("dragend", () => {
            const pos = marker.getPosition();
            updateBlazor(pos.lat(), pos.lng());
        });
    }
}

function updateBlazor(lat, lng) {
    if (dotNetHelper) {
        dotNetHelper.invokeMethodAsync("UpdateCoordinates", lat, lng);
    }
}

function setDotNetHelper(helper) {
    dotNetHelper = helper;
}

window.initMap = initMap;
window.setDotNetHelper = setDotNetHelper;