
function SearchTransaction() {
    document.getElementById("DivSubSription").style.display = 'block';

    $.ajax({
        type: "POST",
        url: "/Reports/GetTrancactionByDate",
        cache: false,
        contentType: "application/json; charset=utf-8",
        data: "{CreatedDate:'" + document.getElementById("date_timepicker_start").value + "'}",
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
                datastring += "<tr> <td> " + (i++) + " </td> <td> " + value.StudentName + "</a> </td> <td>  " + value.TitleAr + "</td> <td>" + value.Price + "</td> <td>" + value.PaymentMethod + "</td>   </tr>";
                sumPrice += value.Price
            });

           // datastring += "<tr> <td> " + "إجمالى الشحن اليومى" + " </td>   <td> " + sumPrice + "</td>   </tr>";

            if (i > 1) {

                $(datastring).appendTo($("#myDataTable"));


                var DetailsDate = '<td width="200"><b>تاريخ التقرير:</b></td>';
                DetailsDate += '<td width="200" colspan="3">  <strong></strong>  ';
                if (document.getElementById("date_timepicker_start").value != "")  
                    DetailsDate += document.getElementById("date_timepicker_start").value;
                else
                    DetailsDate += new Date().toJSON().slice(0,10).replace(/-/g,'/')
                DetailsDate += '</td>';
                $(DetailsDate).appendTo($("#DetailsDate"));

                var DetailsTotalPrice = '<td><b>إجمالى الشحن اليومى :</b></td>';
                DetailsTotalPrice += '<td colspan="3">' + sumPrice + ' د.ك</td>'
                $(DetailsTotalPrice).appendTo($("#DetailsTotalPrice"));

                var DetailsTotalSub = '<td><b>إجمالى عدد الشحن:</b></td>';
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