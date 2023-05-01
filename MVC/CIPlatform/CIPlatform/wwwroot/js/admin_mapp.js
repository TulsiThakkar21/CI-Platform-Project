function approved(missionId, uid)
{
    $.ajax({

        url: '/Home/Approved',
        type: 'POST',
        data: {
            missionId: missionId,
            uid : uid

            },
        success: function () {
            location.reload();
            console.log("success");

        },
        error: function (xhr, textStatus, errorThrown) {
            console.log("Error: " + errorThrown);
            
        }
    });


   
}

function declined(missionId, uid) {

    $.ajax({

        url: '/Home/Declined',
        type: 'POST',
        data: {
            missionId: missionId,
            uid: uid

        },
        success: function () {
            location.reload();
            console.log("success");

        },
        error: function (xhr, textStatus, errorThrown) {
            console.log("Error: " + errorThrown);

        }
    });


}

$(document).ready(function () {
    "use strict";


    $('#sidebarCollapse').on('click', function () {
        $('#sidebar').toggleClass('collapse-sidebar');
    });


});


function updateDateTime() {
    var currentTime = new Date();
    var formattedDateTime = currentTime.toLocaleString('en-US', { weekday: 'long', year: 'numeric', month: 'long', day: 'numeric', hour: 'numeric', minute: 'numeric', second: 'numeric', hour12: true });
    document.getElementById('current-time').innerHTML = formattedDateTime;
}

setInterval(updateDateTime, 1000);


// data table

$(document).ready(function () {

    $('#table_id').DataTable();


});