using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMUniversity.Models
{
    public class SubscriptionViewModel
    {
        public int ID { get; set; }
        public string StudentName{ get; set; }
        public Nullable<decimal> Price { get; set; }
        public string PaymentMethod { get; set; }
        public string CreatedAt { get; set; }


    }
}