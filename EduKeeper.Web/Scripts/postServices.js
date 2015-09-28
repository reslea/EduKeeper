var comments = {
};

var pageNumber = 1;
var isLoadingPosts = true;
var isLoadingComments = true;
var isHasMorePosts = true;
var isHasMoreComments = true;

$(window).on('scroll', function () {
    if ($(window).scrollTop() == $(document).height() - $(window).height()) {
        getPage();
    }
}).scroll();

function getPage() {
    if (!isLoadingPosts || !isHasMorePosts)
        return;

    isLoadingPosts = false;

    if (isHasMorePosts) {
        $("#load").show();
    }

    var options = {
        url: 'GetPosts',
        type: "get",
        data: {
            pageNumber: pageNumber,
            courseId: $("#courseId").val()
        }
    };

    jQuery.ajax(options).done(function (data) {
        $("#load").hide();

        if (data == null) {
            isLoadingPosts = true;
            return;
        }
        if (data.IsHasMore == false)
            isHasMorePosts = false;

        isLoadingPosts = true;
        $("#postsTemplate").tmpl(data.Posts).appendTo("#posts");
        pageNumber++;

        data.Posts.forEach(function (element) {
            if (element.IsHasMore)
                comments[element.Id] = 2; //next page
        });
    });

    return false;

};

function getComments(commentSectionId) {
    if (!isLoadingComments || !isHasMoreComments)
        return;

    isLoadingComments = false;

    $("#loading" + commentSectionId).show();

    var options = {
        url: 'GetComments',
        type: "get",
        data: {
            pageNumber: comments[commentSectionId],
            postId: commentSectionId
        }
    };

    jQuery.ajax(options).done(function (data) {
        if (data.HasNextPage == false)
            isHasMorePosts = false;

        var reversedComments = data.comments.reverse();

        $("#commentTemplate").tmpl(reversedComments).prependTo('#' + commentSectionId);
        isLoadingComments = true;
        $("#loading" + commentSectionId).hide();

        if (data.HasNextPage) {
            comments[commentSectionId]++;
        }
        else {
            delete comments[commentSectionId];
            $("#loadMore" + commentSectionId).hide();
        }
    });

    return false;

};

function getDate(jsonDateString) {
    var month = new Array();
    month[0] = "Jan";
    month[1] = "Feb";
    month[2] = "Mar";
    month[3] = "Apr";
    month[4] = "May";
    month[5] = "Jun";
    month[6] = "Jul";
    month[7] = "Aug";
    month[8] = "Sept";
    month[9] = "Oct";
    month[10] = "Nov";
    month[11] = "Dec";
    var date = new Date(parseInt(jsonDateString.replace('/Date(', '')));

    var minutes = "";
    if (date.getMinutes() < 10)
        minutes = "0" + date.getMinutes();
    else minutes = date.getMinutes();

    return date.getDay() + " " + month[date.getMonth()] + " at " + date.getHours() + ":" + minutes;
}

function createInputId(text, id) {
    return text + id;
}

function postMessage() {
    var courseId = $("#courseId").val();
    var files = $("#files" + courseId)[0].files;
    var data = new FormData();

    if (files.length > 0) {
        for (var i = 0; i < files.length; i++) {
            data.append(files[i].name, files[i]);
        }
    }

    data.append("message", $("#postMessage").val());
    data.append("courseId", courseId);

    var options = {
        url: '/Study/PostMessage',
        method: "post", processData: false, contentType: false,
        data: data
    }

    jQuery.ajax(options).done(function (data) {
        $("#postsTemplate").tmpl(data).prependTo("#posts");
        $("#postMessage").val("");

    });
}

function postComment(id) {
    var options = {
        url: '/Study/PostComment',
        type: "post",
        data: {
            message: $("#text" + id).val(),
            PostId: id
        }
    }

    jQuery.ajax(options).done(function (data) {
        $("#commentTemplate").tmpl(data).appendTo("#" + id);
        $("#text" + id).val("");
        $("#text" + id).focus();
    });
}