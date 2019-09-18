function toogle() {
    debugger
    $('#LecturerID').val() == "-1" ? $('.LecturerData').show() : $('.LecturerData').hide();
}

$(document).ready(function () {

    $(".hallDate").focusout(function () {
        $.ajax({
            type: "POST",
            url: "/Hall/GetHalls",
            cache: false,
            contentType: "application/json; charset=utf-8",
            data: "{FromTime:'" + $('#FromDate').val() +
                "', ToTime:'" + $('#ToDate').val() +
                "', HallRentedID:'" + $('#hdn_HallRentedID').val() +
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

    });
});