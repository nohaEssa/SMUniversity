using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMUModels.ObjectData
{
    public class SubscriptionObj
    {
        public int SessionID { get; set; }
        public int StudentID { get; set; }
        public bool Pending { get; set; }
        public bool AddOrRemove { get; set; }
    }
}
