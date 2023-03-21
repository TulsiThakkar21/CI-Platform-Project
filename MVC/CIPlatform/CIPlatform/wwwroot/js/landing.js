﻿function likeMission(missionId, id) {

    $.ajax({
        url: '/Home/AddFav',
        type: 'POST',
        data: { missionId: missionId, id: id },
        success: function () {
            console.log("success");
        },
        error: function () {
            // Handle error response from the server, e.g. show an error message to the user
            /*alert('Error: Could not like mission.');*/
            console.log("fail");
        }
    });
}

$(function () {
    //var rating = $("#rating");
    var rating = $(this).parent().find(".star");
    $("#rating .star").click(function () {
        debugger
        var star = $(this);
        var stars = star.data("value");
        var missionId = star.data("missionid");

        $.ajax({
            type: "POST",
            url: "/Home/Rate",
            data: { stars: stars, missionId: missionId },
            success: function () {
                console.log("success");
                //debugger
                //rating.removeClass("far").addClass("fas");
                //star.prevAll(".star").removeClass("far").addClass("fas");
                star.removeClass("far").addClass("fas");
            }
        });
    });
});




function rccall(MissionId) {
    var checkbox_value = "";
    var checked_Val = [];
    $(":checkbox").each(function () {
        var ischecked = $(this).is(":checked");
        if (ischecked) {
            checkbox_value += $(this).val() + ",";
            checked_Val.push($(this).val());
        }

    });

    alert(checkbox_value);
    console.log(checkbox_value);


    $.ajax({
        url: '/Home/Recommendedto',
        type: 'POST',
        data: {
            checkbox_value: checkbox_value,
            MissionId: MissionId
        },
        success: function (data) {
            location.reload();
            console.log("success");
            
        },
        error: function () {
            // Handle error response from the server, e.g. show an error message to the user
            /*alert('Error: Could not like mission.');*/
            console.log("fail");
            location.reload();
        }
    });


   


}




    var checkboxes = document.querySelectorAll(".checkbox");

    var filtersSection = document.querySelector(".filters-section");

    var listArray = [];

    var filterList = document.querySelector(".filter-list");

    var len = listArray.length;

    for (var checkbox of checkboxes) {
        checkbox.addEventListener("click", function () {
            if (this.checked == true) {
                addElement(this, this.value);
            } else {
                removeElement(this.value);
            }
        });
        }

    function addElement(current, value) {
        //let filtersSection = document.querySelector(".filters-section");

        let createdTag = document.createElement("span");
    createdTag.classList.add("filter-list");
    createdTag.classList.add("ps-3");
    createdTag.classList.add("pe-1");
    createdTag.classList.add("me-2");
    createdTag.innerHTML = value;

    createdTag.setAttribute("id", value);
    let crossButton = document.createElement("button");
    crossButton.classList.add("filter-close-button");
    let cross = "&times;";

    crossButton.addEventListener("click", function () {
        let elementToBeRemoved = document.getElementById(value);

    console.log(elementToBeRemoved);
    console.log(current);
    elementToBeRemoved.remove();

    current.checked = false;
            });

    crossButton.innerHTML = cross;

    createdTag.appendChild(crossButton);
    filtersSection.appendChild(createdTag);
        }

    function removeElement(value) {
        let filtersSection = document.querySelector(".filters-section");

    let elementToBeRemoved = document.getElementById(value);
    filtersSection.removeChild(elementToBeRemoved);
        }





function commentadd(MissionId) {
    var commenttext = document.getElementById('textAreas').value;
    $.ajax({
        url: '/Home/VolunteeringMission',
        type: "POST",
        data: {
            commenttext: commenttext,
            MissionId: MissionId
        },
        success: function () {
            document.getElementById("processedData").innerHTML = response.processedData;
        },
        error: function (xhr, textStatus, errorThrown) {
            console.log("Error: " + errorThrown);
        }
    });


}