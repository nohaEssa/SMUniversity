function showModal(ID) {
    debugger
    $('#HModalTitle').text('إنهاء المحاضره');
    $('#PModalBody').text('هل انت متأكد من إنهاء المحاضره ؟');
    $("#btnViewSessions_" + ID).attr("href", "#reModalRed");
    $('#SessionTimeID').val(ID);
    clickOnModal(ID);
    return false;
}

function clickOnModal(ID) {
    var href = $('#btnViewSessions_' + ID).attr('href');
    window.location.href = href; //causes the browser to refresh and load the requested url
    return false;
}


$(document).on('confirm', '.remodal', function () {
    debugger
    $.ajax({
        type: "POST",
        url: "/Session/EndSession",
        cache: false,
        contentType: "application/json; charset=utf-8",
        data: "{SessionTimeID:'" + $('#SessionTimeID').val() +
            "'}",
        dataType: "json",
        processData: false,
        success: function (response) {
            debugger
            location.reload();
        },
        failure: function (errMsg) {
            alert(errMsg);
        }
    });
});