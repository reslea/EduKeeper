$(function () {


    $('#search').autocomplete(
            {
                source: '/Study/AutocompleteCourse',
                select: function (event, ui) {
                    $("#search").val(ui.item.label);
                    $("#form0").submit();
                }
            });

    var getPage = function () {
        var $a = $(this);

        var options = {
            url: $a.attr("href"),
            data: $("#search").serialize(),
            type: "get"
        };

        jQuery.ajax(options).done(function (data) {
            var target = $a.parents("div.pagedList").attr("data-target");
            $(target).replaceWith(data);
        });

        return false;
    };

    $(".container").on("click", ".pagedList a", getPage);
});