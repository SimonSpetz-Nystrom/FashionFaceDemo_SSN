$(function () {
    var maleCount = 0;
    var femaleCount = 0;
    var twitterHub = $.connection.twitterHub;
    twitterHub.client.addImage = function (url, displayUrl) {
        // Insert image and remove the last one.
        $(".twitter-image-container").first().before('<div class="twitter-image-container"><a href="http://' + displayUrl + '"><img src="' + url + '" /></a></div>');
        $(".twitter-image-container").last().remove();

        // Scale and center-crop image.
        var $container = $(".twitter-image-container").first();
        var containerSide = $container.width();

        var $img = $(".twitter-image-container img").first();
        var imgWidth = $img.width();
        var imgHeight = $img.height();

        if (imgWidth > imgHeight) {
            imgWidth = imgWidth * (containerSide / imgHeight);
            $img.height(containerSide);
            $img.css("left", -(imgWidth - containerSide) / 2);
        } else if (imgWidth < imgHeight) {
            imgHeight = imgHeight * (containerSide / imgWidth);
            $img.width(containerSide);
            $img.css("top", -(imgHeight - containerSide) / 2);
        } else {
            $img.width(containerSide);
        }
    }

    twitterHub.client.addMales = function (amount) {
        maleCount += amount;
        $(".malesPerHour").html("<b>Total males seen: </b>" + maleCount);
    }

    twitterHub.client.addFemales = function (amount) {
        femaleCount += amount;
        $(".femalesPerHour").html("<b>Total females seen: </b>" + femaleCount);
    }

    $.connection.hub.start().done(function () {
        // Code to run after connection establshed.
    });
});