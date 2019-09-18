$('#CreateChargeCards').click(function () {
    $.ajax({
        type: "POST",
        url: '/CardCategory/CreateChargeCards',
        cache: false,
        contentType: "application/json; charset=utf-8",
        data: "{CardCatID:'" + $('#CardCatID').val() +
            "'}",
        dataType: "json",
        processData: false,
        success: function (response) {
            location.reload();
        },
        failure: function (errMsg) {
            alert(errMsg);
        }
    });
    return false;
});