$(function () {
    var trackingInput = $("#tracking-input");
    var twitterHub = $.connection.twitterHub;
    twitterHub.client.addImage = function (url) {
        $(".twitter-image").first().before("<img src=" + url + " class = twitter-image />");
        $(".twitter-image").last().remove();

        //$(".twitter-image").first().src(url);
    }

    $.connection.hub.start().done(function() {
        // Code to run after connection establshed.
    });
});