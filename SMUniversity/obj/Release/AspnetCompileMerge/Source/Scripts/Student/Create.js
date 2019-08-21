function getColleges() {
    $.ajax({
        type: "POST",
        url: "/University/getColleges",
        cache: false,
        contentType: "application/json; charset=utf-8",
        data: "{UniversityID:'" + $('#UniversityID').val() +
            "'}",
        dataType: "json",
        processData: false,
        success: function (response) {
            if (response !== "ERROR") {
                $('#CollegeID').html(response);
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