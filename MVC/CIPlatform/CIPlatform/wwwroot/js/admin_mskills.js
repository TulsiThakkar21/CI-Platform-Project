
        // Get references to the select elements and buttons
    const selectLeft = document.getElementById('select-left');
    const selectRight = document.getElementById('select-right');
    const btnAdd = document.getElementById('btn-add');
    const btnRemove = document.getElementById('btn-remove');

        // Add event listeners to the buttons
        btnAdd.addEventListener('click', () => {
        // Move selected options from left to right

        Array.from(selectLeft.selectedOptions).forEach(option => {
            selectRight.appendChild(option);

        });
        });

        btnRemove.addEventListener('click', () => {
        // Move selected options from right to left
        Array.from(selectRight.selectedOptions).forEach(option => {
            selectLeft.appendChild(option);
        });
        });



var skillids = [];

function saveSkills() {
    $("#select-down option").remove();

    var $options = $("#select-right > option").clone();

    var skillidslength = $options.length;
    $('#select-down').append($options);

    var dropdown = document.getElementById("select-down");
    var selectedOptions = dropdown.selectedOptions;


    for (var i = 0; i < skillidslength; i++) {
        skillids.push($options[i].id);
    }


}


function missionclick(e) {

    const selectElement = document.getElementById("MissionTitle");
    const selectedOptionId = selectElement.selectedOptions[0].getAttribute("id");
    console.log(selectedOptionId);

    $.ajax({
        url: '/Home/Admin_MissionSkills',
        type: "POST",
        data: {

            selectedOptionId: selectedOptionId,
            skillids: skillids

        },
        success: function (data) {


            document.getElementById("Skillname").value = data.skillId;

            console.log(data);
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