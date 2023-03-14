
function addToFavorites(missionId) {
    const urlParams = new URLSearchParams(window.location.search);
    urlParams.set('missionId', missionId);

    const newUrl = window.location.pathname + '?' + urlParams.toString();
    history.pushState(null, '', newUrl);
    //alert("hello");
    $.ajax({
        url: '/Home/Add',
        type: 'POST',
        data: { missionId: missionId },
        success: function () {
            // Change button color to red
            $('.btn-primary').addClass('btn-danger').removeClass('btn-primary');
        }
    });
}



function removeFromFavorites(missionId) {

    const urlParams = new URLSearchParams(window.location.search);
    urlParams.set('missionId', missionId);

    const newUrl = window.location.pathname + '?' + urlParams.toString();
    history.pushState(null, '', newUrl);

    $.ajax({
        url: '/Home/Remove',
        type: 'POST',
        data: { missionId: missionId },
        success: function () {
            // Change button color to blue
            $('.btn-danger').addClass('btn-primary').removeClass('btn-danger');
        }
    });
}


function mecalls(missionId) {
    const urlParams = new URLSearchParams(window.location.search);
    urlParams.set('missionId', missionId);

    const newUrl = window.location.pathname + '?' + urlParams.toString();
    history.pushState(null, '', newUrl);

    $.ajax({
        url: '/Home/Rate',
        type: 'POST',
        data: { missionId: missionId },

    });
}