function getSubjects(SessionType) {
    debugger;
    $.ajax({
        type: "POST",
        url: "/Subject/getSubjects",
        cache: false,
        contentType: "application/json; charset=utf-8",
        data: "{MajorID:'" + $('#MajorID').val() +
            "', SubjectID:'" + $("#hdn_SubjectID").val() +
            "', SessionType:'" + SessionType +
            "'}",
        dataType: "json",
        processData: false,
        success: function (response) {
            if (response !== "ERROR") {
                $('#SubjectID').html(response);
                if ($("#hdn_SubjectID").val() != null) {
                    debugger
                    getLecturers();
                    //$("#SubjectID").val($("#hdn_SubjectID").val());
                }
            }
        },
        failure: function (errMsg) {
            alert(errMsg);
        }
    });
}

function getLecturers() {
    $.ajax({
        type: "POST",
        url: "/Lecturer/getLecturers",
        cache: false,
        contentType: "application/json; charset=utf-8",
        data: "{SubjectID:'" + $('#SubjectID').val() +
            "', LecturerID:'" + $("#hdn_LecturerID").val() +
            "'}",
        dataType: "json",
        processData: false,
        success: function (response) {
            if (response !== "ERROR") {
                $('#LecturerID').html(response);
                if ($("#hdn_LecturerID").val() != null) {
                    debugger
                    $("#LecturerID").val($("#hdn_LecturerID").val());
                }
            }
        },
        failure: function (errMsg) {
            alert(errMsg);
        }
    });
}

function getHalls() {
    $.ajax({
        type: "POST",
        url: "/Session/GetHalls",
        cache: false,
        contentType: "application/json; charset=utf-8",
        data: "{FromTime:'" + $('#FromDate').val() +
            "', ToTime:'" + $('#ToDate').val() +
            "', SessionID:'" + $('#hdn_SessionID').val() +
            "'}",
        dataType: "json",
        processData: false,
        success: function (response) {
            debugger
            if (response != "ERROR") {
                $('#HallID').html(response);
            }
        },
        failure: function (errMsg) {
            alert(errMsg);
        }
    });
}

function addtime() {
    var x = '<br /><div class="col-md-10">' +
        '<div class="ls-button-group border-bottom">' +
        '<div class="row">' +
        '<div class="col-md-4" id="Myfirst_clok">' +
        '<h5>من تاريخ</h5>' +
        //'<input id="cl_first1" class="form-control clockface_1 dateTimePickerCustom1" type="text" name="CourseFromTime[]" required /><br />' +
        '<input id="cl_first1" class="form-control clockface_1 dateTimePickerCustom1" type="text" name="CourseFromTime" required /><br />' +
        '</div>' +
        '<div class="col-md-4" id="Mysecond_clok">' +
        '<h5>الي تاريخ</h5>' +
        //'<input id="cl_second1" class="form-control clockface_1 dateTimePickerCustom1" type="text" name="CourseToTime[]" required /><br />' +
        '<input id="cl_second1" class="form-control clockface_1 dateTimePickerCustom1" type="text" name="CourseToTime" required /><br />' +
        '</div>' +
        ' <div class="col-md-4" id="Myremve">'+
        ' <button type="button" class="btn btn red" onclick="Remove_Row(1)" id="rem1" >حذف <i class="fa fa-times"></i></button><input type="hidden" id="hid1" value="1"  /></div><br />'+
        '</div> '+
        '</div>' +
        '</div >' +
        '</div >';

    var count = document.getElementById("count").value;
    if (count == 0) {
        var data = document.getElementById("Test").innerHTML;

        $("#Test").append(x);

        // document.getElementById("Test").innerHTML = data + x;
        document.getElementById("count").value = parseInt(document.getElementById("count").value) + 2;

    }
    else {

        var clok1 = document.getElementById("Myfirst_clok").innerHTML;

        //$("#Myfirst_clok").append('<input type="text" value="" data-format="hh:mm A" class="form-control clockface_1" id="cl_first' + document.getElementById("count").value + '" /><br />');
        //$("#Myfirst_clok").append('<input class="form-control clockface_1 dateTimePickerCustom1" type="text" id="cl_first' + document.getElementById("count").value + '" name="CourseFromTime[]" required /><br />');
        $("#Myfirst_clok").append('<input class="form-control clockface_1 dateTimePickerCustom1" type="text" id="cl_first' + document.getElementById("count").value + '" name="CourseFromTime" required /><br />');
        

        var clok2 = document.getElementById("Mysecond_clok").innerHTML;

        //$("#Mysecond_clok").append('<input type="text" value="" data-format="hh:mm A" class="form-control clockface_1" id="cl_second' + document.getElementById("count").value + '" /><br />')
        //$("#Mysecond_clok").append('<input class="form-control clockface_1 dateTimePickerCustom1" type="text" id="cl_second' + document.getElementById("count").value + '" name="CourseToTime[]" required /><br />');
        $("#Mysecond_clok").append('<input class="form-control clockface_1 dateTimePickerCustom1" type="text" id="cl_second' + document.getElementById("count").value + '" name="CourseToTime" required /><br />');

        var Remmoving = document.getElementById("Myremve").innerHTML;

        $("#Myremve").append('<div id="c"' + document.getElementById("count").value + '"><br /> </div> <button type="button" class="btn btn red" onclick="Remove_Row(' + document.getElementById("count").value + ')" id="rem' + document.getElementById("count").value + '" >حذف <i class="fa fa-times"></i></button>'
            + '<input type="hidden" id="hid' + document.getElementById("count").value + '"  value="1"  />');

        document.getElementById("count").value = parseInt(document.getElementById("count").value) + 1;
    }
    $('.dateTimePickerCustom1').datetimepicker();
}

