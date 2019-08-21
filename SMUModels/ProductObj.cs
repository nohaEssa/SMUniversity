using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMUModels
{
    public class ProductObj
    {
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public decimal Cost { get; set; }
        public string DescriptionAr { get; set; }
        public string DescriptionEn { get; set; }
        public string Picture { get; set; }
        public int ProductCategoryID { get; set; }
        public int ProductID { get; set; }
    }
}
