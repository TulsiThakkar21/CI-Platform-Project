var countryid = 0;
function countrychange() {

    countryid = document.getElementById('selectitm').value;
    $.ajax({
        url: '/Home/AdminEditMission',
        type: "POST",
        dataType: 'json',
        data: {


            countryid: countryid


        },
        success: function (response) {
            $('.defaultname').remove();
            document.getElementById('selectcty').innerText = null;
            var option = document.createElement("option");
            option.text = "Please select city";
            option.disabled = true;
            option.selected = true;
            var select = document.getElementById('selectcty');
            select.appendChild(option);

            jQuery.each(response, function (index, item) {
                var option = document.createElement("option");
                option.text = item;
                option.value = item;
                option.id = response.countryid;
                var select = document.getElementById('selectcty');
                select.appendChild(option);
            });



            //var option = document.createElement("option");
            //option.text = "Text";
            //option.value = "myvalue";
            //var select = document.getElementById("id-to-my-select-box");
            //select.appendChild(option);

        },
        error: function (xhr, textStatus, errorThrown) {
            console.log("Error: " + errorThrown);
        }
    });


}

function deleteuser(userid) {
    $.ajax({
        url: '/Home/Admin_user',
        type: 'POST',
        data: { userid: userid },
        success: function () {
            location.reload();
            console.log("success");
        },
        error: function () {

            console.log("fail");
            location.reload();
        }
    });
}

function hello() {

    var fileInput = document.getElementById('fileElem');

    var files = fileInput.files;
    var formData = new FormData();

    for (var i = 0; i < files.length; i++) {

        formData.append('files', files[i]);
        console.log(formData.values);
    }



    var xhr = new XMLHttpRequest();
    xhr.open('POST', '/Home/AdminAddMission', true);
    xhr.onload = function () {
        if (xhr.status === 200) {
            // Handle success
            console.log('Files uploaded successfully.');
        } else {
            // Handle error
            console.error('Error uploading files.');
        }

    };
    xhr.send(formData);


}
function deletemission(missionid) {
    $.ajax({
        url: '/Home/Admin_Mission',
        type: 'POST',
        data: { missionid: missionid },
        success: function () {
            location.reload();
            console.log("success");
        },
        error: function () {

            console.log("fail");
            location.reload();
        }
    });
}

// data table

$(document).ready(function () {

    $('#table_id').DataTable();


});




//document.getElementById("table_id").DataTable();
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


var countryid = 0;
function countrychange() {

    countryid = document.getElementById('selectitm').value;
    $.ajax({
        url: '/Home/Usereditprofileupdate',
        type: "POST",
        dataType: 'json',
        data: {


            countryid: countryid


        },
        success: function (response) {
            $('.defaultname').remove();
            document.getElementById('selectcty').innerText = null;
            var option = document.createElement("option");
            option.text = "Please select city";
            option.disabled = true;
            option.selected = true;
            var select = document.getElementById('selectcty');
            select.appendChild(option);

            jQuery.each(response, function (index, item) {
                var option = document.createElement("option");
                console.log(item);
                option.text = item;
                option.value = item;
                option.id = response.countryid;
                var select = document.getElementById('selectcty');
                select.appendChild(option);
            });




        },
        error: function (xhr, textStatus, errorThrown) {
            console.log("Error: " + errorThrown);
        }
    });


}
var selectedcntry = "";

var cityname = "";

function citychange() {

    cityname = document.getElementById('selectcty').value;



}
function AddSkill(s) {
    var addskill = '';
    $('[name="filcity"]').each(function (i, e) {
        if ($(e).is(':checked')) {

            var comma = addskill.length === 0 ? '' : ',';
            addskill += (comma + e.value);
        }

    });

    jQuery.ajax({
        type: "GET",
        url: yzx,
        data: { addskill: addskill, },
        success: function (data) {


            console.log("successs");
        }
    });
}