function Remove_Row(idd) {

    var count = document.getElementById("count").value;
    //$("#week" + idd).remove();
    $("#cl_first" + idd).remove();
    $("#cl_second" + idd).remove();
    $("#rem" + idd).remove();
    $("#c" + idd).remove();
    document.getElementById("hid" + idd).value = 0;
}

//$('#Type').change(function () {
//    debugger
//    $('#Type').val() == "2" ? $('#Type').show() : $('#Type').hide();
//});

function toogle() {
    debugger
    $('#Type').val() == "2" ? $('.CoursePaymentMethod').show() : $('.CoursePaymentMethod').hide();
    $('#Type').val() == "2" ? $('.SessionPeriod').show() : $('.SessionPeriod').hide();
    $('#Type').val() == "2" ? $('.CourseCost').show() : $('.CourseCost').hide();
    $('#Type').val() == "1" ? $('.SessionPrice').show() : $('.SessionPrice').hide();
    $('#Type').val() == "1" ? $('.SessionPaymentMethod').show() : $('.SessionPaymentMethod').hide();
}

$(document).ready(function () {
    
    $(".sessionDate").focusout(function () {
        getHalls();
    });

    $('input[type=radio][name=GeneralSession]').on('ifChecked', function (event) {
        debugger
        //alert(event.type + ' callback');
        $('input[type=radio][name=GeneralSession]').val() == "false" ? $('input[type=radio][name=GeneralSession]').val("true") : $('input[type=radio][name=GeneralSession]').val("false");
        if ($('input[type=radio][name=GeneralSession]').val() == "false") {
            $('.generalSessionDiv').show();
            $('.subjectDiv').show(); 
            $('.LblSubjectName').hide();
            $('#SubjectID').find('option').not(':first').remove();
            $('.divSelectSubject').removeClass('col-md-10');
            $('.divSelectSubject').addClass('col-md-2 col-sm-12');
        }
        else if ($('input[type=radio][name=GeneralSession]').val() == "true") {
            $('.generalSessionDiv').hide();
            $('.LblSubjectName').show();
            $('.divSelectSubject').removeClass('col-md-2 col-sm-12');
            $('.divSelectSubject').addClass('col-md-10');
            getSubjects(1);
        }
    });
});