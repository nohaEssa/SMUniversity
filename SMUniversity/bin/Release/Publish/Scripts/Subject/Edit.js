$(window).load(function () {
    $("#GeneralSubject").val() == "1" ? $("#txtGeneralNonSession").parent().addClass('checked') : $("#txtGeneralNonSession").parent().removeClass('checked');
    $("#GeneralSubject").val() == "0" ? $("#Female").parent().addClass('checked') : $("#Female").parent().removeClass('checked');
});


$(document).ready(function () {
    $('input[type=radio][name=GeneralSubject]').on('ifChecked', function (event) {
        debugger
        //alert(event.type + ' callback');
        $('input[type=radio][name=GeneralSubject]').val() == "false" ? $('input[type=radio][name=GeneralSubject]').val("true") : $('input[type=radio][name=GeneralSubject]').val("false");
        if ($('input[type=radio][name=GeneralSubject]').val() == "false") {
            $('.divMajor').show();
        }
        else if ($('input[type=radio][name=GeneralSubject]').val() == "true") {
            $('.divMajor').hide();
        }
    });
});