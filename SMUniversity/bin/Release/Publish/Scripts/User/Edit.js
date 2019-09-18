$(window).load(function () {
    debugger
    var UserPermissions = $('#txtUserPermissions').val();
    var res = UserPermissions.split(",");

    //$.each(res, function (key, value) {
    //    alert(key + ": " + value);
    //});

    for (j=0; j < res.length; j++) {
        //$(":checkbox[value=" + res[j] + "]").attr('checked', true);
        $(":checkbox[value=" + res[j] + "]").parent().addClass("checked");
    }
});