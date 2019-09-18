using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMUModels
{
    public class ProductObj
    {
        public string NameAr { get; set; } = "";
        public string NameEn { get; set; } = "";
        public decimal Cost { get; set; } = 0;
        public string DescriptionAr { get; set; } = "";
        public string DescriptionEn { get; set; } = "";
        public string Picture { get; set; } = "";
        public int ProductCategoryID { get; set; } = 0;
        public int ProductID { get; set; } = 0;
    }
}
