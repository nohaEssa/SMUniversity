using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMUModels.ObjectData
{
    public class PaymentObj
    {
        public int StudentID { get; set; } = 0;
        public List<int> CardCategoryIDs { get; set; }
        public string PaymentMethod { get; set; } = "";
        public decimal NewPrice { get; set; } = 0;
    }
}
