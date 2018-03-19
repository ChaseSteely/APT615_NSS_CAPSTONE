function initAutocomplete() {
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
        { name: 'Styled Map' });//END STYLED MAP

    const nashville = { lat: 36.16113, lng: -86.78578 };
    let infoWindow;
    let marker;
    let icon = {
        url: '../images/mapMarker.svg',
        scaledSize: new google.maps.Size(32, 32),
        origin: new google.maps.Point(0, 0),
        anchor: new google.maps.Point(17, 34)
    };
    const map = new google.maps.Map(document.getElementById('trackMap'), {
        center: nashville,
        zoom: 12,
        zoomControl: false,
        disableDefaultUI: true
    });//END map variable

    const input = document.getElementById('apartmentSearch');
    const searchBox = new google.maps.places.SearchBox(input);

    // Bias the SearchBox results towards current map's viewport.
    map.addListener('bounds_changed', function () {
        searchBox.setBounds(map.getBounds());
    });

    searchBox.addListener('places_changed', function () {
        let places = searchBox.getPlaces();

        if (places.length == 0) {
            return;
        }

        marker;

        // For each place, get the icon, name and location.
        let bounds = new google.maps.LatLngBounds();
        places.forEach(function (place) {
            if (!place.geometry) {
                alert("No details available for input: '" + place.name + "'");
                console.log("Returned place contains no geometry");
                return;
            }

            infowindow = new google.maps.InfoWindow({
                maxWidth: 300
            });

            $.ajax({
                url: "../js/APT615.json",
                type: 'get',
                dataType: 'json',
                success: function (data) {
                    let apts = data.apartments
                    apts.forEach(apt => {
                        if (place.name.includes(apt.Name)) {
                            console.log(apt.Name, place.name)
                            $('#Area').val(apt.Area);
                            $('#ApplicationFee').val(apt.ApplicationFee);
                            $('#PetFee').val(apt.PetFee);
                            $('#MiscFees').val(apt.MiscFees);
                            $('#AdminFee').val(apt.AdminFee);
                        } 
                        $('#Name').val(place.name);
                     
                        let streetNum = [];
                        let streetAddress = [];
                        let address = [];

                        for (var i = 0; i < place.address_components.length; i++) {
                            let addressType = place.address_components[i].types[0];
                            if (addressType === "street_number" ) {
                                streetNum = [(place.address_components[i].short_name)]
                            }
                            if (addressType === "route") {
                                streetAddress = [(place.address_components[i].short_name)]
                            }
                            address = [(streetNum), (streetAddress)].join(' ');

                            $('#Street').val(address);

                            if (addressType === "locality") {
                                $('#City').val(place.address_components[i].short_name);
                            }
                            if (addressType === "administrative_area_level_1") {
                                $('#State').val(place.address_components[i].short_name);
                            }
                            if (addressType === "postal_code") {
                                $('#ZipCode').val(place.address_components[i].short_name);
                            }
                        }
                        $('#Latitude').val(place.geometry.location.lat);
                        $('#Longitude').val(place.geometry.location.lng);
                        $('#Website').val(place.website);
                        createMarker(place, apt);
                       
                    })
                }//END SUCCESS
            })

            let targetOffset = $('#trackMap').offset().top;
            $('html, body').animate({ scrollTop: targetOffset }, 1000);

            if (place.geometry.viewport) {
                // Only geocodes have viewport.
                bounds.union(place.geometry.viewport);
            } else {
                bounds.extend(place.geometry.location);
            }
        });
        map.fitBounds(bounds);
    });


    function createMarker(place, apt) {
        // Create a marker for each place.
        let marker = new google.maps.Marker({
            icon: icon,
            position: place.geometry.location,
            animation: google.maps.Animation.DROP,
            map: map
        });

        google.maps.event.addListener(marker, 'click', function () {
            infowindow.setContent(
                '<div id="content" class"content-center">' +
                '<p class="text-center"><strong>' + place.name + '<strong></p>' +
                '<p class="text-center">' + place.formatted_address + '</p>' +
                `<p class="text-center"><a href="${place.website}" target="_blank">Visit Their Website</a></p>` +
                '<h6 class="text-center">' + apt.AvgRent + '</h6>' +
                '</div>'
            );
            infowindow.open(map, this);
        });
        return;
    }

    //Associate the styled map with the MapTypeId and set it to display.
    map.mapTypes.set('styled_map', styledMapType);
    map.setMapTypeId('styled_map');
} //END initAutoComplete()

$('#apartmentsNav').click(function () {
    initAutoComplete()
});

