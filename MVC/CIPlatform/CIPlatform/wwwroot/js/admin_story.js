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
           
            console.log(data.ob1);
            $('#exampleModalStory').modal('show');

            document.getElementById('MissionTitle').value = data.obj1.missionTitle;
            document.getElementById('StoryTitle').value = data.obj1.storyTitle;
            document.getElementById('Description').value = data.obj1.description;

            var filePath = data.obj2;
           
            for (var i = 0; i < data.obj2.length; i++) {
                var img = document.createElement('img');
                img.src = data.obj2[i];
                img.id = i;
                /* img.onclick = removeimg;*/


                var photoContainer = document.getElementById('drop-area');
                photoContainer.appendChild(img);

            }
            //const para = document.createElement("img");
            //para.src = "This is a paragraph";
            //document.body.appendChild(para);
            

        },
        error: function () {

            console.log("fail");
            location.reload();
        }
    });

}


// approve

function approved(missionId, uid) {
    $.ajax({

        url: '/Home/ApprovedStory',
        type: 'POST',
        data: {
            missionId: missionId,
            uid: uid

        },
        success: function () {
            console.log("success");

        },
        error: function (xhr, textStatus, errorThrown) {
            console.log("Error: " + errorThrown);

        }
    });



}


// decline

function declined(missionId, uid) {

    $.ajax({

        url: '/Home/DeclinedStory',
        type: 'POST',
        data: {
            missionId: missionId,
            uid: uid

        },
        success: function () {
            console.log("success");

        },
        error: function (xhr, textStatus, errorThrown) {
            console.log("Error: " + errorThrown);

        }
    });


}

// delete story

function deletestory(storyid) {
    $.ajax({
        url: '/Home/Admin_Story',
        type: 'POST',
        data: { storyid: storyid },
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