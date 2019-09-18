using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMUniversity.Models
{
    public class HallRentViewModel
    {
        public int ID { get; set; }
        public string HallName { get; set; }
        public string lectureName { get; set; }
        public string PhoneNumber { get; set; }
        public string CreatedAt { get; set; }
        public Nullable<decimal> Price { get; set; }
        public string PaymentMethod { get; set; }
        public int Capacity { get; set; }

    }
}