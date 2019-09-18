function getColleges() {
    $.ajax({
        type: "POST",
        url: "/University/getColleges",
        cache: false,
        contentType: "application/json; charset=utf-8",
        data: "{UniversityID:'" + $('#UniversityID').val() +
            "', CollegeID:'" + $("#hdn_CollegeID").val() +
            "'}",
        dataType: "json",
        processData: false,
        success: function (response) {
            if (response !== "ERROR") {
                $('#CollegeID').html(response);
                if ($("#hdn_CollegeID").val() != null) {
                    debugger
                    getMajors();
                    //$("#CollegeID").val($("#hdn_CollegeID").val());
                }
            }
        },
        failure: function (errMsg) {
            alert(errMsg);
        }
    });
}

function getMajors() {
    $.ajax({
        type: "POST",
        url: "/Major/getMajors",
        cache: false,
        contentType: "application/json; charset=utf-8",
        data: "{CollegeID:'" + $('#CollegeID').val() +
            "', MajorID:'" + $("#hdn_MajorID").val() +
            "'}",
        dataType: "json",
        processData: false,
        success: function (response) {
            if (response !== "ERROR") {
                $('#MajorID').html(response);
                if ($("#hdn_MajorID").val() != null) {
                    debugger
                    getSubjects();
                    //$("#MajorID").val($("#hdn_MajorID").val());
                }
            }
        },
        failure: function (errMsg) {
            alert(errMsg);
        }
    });
}

function getAreas() {
    $.ajax({
        type: "POST",
        url: "/Area/getAreas",
        cache: false,
        contentType: "application/json; charset=utf-8",
        data: "{GovernorateID:'" + $('#GovernorateID').val() +
            "'}",
        dataType: "json",
        processData: false,
        success: function (response) {
            console.log(response);
            if (response !== "ERROR") {
                $('#AreaID').html(response);
            }
        },
        failure: function (errMsg) {
            alert(errMsg);
        }
    });
}

function showNewValDiv() {
    $('#CardCategoryID').val() == "-1" ? $('.newChargeValueDiv').show() : $('.newChargeValueDiv').hide();
}

$(document).ready(function () {
    $('input[type=radio][name=StudentType]').on('ifChecked', function (event) {
        debugger
        //alert(event.type + ' callback');
        $('input[type=radio][name=StudentType]').val() == "false" ? $('input[type=radio][name=StudentType]').val("true") : $('input[type=radio][name=StudentType]').val("false");
        if ($('input[type=radio][name=StudentType]').val() == "false") {
            $('.divEduData').show();
        }
        else if ($('input[type=radio][name=StudentType]').val() == "true") {
            $('.divEduData').hide();
        }
    });    
});