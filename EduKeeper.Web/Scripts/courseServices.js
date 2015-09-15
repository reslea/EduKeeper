
function getDate(jsonDateString){
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
    if(date.getMinutes() < 10)
        minutes = "0" + date.getMinutes();
    else minutes = date.getMinutes();

    return date.getDay() + " " + month[date.getMonth()] + " at " + date.getHours() + ":" + minutes;
}

function createInputId(id){
    return 'text'+id;
}

function postMessage(){
    var options ={
        url: '@Url.Action("PostMessage")',
        type: "post",
        data: {
            message: $("#postMessage").val(),
            courseId: $("#courseId").val()
        }
    }

    jQuery.ajax(options).done(function (data) {
        $("#postsTemplate").tmpl(data).prependTo("#posts");
        $("#postMessage").val("");

    });
}

function postComment(id){
    var options ={
        url: '@Url.Action("PostComment")',
        type: "post",
        data: {
            message: $("#text" + id).val(),
            PostId: id
        }
    }

    jQuery.ajax(options).done(function (data) {
        $("#commentTemplate").tmpl(data).prependTo("#" + id);
        $("#" + id).val("");

    });
}