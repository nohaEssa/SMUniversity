
function SearchLecturerIncome() {

    $.ajax({
        type: "POST",
        url: "/Reports/GetLecturerIncometByDate",
        cache: false,
        contentType: "application/json; charset=utf-8",
        data: "{StartDate:'" + document.getElementById("date_timepicker_start").value + "',EndDate: '" + document.getElementById("date_timepicker_end").value + "',LecturerID:'" + $('#LecturerID').val() + "'}",
        dataType: "json",
        async: false,
        success: function (response) {
            debugger;
            
            $("#DetailsDate td").remove();
            $("#LecturerName td").remove();
            $("#PhoneNumber td").remove();
            $("#Address td").remove();
            $("#TotalLecturerIncome td").remove();

            $("#SessionsCount td").remove();
            $("#LecturerPricePerSession td").remove();
            $("#TotalLecturerSessionPrice td").remove();

            $("#CoursesCount td").remove();
            $("#LecturerPricePerCourse td").remove();
            $("#TotalLecturerCoursePrice td").remove();

            $("#PercentageSessionCount td").remove();
            $("#PercentageSessionSumPrice td").remove();
            $("#LecturerPercentagePerSession td").remove();
            $("#TotalLecturerPercentagePrice td").remove();

            $("#FromLecturerSideSessionCount td").remove();
            $("#FromLecturerSidePerSession td").remove();
            $("#TotalFromLecturerSidePrice td").remove();

            if (response) {

                var DetailsDate = '<td width="300"><b>التاريخ:</b></td>';
                DetailsDate += '<td colspan="3" width="300">  <strong>من   : </strong>  ';
                DetailsDate += document.getElementById("date_timepicker_start").value;
                if (document.getElementById("date_timepicker_end").value) {
                    DetailsDate += '  <strong>إلى  : </strong>  ';
                    DetailsDate += document.getElementById("date_timepicker_end").value;
                }
                DetailsDate += '</td>';
                $(DetailsDate).appendTo($("#DetailsDate"));

                var LecturerName = '<td width="300"><b>اسم المحاضر :</b></td>';
                LecturerName += '<td colspan="3" width="300">' + response.LecturerName + ' </td>'
                $(LecturerName).appendTo($("#LecturerName"));

                var PhoneNumber = '<td width="300"><b>هاتف المحاضر :</b></td>';
                PhoneNumber += '<td colspan="3" width="300">' + response.PhoneNumber + ' </td>'
                $(PhoneNumber).appendTo($("#PhoneNumber"));

                var Address = '<td width="300"><b>عنوان المحاضر :</b></td>';
                Address += '<td colspan="3" width="300">' + response.Address + ' </td>'
                $(Address).appendTo($("#Address"));

                var TotalLecturerIncome = '<td width="300"><b>إجمالى دخل المحاضر :</b></td>';
                TotalLecturerIncome += '<td colspan="3" width="300">' + response.TotalLecturerIncome + ' د.ك</td>'
                $(TotalLecturerIncome).appendTo($("#TotalLecturerIncome"));

               // --------------------------------------------------------------

                var SessionsCount = '<td width="300"><b>عدد حصص المحاضر :</b></td>';
                SessionsCount += '<td colspan="3" width="300">' + response.SessionsCount + ' </td>'
                $(SessionsCount).appendTo($("#SessionsCount"));

                var LecturerPricePerSession = '<td width="300"><b>سعرالمحاضر بالحصة  :</b></td>';
                LecturerPricePerSession += '<td colspan="3" width="300">' + response.LecturerPricePerSession + ' د.ك</td>'
                $(LecturerPricePerSession).appendTo($("#LecturerPricePerSession"));

                var TotalLecturerSessionPrice = '<td width="300"><b>إجمالى دخل المحاضر بالحصص :</b></td>';
                TotalLecturerSessionPrice += '<td colspan="3" width="300">' + response.TotalLecturerSessionPrice + ' د.ك</td>'
                $(TotalLecturerSessionPrice).appendTo($("#TotalLecturerSessionPrice"));

                // --------------------------------------------------------------

                var CoursesCount = '<td width="300"><b>عدد دورات المحاضر :</b></td>';
                CoursesCount += '<td colspan="3"  width="300">' + response.CoursesCount + ' </td>'
                $(CoursesCount).appendTo($("#CoursesCount"));

                var LecturerPricePerCourse = '<td width="300"><b>سعرالمحاضر بالدورة  :</b></td>';
                LecturerPricePerCourse += '<td colspan="3" width="300">' + response.LecturerPricePerCourse + ' د.ك</td>'
                $(LecturerPricePerCourse).appendTo($("#LecturerPricePerCourse"));

                var TotalLecturerCoursePrice = '<td width="300"><b>إجمالى دخل المحاضر بالدورات :</b></td>';
                TotalLecturerCoursePrice += '<td colspan="3" width="300">' + response.TotalLecturerCoursePrice + ' د.ك</td>'
                $(TotalLecturerCoursePrice).appendTo($("#TotalLecturerCoursePrice"));


                // --------------------------------------------------------------

                var PercentageSessionCount = '<td width="300"><b>عدد حصص المحاضر بالنسبة :</b></td>';
                PercentageSessionCount += '<td colspan="3" width="300">' + response.PercentageSessionCount + ' </td>'
                $(PercentageSessionCount).appendTo($("#PercentageSessionCount"));

                var PercentageSessionSumPrice = '<td width="300"><b>إجمالى ثمن الحصص بالنسبة  :</b></td>';
                PercentageSessionSumPrice += '<td colspan="3" width="300">' + response.PercentageSessionSumPrice + ' د.ك</td>'
                $(PercentageSessionSumPrice).appendTo($("#PercentageSessionSumPrice"));

                var LecturerPercentagePerSession = '<td width="300"><b>نسبة المحاضر :</b></td>';
                LecturerPercentagePerSession += '<td colspan="3" width="300">' + response.LecturerPercentagePerSession + ' %</td>'
                $(LecturerPercentagePerSession).appendTo($("#LecturerPercentagePerSession"));

                var TotalLecturerPercentagePrice = '<td width="300"><b>إجمالى دخل المحاضر بالنسبة :</b></td>';
                TotalLecturerPercentagePrice += '<td colspan="3" width="300">' + response.TotalLecturerPercentagePrice + ' د.ك</td>'
                $(TotalLecturerPercentagePrice).appendTo($("#TotalLecturerPercentagePrice"));

                // --------------------------------------------------------------

                var FromLecturerSideSessionCount = '<td width="300"><b>عدد حصص حضور الطلبه من جهه المحاضر:</b></td>';
                FromLecturerSideSessionCount += '<td colspan="3" width="300">' + response.FromLecturerSideSessionCount + ' </td>'
                $(FromLecturerSideSessionCount).appendTo($("#FromLecturerSideSessionCount"));

                var FromLecturerSidePerSession = '<td width="300"><b>سعر المحاضر فى الطالب جهته  :</b></td>';
                FromLecturerSidePerSession += '<td colspan="3" width="300">' + response.FromLecturerSidePerSession + ' د.ك</td>'
                $(FromLecturerSidePerSession).appendTo($("#FromLecturerSidePerSession"));

                var TotalFromLecturerSidePrice = '<td width="300"><b>إجمالى دخل المحاضر :</b></td>';
                TotalFromLecturerSidePrice += '<td colspan="3" width="300">' + response.TotalFromLecturerSidePrice + ' د.ك</td>'
                $(TotalFromLecturerSidePrice).appendTo($("#TotalFromLecturerSidePrice"));


                document.getElementById("printbtn").style.display = 'block';
                document.getElementById("logoDiv").style.display = 'block';
                document.getElementById("LecturerDetails").style.display = 'block';
                document.getElementById("SessionsDetails").style.display = 'block';
                document.getElementById("CoursesDetails").style.display = 'block';
                document.getElementById("PercentageDetails").style.display = 'block';
                document.getElementById("FromLecturerSideDetails").style.display = 'block';

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