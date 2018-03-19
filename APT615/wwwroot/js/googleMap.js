$('#getIndexMap').click(function () {
    let targetOffset = $('#map').offset().top;
    $('html, body').animate({ scrollTop: targetOffset }, 1000);
    initMap();
});

const initMap = () => {
    const styledMapType = new google.maps.StyledMapType(
        [
            {
                "featureType": "administrative.locality",
                "elementType": "all",
                "stylers": [
                    {
                        "hue": "#2c2e33"
                    },
                    {
                        "saturation": 7
                    },
                    {
                        "lightness": 19
                    },
                    {
                        "visibility": "on"
                    }
                ]
            },
            {
                "featureType": "administrative.locality",
                "elementType": "labels.text",
                "stylers": [
                    {
                        "visibility": "on"
                    },
                    {
                        "saturation": "-3"
                    }
                ]
            },
            {
                "featureType": "administrative.locality",
                "elementType": "labels.text.fill",
                "stylers": [
                    {
                        "color": "#f39247"
                    }
                ]
            },
            {
                "featureType": "landscape",
                "elementType": "all",
                "stylers": [
                    {
                        "hue": "#ffffff"
                    },
                    {
                        "saturation": -100
                    },
                    {
                        "lightness": 100
                    },
                    {
                        "visibility": "simplified"
                    }
                ]
            },
            {
                "featureType": "poi",
                "elementType": "all",
                "stylers": [
                    {
                        "hue": "#ffffff"
                    },
                    {
                        "saturation": -100
                    },
                    {
                        "lightness": 100
                    },
                    {
                        "visibility": "off"
                    }
                ]
            },
            {
                "featureType": "poi.school",
                "elementType": "geometry.fill",
                "stylers": [
                    {
                        "color": "#f39247"
                    },
                    {
                        "saturation": "0"
                    },
                    {
                        "visibility": "on"
                    }
                ]
            },
            {
                "featureType": "road",
                "elementType": "geometry",
                "stylers": [
                    {
                        "hue": "#ff6f00"
                    },
                    {
                        "saturation": "100"
                    },
                    {
                        "lightness": 31
                    },
                    {
                        "visibility": "simplified"
                    }
                ]
            },
            {
                "featureType": "road",
                "elementType": "geometry.stroke",
                "stylers": [
                    {
                        "color": "#f39247"
                    },
                    {
                        "saturation": "0"
                    }
                ]
            },
            {
                "featureType": "road",
                "elementType": "labels",
                "stylers": [
                    {
                        "hue": "#008eff"
                    },
                    {
                        "saturation": -93
                    },
                    {
                        "lightness": 31
                    },
                    {
                        "visibility": "on"
                    }
                ]
            },
            {
                "featureType": "road.arterial",
                "elementType": "geometry.stroke",
                "stylers": [
                    {
                        "visibility": "on"
                    },
                    {
                        "color": "#f3dbc8"
                    },
                    {
                        "saturation": "0"
                    }
                ]
            },
            {
                "featureType": "road.arterial",
                "elementType": "labels",
                "stylers": [
                    {
                        "hue": "#bbc0c4"
                    },
                    {
                        "saturation": -93
                    },
                    {
                        "lightness": -2
                    },
                    {
                        "visibility": "simplified"
                    }
                ]
            },
            {
                "featureType": "road.arterial",
                "elementType": "labels.text",
                "stylers": [
                    {
                        "visibility": "off"
                    }
                ]
            },
            {
                "featureType": "road.local",
                "elementType": "geometry",
                "stylers": [
                    {
                        "hue": "#e9ebed"
                    },
                    {
                        "saturation": -90
                    },
                    {
                        "lightness": -8
                    },
                    {
                        "visibility": "simplified"
                    }
                ]
            },
            {
                "featureType": "transit",
                "elementType": "all",
                "stylers": [
                    {
                        "hue": "#e9ebed"
                    },
                    {
                        "saturation": 10
                    },
                    {
                        "lightness": 69
                    },
                    {
                        "visibility": "on"
                    }
                ]
            },
            {
                "featureType": "water",
                "elementType": "all",
                "stylers": [
                    {
                        "hue": "#e9ebed"
                    },
                    {
                        "saturation": -78
                    },
                    {
                        "lightness": 67
                    },
                    {
                        "visibility": "simplified"
                    }
                ]
            }
        ],
        { name: 'Styled Map' });

    let uluru = { lat: 36.16113, lng: -86.78578 };
    let infoWindow;
    let marker;
    let markerIcon = {
        url: '../images/mapMarker.svg',
        scaledSize: new google.maps.Size(32, 32),
        anchor: new google.maps.Point(0, 32) // lets offset the marker image
    };

    // Create a map object, and include the MapTypeId to add
    // to the map type control.
    let map = new google.maps.Map(document.getElementById('map'), {
        center: uluru,
        zoom: 12,
        disableDefaultUI: true
    });

    infowindow = new google.maps.InfoWindow();

    $.ajax({
        url: "../js/apartment.json",
        type: 'get',
        dataType: 'json',
        success: function (data) {
            let apartments = data.apts
            apartments.forEach(apt => {
                createMarker(apt);
            })

        }//END SUCCESS
    })

    function createMarker(place) {
        let login = '<a class="nav-link" href="/Account/Login">Log in</a>';
        let track = '<a id="apartmentsNav" class="nav-link" href="/Apartments/Track">Track</a>'
        let markerLocation = { lat: place.lat, lng: place.lng };
        let marker = new google.maps.Marker({
            icon: markerIcon,
            position: markerLocation,
            animation: google.maps.Animation.DROP,
            map: map
        });
        google.maps.event.addListener(marker, 'click', function () {
            infowindow.setContent('<div id="content" class"content-center">' +
                '<p class="text-center">' + login + ' or ' + track +'</p>' +
                '<p class="text-center"><strong>' + place.name + '<strong></p>' +
                '<p class="text-center">' + place.rentSummary + '</p>' +
               '</div>');
            infowindow.open(map, this);
        });
    }

    //Associate the styled map with the MapTypeId and set it to display.
    map.mapTypes.set('styled_map', styledMapType);
    map.setMapTypeId('styled_map');
}//END INIT MAP



