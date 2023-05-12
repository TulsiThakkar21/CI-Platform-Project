function likeMission(missionId, id) {

    $.ajax({
        url: '/Home/AddFav',
        type: 'POST',
        data: { missionId: missionId, id: id },
        success: function () {
            console.log("success");
            location.reload();
        },
        error: function () {
            // Handle error response from the server, e.g. show an error message to the user
            /*alert('Error: Could not like mission.');*/
            console.log("fail");

        }
    });
}

//$(function () {
//    //var rating = $("#rating");
//    var rating = $(this).parent().find(".star");
//    $("#rating .star").click(function () {
//        //debugger
//        var star = $(this);
//        var stars = star.data("value");
//        var missionId = star.data("missionid");

//        $.ajax({
//            type: "POST",
//            url: "/Home/Rate",
//            data: { stars: stars, missionId: missionId },
//            success: function () {
//                console.log("success");
//                //debugger
//                //rating.removeClass("far").addClass("fas");
//                //star.prevAll(".star").removeClass("far").addClass("fas");
//                star.removeClass("far").addClass("fas");
//                location.reload();
//            }
//        });
//    });
//});




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
            location.reload();
        },
        error: function (xhr, textStatus, errorThrown) {
            console.log("Error: " + errorThrown);
        }
    });


}



//function addstory() {

//    var appliedMission= $('#exampleFormControlSelect12').val();
//    var storyTitle = document.getElementById('sTitle').value;
//    var pubDate = document.getElementById('sPDate').value;
//    var desc = document.getElementById('sDesc').value;
//    var vid = document.getElementById('vidUrl').value;

//    $.ajax({
//        url: '/Home/ShareYourStory',
//        type: "POST",
//        data: {

//            storyTitle: storyTitle,
//            pubDate: pubDate,
//            desc: desc,
//            vid: vid,
//            appliedMission: appliedMission

//        },
//        success: function () {
//            document.getElementById("sTitle").innerHTML = response.sTitle;
//        },
//        error: function (xhr, textStatus, errorThrown) {
//            console.log("Error: " + errorThrown);
//        }
//    });


//}








//function newselecId(s) {
    
//    const selectElement = document.getElementById("exampleFormControlSelect12");
//    const selectedOptionId = selectElement.selectedOptions[0].getAttribute("id");
//    console.log(selectedOptionId);

//    $.ajax({
//        url: '/Home/EditStory',
//        type: "POST",
//        data: {

//            selectedOptionId: selectedOptionId

//        },
//        success: function (data) {
//            var a = new Date(data.publishedAt).toISOString().slice(0, 10);

//            document.getElementById("sTitle").value = data.title;
//            document.getElementById("sPDate").value = a;
//            document.getElementById("sDesc").innerHTML = data.description;
//            document.getElementById("vidUrl").value = data.vidUrl;
//            console.log(data);
//        },
//        error: function (xhr, textStatus, errorThrown) {
//            console.log("Error: " + errorThrown);
//        }
//    });

//}


//function editstory() {
    
//    const selectElement = document.getElementById("exampleFormControlSelect12");
//    const selectedOptionId = selectElement.selectedOptions[0].getAttribute("id");



//    $.ajax({
//        url: '/Home/ShareYourStory',
//        type: "POST",
//        data: {
//            selectedOptionId: selectedOptionId,


//        },
//        success: function () {
//            document.getElementById("sTitle").innerHTML = response.sTitle;
//        },
//        error: function (xhr, textStatus, errorThrown) {
//            console.log("Error: " + errorThrown);
//        }
//    });

//}


//function addstory() {
//    const selectElement = document.getElementById("exampleFormControlSelect12");
//    const missiondd = selectElement.selectedOptions[0].getAttribute("id");

    
//        document.getElementById('submitbtns').disabled = false;
//        document.getElementById('submitbtns').style.borderColor = "#F88634";
//        document.getElementById('submitbtns').style.color = "#F88634";

