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




function editCMSData(CMSId) {


    $.ajax({
        url: '/Home/EditCMSData',
        type: 'POST',
        data: { CMSId: CMSId },
        success: function (data) {
            console.log(data);
            $('#exampleModalCMS').modal('show');

            document.getElementById('CmsPageId').value = data.cmsPageId;
            document.getElementById('TitleEdit').value = data.title;
            document.getElementById('DescriptionEdit').value = data.description;
            document.getElementById('SlugEdit').value = data.slug;
            document.getElementById('StatusEdit').value = data.status;
            

        },
        error: function () {

            console.log("fail");
            location.reload();
        }
    });
}



function saveEditedDataCMS() {

    var cmsId = document.getElementById('CmsPageId').value;
    var title = document.getElementById('TitleEdit').value;
    var desc = document.getElementById('DescriptionEdit').value;
    var slug = document.getElementById('SlugEdit').value;
    var status = document.getElementById('StatusEdit').value;
    

    $.ajax({
        url: '/Home/SaveCMSData',
        type: "POST",
        data: {
            cmsId: cmsId,
            title: title,
            desc: desc,
            slug: slug,
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


function deleteCmsData(cmsid) {
    if (window.confirm("Are you sure you want to delete?")) {

        $.ajax({
            url: '/Home/CMSPage',
            type: "POST",
            data: {
                cmsid: cmsid


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
}

// data table

$(document).ready(function () {

    $('#table_id').DataTable();


});