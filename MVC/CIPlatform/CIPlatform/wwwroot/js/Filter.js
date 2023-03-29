

function SortBySubcat(s) {
    var subcats_id = '';
    $('[name="subcats"]').each(function (i, e) {
        if ($(e).is(':checked')) {

            var comma = subcats_id.length === 0 ? '' : ',';
            subcats_id += (comma + e.value);
        }
    });

    jQuery.ajax({
        type: "GET",
        url: '/Home/_Grid',
        data: { subcats_id: subcats_id, },
        success: function (data) {
            $('#filter_products').html(data);
            console.log("successs");
        }
    });
}


function FilterCity(s) {
    var filtercity = '';
    $('[name="filcity"]').each(function (i, e) {
        if ($(e).is(':checked')) {

            var comma = filtercity.length === 0 ? '' : ',';
            filtercity += (comma + e.value);
        }
    });

    jQuery.ajax({
        type: "GET",
        url: '/Home/_Grid',
        data: { filtercity: filtercity, },
        success: function (data) {
            $('#filter_products').html(data);
            console.log("successs");
        }
    });
}
function FilterCountry(s) {
    var filterCountry = '';
    $('[name="filcountry"]').each(function (i, e) {
        if ($(e).is(':checked')) {

            var comma = filterCountry.length === 0 ? '' : ',';
            filterCountry += (comma + e.value);
        }
    });

    jQuery.ajax({
        type: "GET",
        url: '/Home/_Grid',
        data: { filterCountry: filterCountry, },
        success: function (data) {
            $('#filter_products').html(data);
            console.log("successs");
        }
    });
}