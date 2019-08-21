using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMUModels
{
    public class ContactUsData
    {
        public InstituteData InstituteData { get; set; }
        public List<BranchData> BranchData { get; set; }
    }

    public class BranchData
    {
        public int BranchID { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string PhoneNumber1 { get; set; }
        public string PhoneNumber2 { get; set; }
        public string PhoneNumber3 { get; set; }
        public string AddressAr { get; set; }
        public string AddressEn { get; set; }
        public string MapLink { get; set; }
        public string DescriptionAr { get; set; }
        public string DescriptionEn { get; set; }
    }

    public class InstituteData
    {
        public string Email { get; set; }
        public string Website { get; set; }
    }
}
