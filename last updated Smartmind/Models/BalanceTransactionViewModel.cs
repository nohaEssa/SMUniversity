using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMUniversity.Models
{
    public class BalanceTransactionViewModel
    {
        public int ID { get; set; }
        public int StudentID { get; set; }
        public string StudentName { get; set; }
        public string TitleAr { get; set; }
        public Nullable<decimal> Price { get; set; }
        public int TransactionTypeID { get; set; }
        public string PaymentMethod { get; set; }
    }
}