function missionclick(e) {

    const selectElement = document.getElementById("MissionTitle");
    const selectedOptionId = selectElement.selectedOptions[0].getAttribute("id");
    console.log(selectedOptionId);

    $.ajax({
        url: '/Home/Admin_Story',
        type: "POST",
        data: {

            selectedOptionId: selectedOptionId,


        },
        success: function (data) {


            //document.getElementById("Skillname").value = data.skillId;

            console.log(data);
        },
        error: function (xhr, textStatus, errorThrown) {
            console.log("Error: " + errorThrown);
        }
    });

}



function storyAdminUser(storyId) {

    

    $.ajax({
        url: '/Home/ViewStoryDetails',
        type: 'POST',
        data: { storyId: storyId },
        success: function (data) {
           
            console.log(data);
            $('#exampleModalStory').modal('show');

            document.getElementById('MissionTitle').value = data.missionTitle;
            document.getElementById('StoryTitle').value = data.storyTitle;
            document.getElementById('Description').value = data.description;

            

        },
        error: function () {

            console.log("fail");
            location.reload();
        }
    });

}


function abcd() {
    $('#exampleModalStory').modal('show');
}

function viewStoryDetails(storyId) {

    //var uId = $('#UserEditByAdmin').attr('userId');
    $('#exampleModalStory').modal('show');
    $.ajax({
        url: '/Home/ViewStoryDetails',
        type: 'POST',
        data: { storyId: storyId },
        success: function (data) {
            console.log(data);
       

            //document.getElementById('storyid').value = data.userId;

            

        },
        error: function () {

            console.log("fail");
        
        }
    });

}

// data table

$(document).ready(function () {

    $('#table_id').DataTable();


});