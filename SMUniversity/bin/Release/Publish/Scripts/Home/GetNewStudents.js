$(window).load(function () {

    $.ajax({
        type: "POST",
        url: "/Home/GetNewStudents",
        cache: false,
        contentType: "application/json; charset=utf-8",
        data: "",
        dataType: "json",
        async: false,
        success: function (response) {
            debugger;

            $("#ls-editable-table tbody").remove();

            var datastring = "";
            var i = 1;
            var sumPrice = 0;
            $.each(response, function (index, value) {
                datastring += "<tr> <td  class='text-center'> " + (i++) + " </td> <td class='text-center'> " + value.StudentName + "</td> <td  class='text-center'>" + value.Email + "</td> <td  class='text-center'>" + value.Phone + "</td> <td  class='text-center'>" + value.CollageName + "</td> <td  class='text-center'>" + value.Branch + "</td> </tr>";
                sumPrice += value.Price
            });
            //   datastring += "<tr> <td> " + "إجمالى التكلفه" + " </td>   <td> " + sumPrice + "</td>   </tr>";
            if (i > 1) {

                $(datastring).appendTo($("#ls-editable-table"));

            }
        },
        failure: function (errMsg) {

            ShowSucsses(errMsg, "2");
        }
    });


    $.ajax({
        type: "POST",
        url: "/Home/GetNewVoucher",
        cache: false,
        contentType: "application/json; charset=utf-8",
        data: "",
        dataType: "json",
        async: false,
        success: function (response) {
            debugger;

            $("#lastVoucher tbody").remove();

            var datastring = "";
            var i = 1;
            var sumPrice = 0;
            $.each(response, function (index, value) {
                datastring += "<tr> <td  class='text-center'> " + (i++) + " </td> <td class='text-center'> " + value.Serial + "</td> <td  class='text-center'>" + value.lecturerName + "</td> <td  class='text-center'>" + value.HallCode + "</td> <td  class='text-center'>" + value.PaymentMethod + "</td> <td  class='text-center'>" + value.Cost + "</td>  <td  class='text-center'>" + value.createdDate + "</td> <td  class='text-center'> <a class='btn btn-xs btn-info' href='/Invoice/VoucherTemplate?VoucherID=" + value.ID + "'><i class='fa fa-eye'></i></a></td></tr>";
                sumPrice += value.Price
            });
            //   datastring += "<tr> <td> " + "إجمالى التكلفه" + " </td>   <td> " + sumPrice + "</td>   </tr>";
            if (i > 1) {

                $(datastring).appendTo($("#lastVoucher"));
            }
        },
        failure: function (errMsg) {

            ShowSucsses(errMsg, "2");
        }
    });


})

