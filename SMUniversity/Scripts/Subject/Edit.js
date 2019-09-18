$(window).load(function () {
    debugger
    $('#GeneralSubject').val() == "1" ? $("#txtGeneralSubject").parent().addClass('checked') : $("#txtNonGeneralSubject").parent().removeClass('checked');
    //$('input[type=radio][name=GeneralSubject]').val() == "false" ? $("#txtNonGeneralSubject").parent().addClass('checked') : $("#txtGeneralSubject").parent().removeClass('checked');
});