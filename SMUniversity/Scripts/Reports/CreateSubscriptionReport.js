function SearchSubscription() {
                                                                                                 
    $.ajax({
        type: "POST",
        url: "/Reports/GetSubscriptionBySessionWithinDate",
        cache: false,
        contentType: "application/json; charset=utf-8",
        data: "{StartDate:'" + document.getElementById("date_timepicker_start").value + "',EndDate: '" + document.getElementById("date_timepicker_end").value + "',SessionID:'" + $('#SessionID').val() + "'}",
        dataType: "json",
        async: false,
        success: function (response) {
            debugger;

            $("#myDataTable tbody").remove();
            $("#UniversityView p").remove();
            $("#DetailsDate td").remove();
            $("#DetailsTotalPrice td").remove();
            $("#DetailsTotalSub td").remove();

            var datastring = "";
            var i = 1;
            var sumPrice = 0;
            $.each(response.Subscriptions, function (index, value) {
                datastring += "<tr> <td> " + (i++) + " </td> <td> " + value.StudentName + "</td> <td>  " + value.Price + "</td> <td>" + value.PaymentMethod + "</td> <td>" + value.CreatedAt + "</td>   </tr>";
                sumPrice += value.Price
            });

            //$.each(response.pricesDetails, function (index, value) {
            //    if (index % 2 != 1) {
            //        datastring += "<tr> <td> </td> <td> " + value + " </td> ";

            //    } else {
            //        datastring += "<td> " + value + "</td>   </tr>";
            //    }

            //});
            //  datastring += "<tr><td> </td> <td> " + "إجمالى الشحن اليومى" + " </td>   <td> " + sumPrice + "</td>   </tr>";

            if (i > 1) {
                $(datastring).appendTo($("#myDataTable"));

                var University = '<p style="text-align:center; font-size:15px;" >'
                University += $("#UniversityID option:selected").text() + " : " + $("#CollegeID option:selected").text();
                University += '</p>'
                $(University).appendTo($("#UniversityView"));

                var DetailsDate = '<td width="150"><b>التاريخ:</b></td>';
                DetailsDate += '<td colspan="3">  <strong>من   : </strong>  ';
                DetailsDate += document.getElementById("date_timepicker_start").value;
                if (document.getElementById("date_timepicker_end").value) {
                    DetailsDate += '  <strong>إلى  : </strong>  ';
                    DetailsDate += document.getElementById("date_timepicker_end").value;
                }
                DetailsDate += '</td>';
                $(DetailsDate).appendTo($("#DetailsDate"));

                var DetailsTotalPrice = '<td><b>الإجمالي :</b></td>';
                DetailsTotalPrice += '<td colspan="3">' + sumPrice + ' د.ك</td>'
                $(DetailsTotalPrice).appendTo($("#DetailsTotalPrice"));

                var DetailsTotalSub = '<td><b>إجمالى السحب:</b></td>';
                DetailsTotalSub += '<td colspan="3">' + (i-1) + '</td>';
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





function getColleges() {
    $.ajax({
        type: "POST",
        url: "/University/getColleges",
        cache: false,
        contentType: "application/json; charset=utf-8",
        data: "{UniversityID:'" + $('#UniversityID').val() +"'}",
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

function getSubjects() {
    $.ajax({
        type: "POST",
        url: "/Subject/getSubjects",
        cache: false,
        contentType: "application/json; charset=utf-8",
        data: "{MajorID:'" + $('#MajorID').val() +
            "'}",
        dataType: "json",
        processData: false,
        success: function (response) {
            if (response !== "ERROR") {
                $('#SubjectID').html(response);
            }
        },
        failure: function (errMsg) {
            alert(errMsg);
        }
    });
}

function getSession() {
    $.ajax({
        type: "POST",
        url: "/Session/getSubjectSessions",
        cache: false,
        contentType: "application/json; charset=utf-8",
        data: "{SubjectID:'" + $('#SubjectID').val() +
            "'}",
        dataType: "json",
        processData: false,
        success: function (response) {
            if (response !== "ERROR") {
                $('#SessionID').html(response);
            }
        },
        failure: function (errMsg) {
            alert(errMsg);
        }
    });
}
