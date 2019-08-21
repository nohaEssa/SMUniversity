$(window).load(function () {
    var nodes = document.getElementById("EditSession").getElementsByTagName('*');
    for (var i = 0; i < nodes.length; i++) {
        if ($('#hideEdit').val() == '1') {
            nodes[i].disabled = true;
        } else {
            nodes[i].disabled = false;
        }
    }

});