//        var abcd = $('#exampleFormControlSelect12').val();
//        var storyTitle = document.getElementById('sTitle').value;
//        var pubDate = document.getElementById('sPDate').value;
//        var desc = document.getElementById('sDesc').value;
//        var videourl = document.getElementById('vidUrl').value;
//        $.ajax({
//            url: '/Home/ShareYourStory',
//            type: "POST",
//            data: {
//                missiondd: missiondd,
//                storyTitle: storyTitle,
//                pubDate: pubDate,
//                desc: desc,
//                abcd: abcd,
//                videourl: videourl,
//                /*newArray: newArray*/

//            },
//            success: function () {
//                console.log(1);
//                /* document.getElementById("sTitle").innerHTML = response.sTitle;*/
//            },
//            error: function (xhr, textStatus, errorThrown) {
//                console.log("Error: " + errorThrown);
//            }
//        });
    

//}

function submitstate() {
    document.getElementById('submitbtns').disabled = true;
    document.getElementById('submitbtns').style.borderColor = "grey";
    document.getElementById('submitbtns').style.color = "grey";
}

//function editstory() {



//    var appliedMission = $('#exampleFormControlSelect12').val();
//    var storyTitle = document.getElementById('sTitle').value;
//    var pubDate = document.getElementById('sPDate').value;
//    var desc = document.getElementById('sDesc').value;
//    var vid = document.getElementById('vidUrl').value;

//    $.ajax({
//        url: '/Home/SaveStory',
//        type: "POST",
//        data: {

//            storyTitle: storyTitle,
//            pubDate: pubDate,
//            desc: desc,
//            vid: vid,
//            appliedMission: appliedMission,


//        },
//        success: function () {
//            document.getElementById("sTitle").innerHTML = response.sTitle;
//        },
//        error: function (xhr, textStatus, errorThrown) {
//            console.log("Error: " + errorThrown);
//        }
//    });

//}



function applyformission(missionidforapply) {

    $.ajax({

        url: '/Home/VolunteeringMission',
        type: 'POST',
        data: { missionidforapply: missionidforapply },
        success: function () {
            console.log("success");

            location.reload();
            location.reload();
        }


    });
}



function onCancel() {

    document.getElementById('exampleFormControlSelect12').value = "";
    document.getElementById('sTitle').value = "";
    document.getElementById('sPDate').value = "";
    document.getElementById('sDesc').value = "";
    document.getElementById('vidUrl').value = "";


}




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

var availability = "";

function getavailability() {

    availability = document.getElementById('selectavailability').value;
}




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

var a = skillids[0];
console.log(a);
console.log(a);
console.log(a);

function editUser() {

    //event.preventDefault();
    
    var firstname = document.getElementById('FirstName').value;
    var lastname = document.getElementById('lname').value;
    var empId = document.getElementById('empid').value;
    var title = document.getElementById('title').value;
    var dept = document.getElementById('dept').value;
    var profile = document.getElementById('profiletxt').value;
    var whyI = document.getElementById('why').value;
    var linkedInurl = document.getElementById('link').value;
    var availability = document.getElementById('selectavailability').value;

    $.ajax({
        url: '/Home/SaveUserData',
        type: "POST",
        data: {

            firstname: firstname,
            lastname: lastname,
            empId: empId,
            title: title,
            dept: dept,
            profile: profile,
            whyI: whyI,   
            linkedInurl: linkedInurl,
            countryid: countryid,
            cityname: cityname,
            availability: availability,
            skillids: skillids

        },
        success: function () {
            //document.getElementById("FirstName").innerHTML = response.FirstName;
            /*document.querySelector("#usersaveddata").addEventListener('click', function () {*/
                //Swal.fire("Congratulations", "Your data has been saved successfully", "success");
            /*});*/
            
        },
        error: function (xhr, textStatus, errorThrown) {
            console.log("Error: " + errorThrown);
            //Swal.fire({
            //    icon: 'error',
            //    title: 'Oops...',
            //    text: 'Something went wrong!',
            //    footer: '<a href="">Why do I have this issue?</a>'
            //})
        }
    });

  
}





