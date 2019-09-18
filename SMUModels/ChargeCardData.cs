using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMUModels
{
    public class ChargeCardData
    {
        public string Code { get; set; } = "";
        public decimal Price { get; set; } = 0;
        public bool Valid { get; set; } = false;
    }
}
