
var pageNumber = 1;
var scroll = true;

$(window).on('scroll', function () {
    if ($(window).scrollTop() == $(document).height() - $(window).height()) {
        getPage();
    }
}).scroll();

function getPage() {

    scroll = false;

    $("#load").show();

    var options = {
        url: 'GetPosts',
        type: "get",
        data: {
            pageNumber: pageNumber,
            courseId: $("#courseId").val()
        }
    };

    jQuery.ajax(options).done(function (data) {
        $("#postsTemplate").tmpl(data).appendTo("#posts");
        pageNumber++;
        scroll = true;
        $("#load").hide();
    });

    return false;

};