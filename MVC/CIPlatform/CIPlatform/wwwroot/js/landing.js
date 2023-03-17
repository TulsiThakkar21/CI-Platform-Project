function likeMission(missionId, id) {

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


