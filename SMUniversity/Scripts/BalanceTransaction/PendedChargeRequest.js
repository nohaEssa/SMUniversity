function showModal(ID, Price, Name) {
    debugger
    $('#HModalTitle').text('تأكيد شحن كارت');
    $('#PModalBody').text('سيتم تأكيد شحن مبلغ ' + Price + ' د.ك الي الطالب ' + Name + '');
    $("#btnConfirmTransaction_" + ID).attr("href", "#reModalRed");
    $('#TransactionID').val(ID);
    //$("#submitModalRequest").attr("href", "/BalanceTransaction/ConfirmTransaction?TransactionID=" + ID);
    clickOnModal(ID);
    return false;
}

function clickOnModal(ID) {
    var href = $('#btnConfirmTransaction_' + ID).attr('href');
    window.location.href = href; //causes the browser to refresh and load the requested url
    return false;
}


$(document).on('confirm', '.remodal', function () {
    debugger
    $.ajax({
        type: "POST",
        url: "/BalanceTransaction/ConfirmTransaction",
        cache: false,
        contentType: "application/json; charset=utf-8",
        data: "{TransactionID:'" + $('#TransactionID').val() +
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

    //alert('hey there!!!!!');
    //debugger
    //// disable other close options while modal is open
    //remodalInst.settings = {
    //    closeOnCancel: false,
    //    closeOnEscape: false,
    //    closeOnOutsideClick: false
    //}
    //// show confirmation window
    //$('#confirmation-box').show();
    //// hide confirmation window after clicking "no" without hiding whole modal
    //$(document).on('confirmation', '.remodal', function () {
    //    $('#confirmation-box').hide();
    //});
});