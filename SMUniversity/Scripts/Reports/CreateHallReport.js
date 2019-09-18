
function SearchTransaction() {
    document.getElementById("DivSubSription").style.display = 'block';

    $.ajax({
        type: "POST",
        url: "/Reports/GetHallRentByDate",
        cache: false,
        contentType: "application/json; charset=utf-8",
        data: "{StartDate:'" + document.getElementById("date_timepicker_start").value + "',EndDate: '"+document.getElementById("date_timepicker_end").value+"'}",
        dataType: "json",
        async: false,
        success: function (response) {
            debugger;

            $("#myDataTable tbody").remove();
            $("#DetailsDate td").remove();
            $("#DetailsTotalPrice td").remove();
            $("#DetailsTotalSub td").remove();

            var datastring = "";
            var i = 1;
            var sumPrice = 0;
            $.each(response, function (index, value) {
                datastring += "<tr> <td> " + (i++) + " </td> <td> " + value.HallName + "</td> <td>" + value.lectureName + "</td> <td>" + value.PhoneNumber + "</td> <td>" + value.CreatedAt + "</td> <td>" + value.Price + "</td> </tr>";
                sumPrice += value.Price
            });
         //   datastring += "<tr> <td> " + "إجمالى التكلفه" + " </td>   <td> " + sumPrice + "</td>   </tr>";
            if (i > 1) {

                $(datastring).appendTo($("#myDataTable"));
                var DetailsDate = '<td width="200"><b>التاريخ:</b></td>';
                DetailsDate += '<td colspan="3">  <strong>من   : </strong>  ';
                DetailsDate += document.getElementById("date_timepicker_start").value;
                if (document.getElementById("date_timepicker_end").value) {
                    DetailsDate += '  <strong>إلى  : </strong>  ';
                    DetailsDate += document.getElementById("date_timepicker_end").value;
                }
                DetailsDate += '</td>';
                $(DetailsDate).appendTo($("#DetailsDate"));

                var DetailsTotalPrice = '<td><b>إجمالى تكلفه الحجز :</b></td>';
                DetailsTotalPrice += '<td colspan="3">' + sumPrice + ' د.ك</td>'
                $(DetailsTotalPrice).appendTo($("#DetailsTotalPrice"));

                var DetailsTotalSub = '<td><b>إجمالى عدد الحجوزات:</b></td>';
                DetailsTotalSub += '<td colspan="3">' + (i - 1) + '</td>';
                $(DetailsTotalSub).appendTo($("#DetailsTotalSub"));


                document.getElementById("DivSubSription").style.display = 'block';
                document.getElementById("printbtn").style.display = 'block';
                document.getElementById("logoDiv").style.display = 'block';
                document.getElementById("tblDetails").style.display = 'block';

            }
        },
        failure: function (errMsg) {

            ShowSucsses(errMsg, "2");
        }
    });
    //}
}

////////////////////////////////
$(window).load(function () {

    $('#AtReA5eran').addClass('active open');


    //var element = document.getElementById("FUhjohfjhu");
    //element.innerHTML = element.innerHTML + "<span class='selected'></span>";

    //$('#35klhdkafhukgs').addClass('active');

    //TableAdvanced.init();

    //$(".select2_category").select2();


});