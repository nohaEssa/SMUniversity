﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMUModels.ObjectData
{
    public class UserObj
    {
        public int UserID { get; set; } = 0;
        public string Email { get; set; } = "";
        public string NameAr { get; set; } = "";
        public string NameEn { get; set; } = "";
        public string PhoneNumber { get; set; } = "";
        public int BranchID { get; set; } = 0;
        public int UserCategoryID { get; set; } = 0;
        public string UserName { get; set; } = "";
        public string Password { get; set; } = "";
    }
}
