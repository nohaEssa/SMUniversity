$(window).load(function () {

    $('#HallIDs').multipleSelect({
        placeholder: "اختر القاعه",
        filter: true,
        width: '100%'
    });
});

function saveScreen() {
    $.ajax({
        type: "POST",
        url: "/Screen/Create",
        cache: false,
        contentType: "application/json; charset=utf-8",
        data: "{SubjectID:'" + $('#SubjectID').val() +
            "'}",
        dataType: "json",
        processData: false,
        success: function (response) {
            if (response !== "ERROR") {
                $('#LecturerID').html(response);
            }
        },
        failure: function (errMsg) {
            alert(errMsg);
        }
    });
}