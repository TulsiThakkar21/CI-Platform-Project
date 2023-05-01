$(document).ready(function () {

    $('#table_id').DataTable();


});

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





function deleteBannerData(bnrid) {

    if (window.confirm("Are you sure you want to delete?")) {
        $.ajax({
            url: '/Home/Admin_Banner',
            type: "POST",
            data: {
                bnrid: bnrid


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

function editBannerData(bnrid) {

    $.ajax({
        url: '/Home/EditBanner',
        type: 'POST',
        data: { bnrid: bnrid },
        success: function (data) {

            console.log(data);
            $('#exampleModalMT').modal('show');

            document.getElementById('BannerId').value = data.bannerId;
            //document.getElementById('ImageEdit').value = data.image;
            document.getElementById('TextEdit').value = data.text;
            document.getElementById('SortOrderEdit').value = data.sortOrder;

            
            var images = document.createElement("img");
            var imageParent = document.getElementById("imgdiv");

            images.src = "data:image/png;base64," + data.image;
            images.style.width = "60%";
            imageParent.appendChild(images);

            


        },
        error: function () {

            console.log("fail");
            location.reload();
        }
    });
}

function saveEditedBannerData() {

    var bnrid = document.getElementById('BannerId').value;
    var image = document.getElementById('ImageEdit').value;
    var text = document.getElementById('TextEdit').value;
    var sortOrder = document.getElementById('SortOrderEdit').value;


    $.ajax({
        url: '/Home/SaveBannerData',
        type: "POST",
        data: {
            bnrid: bnrid,
            image: image,
            text: text,
            sortOrder: sortOrder



        },
        success: function () {

            console.log("success");

            //location.reload();
        },
        error: function (xhr, textStatus, errorThrown) {
            console.log("Error: " + errorThrown);




            //location.reload();
        }
    });
}








