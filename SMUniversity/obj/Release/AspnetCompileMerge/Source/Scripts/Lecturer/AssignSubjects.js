$(window).load(function () {
    $('#LecturerIDs').multipleSelect({
        placeholder: "اختر محاضرين",
        filter: true,
        width: '100%'
    });
});

function getMajors() {
    $.ajax({
        type: "POST",
        url: "/Major/getMajors",
        cache: false,
        contentType: "application/json; charset=utf-8",
        data: "{CollegeID:'" + $('#CollegeID').val() +
            "'}",
        dataType: "json",
        processData: false,
        success: function (response) {
            if (response !== "ERROR") {
                $('#MajorID').html(response);
            }
        },
        failure: function (errMsg) {
            alert(errMsg);
        }
    });
}

function getSubjects() {
    $.ajax({
        type: "POST",
        url: "/Subject/getSubjects",
        cache: false,
        contentType: "application/json; charset=utf-8",
        data: "{MajorID:'" + $('#MajorID').val() +
            "'}",
        dataType: "json",
        processData: false,
        success: function (response) {
            if (response !== "ERROR") {
                $('#SubjectID').html(response);
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
        url: "/Lecturer/getLecturersBySubjectID",
        cache: false,
        contentType: "application/json; charset=utf-8",
        data: "{SubjectID:'" + $('#SubjectID').val() +
            "'}",
        dataType: "json",
        processData: false,
        success: function (response) {
            if (response !== "ERROR") {
                for (var i = 0; i < response.length; i++) {
                    $("#LecturerIDs").append(new Option(response[i].Name, response[i].LecturerID));
                }

                $('#LecturerIDs').multipleSelect({
                    placeholder: "اختر محاضرين",
                    filter: true,
                    width: '100%'
                });
            }
        },
        failure: function (errMsg) {
            alert(errMsg);
        }
    });
}