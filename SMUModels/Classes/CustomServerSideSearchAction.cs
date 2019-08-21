using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMUModels.Classes
{
    //public JsonResult CustomServerSideSearchAction(DataTableAjaxPostModel model)
    //{
    //    // action inside a standard controller
    //    int filteredResultsCount;
    //    int totalResultsCount;
    //    var res = YourCustomSearchFunc(model, out filteredResultsCount, out totalResultsCount);

    //    var result = new List<YourCustomSearchClass>(res.Count);
    //    foreach (var s in res)
    //    {
    //        // simple remapping adding extra info to found dataset
    //        result.Add(new YourCustomSearchClass
    //        {
    //            EmployerId = User.ClaimsUserId(),
    //            Id = s.Id,
    //            Pin = s.Pin,
    //            Firstname = s.Firstname,
    //            Lastname = s.Lastname,
    //            RegistrationStatusId = DoSomethingToGetIt(s.Id),
    //            Address3 = s.Address3,
    //            Address4 = s.Address4
    //        });
    //    };

    //    return Json(new
    //    {
    //        // this is what datatables wants sending back
    //        draw = model.draw,
    //        recordsTotal = totalResultsCount,
    //        recordsFiltered = filteredResultsCount,
    //        data = result
    //    });
    //}
}
