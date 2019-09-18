$(window).load(function () {

    $('#LecturerIDs').multipleSelect({
        placeholder: "اختر محاضرين",
        filter: true,
        width: '100%'
    });
});

$(document).ready(function () {
    $('input[type=radio][name=GeneralSubject]').on('ifChecked', function (event) {
        debugger
        //alert(event.type + ' callback');
        $('input[type=radio][name=GeneralSubject]').val() == "false" ? $('input[type=radio][name=GeneralSubject]').val("true") : $('input[type=radio][name=GeneralSubject]').val("false");
        if ($('input[type=radio][name=GeneralSubject]').val() == "false") {
            //$('.divMajor').show();
            $('.divEduData').show();
        }
        else if ($('input[type=radio][name=GeneralSubject]').val() == "true") {
            //$('.divMajor').hide();
            $('.divEduData').hide();
        }
    });
});