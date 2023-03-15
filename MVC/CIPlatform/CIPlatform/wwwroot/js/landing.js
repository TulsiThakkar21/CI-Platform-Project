
function addToFavorites(missionId, e) {
    const urlParams = new URLSearchParams(window.location.search);
    urlParams.set('missionId', missionId);

    const newUrl = window.location.pathname + '?' + urlParams.toString();
    history.pushState(null, '', newUrl);
    //alert("hello");
    $.ajax({
        url: '/Home/Add',
        type: 'POST',
        data: { missionId: missionId },
        success: function () {
            // Change button color to red
            $('.btn-primary').addClass('btn-danger').removeClass('btn-primary');
        }
    });
}



function removeFromFavorites(missionId) {

    const urlParams = new URLSearchParams(window.location.search);
    urlParams.set('missionId', missionId);

    const newUrl = window.location.pathname + '?' + urlParams.toString();
    history.pushState(null, '', newUrl);

    $.ajax({
        url: '/Home/Remove',
        type: 'POST',
        data: { missionId: missionId },
        success: function () {
            // Change button color to blue
            $('.btn-danger').addClass('btn-primary').removeClass('btn-danger');
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
                debugger
                rating.removeClass("far").addClass("fas");
                star.prevAll(".star").removeClass("far").addClass("fas");
                star.removeClass("far").addClass("fas");
            }
        });
    });
});

//function mecalls(stars,missionId) {
//    const urlParams = new URLSearchParams(window.location.search);
//    urlParams.set('stars', stars , 'missionId', missionId);

//    const newUrl = window.location.pathname + '?' + urlParams.toString();
//    history.pushState(null, '', newUrl);

//    $.ajax({
//        url: '/Home/Rate',
//        type: 'POST',
//        data: { stars: stars, missionId: missionId },

//    });
//}


//$(function () {
//    var rating = $("#rating");

//    rating.find(".star").click(function () {
//        var star = $(this);
//        var stars = star.data("value");
//        const urlParams = new URLSearchParams(window.location.search);
//        var missionid = $(this);
//        var missionId = missionid.data("value");



//        $.ajax({
//            type: "POST",
//            url: "/Home/Rate",
//            data: { stars: stars, missionId: missionId },
//            success: function () {
//                rating.find(".star").removeClass("fas").addClass("far");
//                star.prevAll(".star").addBack().removeClass("far").addClass("fas");
//                missionid.find(".missionid");
//            }
//        });
//    });
//});


