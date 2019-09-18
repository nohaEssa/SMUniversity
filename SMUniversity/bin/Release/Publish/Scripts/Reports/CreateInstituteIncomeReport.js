
function SearchLecturerIncome() {

    $.ajax({
        type: "POST",
        url: "/Reports/GetInstituteIncometByDate",
        cache: false,
        contentType: "application/json; charset=utf-8",
        data: "{StartDate:'" + document.getElementById("date_timepicker_start").value + "',EndDate: '" + document.getElementById("date_timepicker_end").value + "'}",
        dataType: "json",
        async: false,
        success: function (response) {
            debugger;

            $("#DetailsDate td").remove();
            $("#InstituteName td").remove();
            $("#PhoneNumber td").remove();
            $("#Address td").remove();
            $("#TotalInstituteIncome td").remove();
            $("#NetTotal td").remove();

            $("#SessionsCount td").remove();
            $("#TotalInstituteSessionPrice td").remove();
            $("#Net1 td").remove();

            $("#CoursesCount td").remove();
            $("#TotalInstituteCoursePrice td").remove();
            $("#Net2 td").remove();

            $("#PercentageSessionCount td").remove();
            $("#TotalInstitutePercentagePrice td").remove();
            $("#Net3 td").remove();


            $("#FromLecturerSideSessionCount td").remove();
            $("#TotalInstituteFromLecturerSidePrice td").remove();
            $("#Net4 td").remove();

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

                var InstituteName = '<td width="300"><b>اسم المعهد :</b></td>';
                InstituteName += '<td colspan="3" width="300">معهد سمارت مايند  </td>'
                $(InstituteName).appendTo($("#InstituteName"));

                var PhoneNumber = '<td width="300"><b>هاتف المعهد :</b></td>';
                PhoneNumber += '<td colspan="3" width="300">' + response.PhoneNumber + ' </td>'
                $(PhoneNumber).appendTo($("#PhoneNumber"));

                var Address = '<td width="300"><b>بريدالكترونى:</b></td>';
                Address += '<td colspan="3" width="300">' + response.Address + ' </td>'
                $(Address).appendTo($("#Address"));

                var TotalInstituteIncome = '<td width="300"><b>إجمالى دخل المعهد :</b></td>';
                TotalInstituteIncome += '<td colspan="3" width="300">' + response.TotalInstituteIncome + ' د.ك</td>'
                $(TotalInstituteIncome).appendTo($("#TotalInstituteIncome"));

                var NetTotal = '<td width="200"><b>صافى دخل المعهد :</b></td>';
                NetTotal += '<td colspan="3" width="200">' + response.NetInstituteIncome + ' د.ك</td>'
                NetTotal += '<td width="200"><b>صافى دخل المحاضرين :</b></td>';
                NetTotal += '<td colspan="3" width="200">' + response.NetLecturerIncome + ' د.ك</td>'
                $(NetTotal).appendTo($("#NetTotal"));
                // --------------------------------------------------------------


                var SessionsCount = '<td  width="300"><b>عدد حصص المعهد :</b></td>';
                SessionsCount += '<td colspan="3" width="300">' + response.SessionsCount + ' </td>'
                $(SessionsCount).appendTo($("#SessionsCount"));

                var TotalInstituteSessionPrice = '<td width="300"><b>إجمالى دخل المعهد بالحصص :</b></td>';
                TotalInstituteSessionPrice += '<td colspan="3" width="300">' + response.TotalInstituteSessionPrice + ' د.ك</td>'
                $(TotalInstituteSessionPrice).appendTo($("#TotalInstituteSessionPrice"));


                var Net1 = '<td width="200"><b>صافى دخل المعهد :</b></td>';
                Net1 += '<td colspan="3" width="200">' + response.NetInstituteSessionPrice + ' د.ك</td>'
                Net1 += '<td width="200"><b>صافى دخل المحاضرين :</b></td>';
                Net1 += '<td colspan="3" width="200">' + response.NetLecturerSessionPrice + ' د.ك</td>'
                $(Net1).appendTo($("#Net1"));
                // --------------------------------------------------------------

                var CoursesCount = '<td width="300"><b>عدد دورات المعهد :</b></td>';
                CoursesCount += '<td colspan="3" width="300">' + response.CoursesCount + ' </td>'
                $(CoursesCount).appendTo($("#CoursesCount"));

                var TotalInstituteCoursePrice = '<td width="300"><b>إجمالى دخل المعهد بالدورات :</b></td>';
                TotalInstituteCoursePrice += '<td colspan="3" width="300">' + response.TotalInstituteCoursePrice + ' د.ك</td>'
                $(TotalInstituteCoursePrice).appendTo($("#TotalInstituteCoursePrice"));

                var Net2 = '<td width="200"><b>صافى دخل المعهد :</b></td>';
                Net2 += '<td colspan="3" width="200">' + response.NetInstituteCoursePrice + ' د.ك</td>'
                Net2 += '<td width="200"><b>صافى دخل المحاضرين :</b></td>';
                Net2 += '<td colspan="3" width="200">' + response.NetLecturerCoursePrice + ' د.ك</td>'
                $(Net2).appendTo($("#Net2"));
                // --------------------------------------------------------------

                var PercentageSessionCount = '<td width="300"><b>عدد حصص المعهد بالنسبة :</b></td>';
                PercentageSessionCount += '<td colspan="3" width="300">' + response.PercentageSessionCount + ' </td>'
                $(PercentageSessionCount).appendTo($("#PercentageSessionCount"));

                var TotalInstitutePercentagePrice = '<td width="300"><b>إجمالى دخل المعهد بالنسبة  :</b></td>';
                TotalInstitutePercentagePrice += '<td colspan="3" width="300">' + response.TotalInstitutePercentagePrice + ' د.ك</td>'
                $(TotalInstitutePercentagePrice).appendTo($("#TotalInstitutePercentagePrice"));

                var Net3 = '<td width="200"><b>صافى دخل المعهد :</b></td>';
                Net3 += '<td colspan="3" width="200">' + response.NetInstitutePercentagePrice + ' د.ك</td>'
                Net3 += '<td width="200"><b>صافى دخل المحاضرين :</b></td>';
                Net3 += '<td colspan="3" width="200">' + response.NetLecturerPercentagePrice + ' د.ك</td>'
                $(Net3).appendTo($("#Net3"));
                // --------------------------------------------------------------

                var FromLecturerSideSessionCount = '<td width="300"><b>عدد حصص المعهد للطلبة من جهه المحاضر :</b></td>';
                FromLecturerSideSessionCount += '<td colspan="3" width="300">' + response.FromLecturerSideSessionCount + ' </td>'
                $(FromLecturerSideSessionCount).appendTo($("#FromLecturerSideSessionCount"));

                var TotalInstituteFromLecturerSidePrice = '<td width="300"><b>إجمالى دخل المعهد من طلبه جهه المحاضر  :</b></td>';
                TotalInstituteFromLecturerSidePrice += '<td colspan="3" width="300">' + response.TotalInstituteFromLecturerSidePrice + ' د.ك</td>'
                $(TotalInstituteFromLecturerSidePrice).appendTo($("#TotalInstituteFromLecturerSidePrice"));

                var Net4 = '<td width="200"><b>صافى دخل المعهد :</b></td>';
                Net4 += '<td colspan="3" width="200">' + response.NetInstituteFromLecturerSidePrice + ' د.ك</td>'
                Net4 += '<td width="200"><b>صافى دخل المحاضرين :</b></td>';
                Net4 += '<td colspan="3" width="200">' + response.NetFromLecturerSidePrice + ' د.ك</td>'
                $(Net4).appendTo($("#Net4"));



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