function changeuserpass() {

    var newpass = $("#NewPassword").val();

    $.ajax({

        url: '/Home/ChangeUserPass',
        type: 'POST',
        data: { newpass: newpass },
        success: function () {
            
            Swal.fire("Your password has been changed successfully", "Please Login!", "success");
            
        },
        error: function (xhr, textStatus, errorThrown) {
            console.log("Error: " + errorThrown);
            Swal.fire({
                icon: 'error',
                title: 'Oops...',
                text: 'Something went wrong!',
                
            })
        }


    });
 

}

// CONTACT US

function send() {

    var subject = document.getElementById('Subject').value;
    var msg = document.getElementById('Message').value;


    $.ajax({
        url: '/Home/EditProfile',
        type: "POST",
        data: {

            subject: subject,
            msg: msg,
          
        },
        success: function () {
            console.log("Sent");
        },
        error: function (xhr, textStatus, errorThrown) {
            console.log("Error: " + errorThrown);
        }
    });

}

// vol timesheet

function timemissionId(e) {

    const selectElement = document.getElementById("Mission");
    const selectedOptionId = selectElement.selectedOptions[0].getAttribute("id");



    $.ajax({
        url: '/Home/VolTimesheet',
        type: "POST",
        data: {

            selectedOptionId: selectedOptionId,
           
        },
        success: function () {
            console.log("success");
        },
        error: function (xhr, textStatus, errorThrown) {
            console.log("Error: " + errorThrown);
        }
    });

}

function submitTimedata() {

    const selectElement = document.getElementById("Mission");
    const selectedOptionId = selectElement.selectedOptions[0].getAttribute("id");

    var date = document.getElementById('Date').value;
    var hours = document.getElementById('Hours').value;
    var mins = document.getElementById('Minutes').value;
    var msg = document.getElementById('Message').value;

    $.ajax({
        url: '/Home/VolTimesheet',
        type: "POST",
        data: {

            selectedOptionId: selectedOptionId,
            date: date,
            hours: hours,
            mins: mins,
            msg: msg

        },
        success: function () {
            console.log("success");
        },
        error: function (xhr, textStatus, errorThrown) {
            console.log("Error: " + errorThrown);
        }
    });
}



function submitGoaldata() {

    const selectElement = document.getElementById("goalmissionlist");
    const selectedOptionId = selectElement.selectedOptions[0].getAttribute("id");

    var action = document.getElementById('Action').value;
    var datevol = document.getElementById('DateVol').value;
    var goalMsg = document.getElementById('GoalMessage').value;

    $.ajax({
        url: '/Home/VolTimesheet',
        type: "POST",
        data: {

            selectedOptionId: selectedOptionId,
            action: action,
            datevol: datevol,
            goalMsg: goalMsg

        },
        success: function () {
            console.log("success");
        },
        error: function (xhr, textStatus, errorThrown) {
            console.log("Error: " + errorThrown);
        }
    });
}


// Vol T

function editVoltime(e)
{
    const selectElement = document.getElementById("Missionedit");
    const selectedOptionId = selectElement.selectedOptions[0].getAttribute("id");



    $.ajax({
        url: '/Home/EditVolTime',
        type: "POST",
        data: {
            selectedOptionId: selectedOptionId
            

        },
        success: function (data) {

            //var a = new Date(data.date).ToISOString().slice(0, 10);

            document.getElementById("DateEdit").value = data.date;
            //document.getElementById("HoursEdit").value = data.hours;
            document.getElementById("MessageEdit").value = data.notes;
            console.log(data);

        },
        error: function (xhr, textStatus, errorThrown) {
            console.log("Error: " + errorThrown);
            
        }
    });
}


function saveVolTime()
{
    const selectElement = document.getElementById("Missionedit");
    const selectedOptionId = selectElement.selectedOptions[0].getAttribute("id");

    var date1 = document.getElementById('DateEdit').value;
    var hours1 = document.getElementById('HoursEdit').value;
    var mins1 = document.getElementById('MinutesEdit').value;
    var msg1 = document.getElementById('MessageEdit').value;

    $.ajax({
        url: '/Home/SaveVolTimeData',
        type: "POST",
        data: {

            selectedOptionId: selectedOptionId,
            date1: date1,
            hours1: hours1,
            mins1: mins1,
            msg1: msg1

        },
        success: function () {
            console.log("success");
        },
        error: function (xhr, textStatus, errorThrown) {
            console.log("Error: " + errorThrown);
        }
    });
}


