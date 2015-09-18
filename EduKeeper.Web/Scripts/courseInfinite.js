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
        if (data.IsHasMore == false)
            isHasMorePosts = false;

        $("#postsTemplate").tmpl(data.Posts).appendTo("#posts");
        pageNumber++;
        isLoadingPosts = true;
        $("#load").hide();

        data.Posts.forEach(function (element) {
            if(element.IsHasMore)
                comments[element.Id] = 2; //next page
        });
    });

    return false;

};

function getComments(commentSectionId) {
    if (!isLoadingComments || !isHasMoreComments)
        return;

    isLoadingComments = false;

    if (isLoadingComments) {
        //hide button show load ??
    }

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

        $("#commentTemplate").tmpl(data.comments).appendTo('#' + commentSectionId);
        isLoadingComments = true;
        $("#load").hide();

        if (data.HasNextPage) {
            comments[commentSectionId]++;
        }
        else {
            delete comments[commentSectionId];
            $(this).remove();
        }
    });

    return false;

};