using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMUniversity.Models
{
    public class collectionModel
    {
        public collectionModel()
        {
            Subscriptions = new List<SubscriptionViewModel>();
            pricesDetails = new List<string>();
        }
        public List<SubscriptionViewModel> Subscriptions { get; set; }
        public List<string> pricesDetails { get; set; }
    }

}