// for goal based


function editVolGoal(e) {
    const selectElement = document.getElementById("MissioneditGoal");
    const selectedOptionId = selectElement.selectedOptions[0].getAttribute("id");



    $.ajax({
        url: '/Home/EditVolGoal',
        type: "POST",
        data: {
            selectedOptionId: selectedOptionId


        },
        success: function (data) {

            //var a = new Date(data.date).ToISOString().slice(0, 10);

            document.getElementById("ActionGoalEdit").value = data.action;
            document.getElementById("DateVolGoalEdit").value = data.datevol;
            document.getElementById("GoalMessageEdit").value = data.notes;
            console.log(data);

        },
        error: function (xhr, textStatus, errorThrown) {
            console.log("Error: " + errorThrown);

        }
    });
}


function saveVolGoal() {
    const selectElement = document.getElementById("MissioneditGoal");
    const selectedOptionId = selectElement.selectedOptions[0].getAttribute("id");

    var goaldate = document.getElementById('DateVolGoalEdit').value;
    var action1 = document.getElementById('ActionGoalEdit').value;
    var goalmsg1 = document.getElementById('GoalMessageEdit').value;

    $.ajax({
        url: '/Home/SaveVolGoalData',
        type: "POST",
        data: {

            selectedOptionId: selectedOptionId,
            goaldate: goaldate,
            action1: action1,
            goalmsg1: goalmsg1
            

        },
        success: function () {
            console.log("success");
        },
        error: function (xhr, textStatus, errorThrown) {
            console.log("Error: " + errorThrown);
        }
    });
}

// remove data vol t

function onremove(missionId) {

    $.ajax({
        url: '/Home/VolTimesheet',
        type: "POST",
        data: {
            missionId: missionId


        },
        success: function (data) {

            console.log("Data Deleted");

        },
        error: function (xhr, textStatus, errorThrown) {
            console.log("Error: " + errorThrown);

        }
    });

}

function onremovegoal(missionId) {

    $.ajax({
        url: '/Home/VolTimesheet',
        type: "POST",
        data: {
            missionId: missionId


        },
        success: function (data) {

            console.log("Data Deleted");

        },
        error: function (xhr, textStatus, errorThrown) {
            console.log("Error: " + errorThrown);

        }
    });

}

//upload img



function explorecall() {
    const selectElement = document.getElementById("exploreselect");
    const selectedOptionId = selectElement.selectedOptions[0].getAttribute("id");
    var selectoption = 0;
    if (selectedOptionId == "topthemes") {
        selectoption = 1;
    }
    else if (selectedOptionId == "mostranked") {
        selectoption = 2;
    }
    else if (selectedOptionId == "topfavmission") {
        selectoption = 3;
    }
    else if (selectedOptionId == "random") {
        selectoption = 4;
    }

    $.ajax({
       
        url: '/Home/_Grid',
        type: "POST",
        data: {
            selectoption: selectoption
 
        },
        success: function (data) {
            $('#filter_products-grid').html(data);
            console.log("Data Deleted");

        },
        error: function (xhr, textStatus, errorThrown) {
            console.log("Error: " + errorThrown);

        }
    });
}

//notification - read unread

//function readUnread() {

//    var dott = document.getElementById("dot");
//    var tickk = document.getElementById("tick");

//    if (dott.style.display == "block" && tickk.style.display == "none") {

//        dott.style.display = "none";
//        tickk.style.display = "block";
//    }
//    else {

//        dott.style.display = "block";
//        tickk.style.display = "none";
//    }
   
//}



$(document).ready(function () {
    $('#notification-link').click(function (event) {
        event.preventDefault();
        $('#dot').hide();
        $('#tick').show();
    });
});


$(document).ready(function () {
    $('#notification-link-approve').click(function (event) {
        event.preventDefault();
        $('#dot-a').hide();
        $('#tick-a').show();
    });
});