$('#getIndexMap').click(function () {
    let targetOffset = $('#map').offset().top;
    $('html, body').animate({ scrollTop: targetOffset }, 1000);
    initMap();
});

function populateApartments() {
    $.ajax({
        url: "../lib/apartment.json",
        type: 'get',
        dataType: 'json',
        success: function (data) {
            let apartment = data.map.buildings
            console.log(apartment)
        },
        error: function (e) {
            console.log("error", error)
        }
    })
}
