
function editSkillData(skillId) {

    $.ajax({
        url: '/Home/EditSkillsData',
        type: 'POST',
        data: { skillId: skillId },
        success: function (data) {
            console.log(data);
            $('#exampleModalSkills').modal('show');

            document.getElementById('SkillsId').value = data.skillId;
            document.getElementById('SkillNameEdit').value = data.skillName;
            document.getElementById('StatusEdit').value = data.status;


        },
        error: function () {

            console.log("fail");
            
        }
    });
}

function saveEditedSkillData() {

    var skillsId = document.getElementById('SkillsId').value;
    var skillsname = document.getElementById('SkillNameEdit').value;
    var status = document.getElementById('StatusEdit').value;


    $.ajax({
        url: '/Home/SaveSkillsData',
        type: "POST",
        data: {
            skillsId: skillsId,
            skillsname: skillsname,
            status: status



        },
        success: function () {
            //location.reload();
            console.log("success");
            //Swal.fire({
            //    position: 'top-end',
            //    icon: 'success',
            //    title: 'Your data has been saved',
            //    showConfirmButton: false,
            //    timer: 2500
            //})
        },
        error: function (xhr, textStatus, errorThrown) {
            console.log("Error: " + errorThrown);
            //Swal.fire({
            //    icon: 'error',
            //    title: 'Oops...',
            //    text: 'Something went wrong!',

            //})
        }
    });
}


function deleteSkillData(skillsId) {

    $.ajax({
        url: '/Home/Admin_Skills',
        type: "POST",
        data: {
            skillsId: skillsId


        },
        success: function (data) {
            location.reload();
            console.log("Data Deleted");

        },
        error: function (xhr, textStatus, errorThrown) {
            console.log("Error: " + errorThrown);
            console.log("success");
           
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