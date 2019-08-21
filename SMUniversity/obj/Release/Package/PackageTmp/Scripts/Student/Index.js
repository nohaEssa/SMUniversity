$(document).ready(function () {
    data = {
        //"dom": 'Blfrtip',

        "bServerSide": true,
        "sAjaxSource": "/BalanceTransaction/index",
        "bProcessing": true,
        "lengthMenu": [[10, 20, 50, -1], [10, 20, 50, "All"]],
        "aoColumns": [
            { "sName": "StudentID", },
            { "sName": "StudentName" },
            { "sName": "PhoneNumber" },
            {
                "bSearchable": false,
                "bSortable": false,
                "mRender": function (data, type, full) {
                    var id = full[0]; //row id in the first column
                    return "<a href='/Student/StudentProfile?STID=" + id + "' class='btn btn-icon-only btn-circle default tooltips'  data-container='body' data-placement='right' data-original-title='البروفايل'  > <i class='fa fa-user'></i>  </a>" + "<a href='/Student/Register?STID=" + id + "' class='btn btn-icon-only btn-circle green tooltips'  data-container='body' data-placement='right' data-original-title='تسجيل مواد'  > <i class='fa fa-book'></i>  </a>" + "<a href='/Student/Edit?STID=" + id + "' class='btn btn-icon-only btn-circle blue tooltips' data-container='body' data-placement='right' data-original-title='تعديل' > <i class='fa fa-edit'></i>  </a>" + "<a data-toggle='modal' href='#Adohjh' class='btn btn-icon-only btn-circle red tooltips' data-container='body' data-placement='right' data-original-title='مسح البيانات' onclick='Javascript: AddIDGoDelete(" + id + ");' > <i class='fa fa-recycle'></i>  </a>" + "<a href='/Student/Schedule?StID=" + id + "' class='btn btn-icon-only btn-circle purple tooltips' data-container='body' data-placement='right' data-original-title='الجدول الدراسى'  > <i class='fa fa-calendar'></i>  </a><a data-toggle='modal' href='#basic' class='btn btn-icon-only btn-circle blue tooltips' data-container='body' data-placement='right' data-original-title='نسخ البيانات' onclick='Javascript: CopyStudentData(" + id + ");' > <i class='fa fa-recycle'></i>  </a><a href='/Student/DropOut?Code=" + id + "' class='btn btn-circle light blue tooltips' data-container='body' data-placement='right' data-original-title='استمارة الإنسحاب'  > <i class='fa fa-map'></i>استمارة الإنسحاب</a>  <a class='btn btn-circle light blue tooltips' data-container='body' data-placement='right' data-original-title='ليلة الأختبار' onclick='Javascript: getStudentData(" + id + ");' > سند ليلة الأختبار</a>";
                }
            }
        ]
    };
    $('#myDataTable').dataTable(data);
});
