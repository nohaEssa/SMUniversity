var count = 0;
$(window).load(function () {

    $('#LecturerIDs').multipleSelect({
        placeholder: "اختر محاضرين",
        filter: true,
        width: '100%'
    });
});

$(document).ready(function () {
    $('input[type=radio][name=GeneralSubject]').on('ifChecked', function (event) {
        count++;
        debugger
        if ($("#SubjectID").val() == undefined) {
            $('input[type=radio][name=GeneralSubject]').val() == "false" ? $('input[type=radio][name=GeneralSubject]').val("true") : $('input[type=radio][name=GeneralSubject]').val("false");
        }
        if ($('input[type=radio][name=GeneralSubject]').val() == "false") {
            //$('.divMajor').show();
            $('.divEduData').show();
            if ($("#SubjectID").val() != undefined) {
                $('input[type=radio][name=GeneralSubject]').val("true");
            }
        }
        else if ($('input[type=radio][name=GeneralSubject]').val() == "true") {
            //$('.divMajor').hide();
            $('.divEduData').hide();
            if ($("#SubjectID").val() != undefined) {
                $('input[type=radio][name=GeneralSubject]').val("false");
            }
        }
    });

    $("#SubjectCode").focusout(function () {
        checkSubjectCodeAvailability();
    });

    $("#submit").click(function (e) {
        debugger;
        if ($('#lblSubjectCodeError').is(":hidden")) {
            $('#submit').prop('type', 'submit');
            $("#submit").submit();
        }
    });
});

function checkSubjectCodeAvailability() {
    $.ajax({
        type: "POST",
        url: "/Subject/CheckSubjectCodeAvailability",
        cache: false,
        contentType: "application/json; charset=utf-8",
        data: "{SubjectCode:'" + $('#SubjectCode').val() +
              "', SubjectID:'" + $("#SubjectID").val() +
              "'}",
        dataType: "json",
        processData: false,
        success: function (response) {
            debugger;
            if (response == "ERROR") {
                $('#lblSubjectCodeError').show();
            } else {
                $('#lblSubjectCodeError').hide();
            }
        },
        failure: function (errMsg) {
            alert(errMsg);
        }
    });
}