


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



// edit user

function editAdminUser(uId) {

    //var uId = $('#UserEditByAdmin').attr('userId');

    $.ajax({
        url: '/Home/EditAdminUser',
        type: 'POST',
        data: { uId: uId },
        success: function (data) {
            console.log(data);
            $('#exampleModalUE').modal('show');

            document.getElementById('userid').value = data.userId;

            document.getElementById('FirstNameEdit').value = data.firstName;
            document.getElementById('LastNameEdit').value = data.lastName;
            document.getElementById('EmailEdit').value = data.email;
            document.getElementById('PasswordEdit').value = data.password;
            //document.getElementById('customFileEdit').value = data.avtar;
            document.getElementById('empidEdit').value = data.employeeId;
            document.getElementById('deptEdit').value = data.department;
            document.getElementById('CityIdEdit').value = data.cityId;
            document.getElementById('CountryIdEdit').value = data.countryId;
            document.getElementById('profiletxtEdit').value = data.profileText;
            document.getElementById('StatusEdit').value = data.status;

        },
        error: function () {

            console.log("fail");
            location.reload();
        }
    });

}

function saveEditedData(uId) {

    var uid = document.getElementById('userid').value;
    var fname = document.getElementById('FirstNameEdit').value;
    var lname = document.getElementById('LastNameEdit').value;
    var email = document.getElementById('EmailEdit').value;
    var pass = document.getElementById('PasswordEdit').value;
    var avtar = document.getElementById('customFileEdit').value;
    var empid = document.getElementById('empidEdit').value;
    var dept = document.getElementById('deptEdit').value;
    var cityid = document.getElementById('CityIdEdit').value;
    var countryid = document.getElementById('CountryIdEdit').value;
    var protxt = document.getElementById('profiletxtEdit').value;
    var status = document.getElementById('StatusEdit').value;

    $.ajax({
        url: '/Home/SaveAdminUserData',
        type: "POST",
        data: {
            uid: uid,
            fname: fname,
            lname: lname,
            email: email,
            pass: pass,
            avtar: avtar,
            empid: empid,
            dept: dept,
            cityid: cityid,
            countryid: countryid,
            protxt: protxt,
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



// data table

$(document).ready(function () {

    $('#table_id').DataTable();


});

// CITY COUNTRY


var countryid = 0;
function countrychange() {

    countryid = document.getElementById('selectitm').value;
    $.ajax({
        url: '/Home/Admin_userCityCountry',
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


function saveAdminUserData() {

    var fname = document.getElementById('FirstName').value;
    var lname = document.getElementById('LastName').value;
    var email = document.getElementById('Email').value;
    var pass = document.getElementById('Password').value;
    var avtar = document.getElementById('customFile').value;
    var empid = document.getElementById('empid').value;
    var dept = document.getElementById('dept').value;
 


    var protxt = document.getElementById('profiletxt').value;
    var status = document.getElementById('Status').value;

    $.ajax({
        url: '/Home/Admin_user',
        type: "POST",
        data: {

            fname: fname,
            lname: lname,
            email: email,
            pass: pass,
            avtar: avtar,
            empid: empid,
            dept: dept,
            cityid: cityname,
            countryid: countryid,
            protxt: protxt,
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