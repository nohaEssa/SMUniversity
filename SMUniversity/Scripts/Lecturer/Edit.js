$(window).load(function () {
    $("#Gender").val() == "true" ? $("#Male").parent().addClass('checked') : $("#Male").parent().removeClass('checked');
    $("#Gender").val() == "false" ? $("#Female").parent().addClass('checked') : $("#Female").parent().removeClass('checked');
});