$(window).load(function () {
    //$("#Male").val() == "true" ? $("#Male").parent().addClass('checked') : $("#Male").parent().removeClass('checked');
    $("#Gender").val() == "1" ? $("#Male").parent().addClass('checked') : $("#Male").parent().removeClass('checked');
    $("#Gender").val() == "0" ? $("#Female").parent().addClass('checked') : $("#Female").parent().removeClass('checked');

    $(".datePickerOnly").val($("#DOB").val());
});