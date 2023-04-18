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





function deleteMTData(mtId) {

    $.ajax({
        url: '/Home/Admin_MissionTheme',
        type: "POST",
        data: {
            mtId: mtId


        },
        success: function (data) {
            location.reload();
            console.log("Data Deleted");

        },
        error: function (xhr, textStatus, errorThrown) {
            console.log("Error: " + errorThrown);

        }
    });
}

function editMTData(mtid) {
    
    $.ajax({
        url: '/Home/EditMTData',
        type: 'POST',
        data: { mtid: mtid },
        success: function (data) {
            console.log(data);
            $('#exampleModalMT').modal('show');
            //jQuery('#exampleModalMT').modal({
            //    show: true,
            //    backdrop: 'static'
            //});
            document.getElementById('MissionThemeId').value = data.missionThemeId;
            document.getElementById('TitleEdit').value = data.title;           
            document.getElementById('StatusEdit').value = data.status;


        },
        error: function () {

            console.log("fail");
            location.reload();
        }
    });
}

function saveEditedMTData() {

    var mtid = document.getElementById('MissionThemeId').value;
    var title = document.getElementById('TitleEdit').value;
    var status = document.getElementById('StatusEdit').value;


    $.ajax({
        url: '/Home/SaveMTData',
        type: "POST",
        data: {
            mtid: mtid,
            title: title,
            status: status



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

    $('#table_id').DataTable();

  
});




