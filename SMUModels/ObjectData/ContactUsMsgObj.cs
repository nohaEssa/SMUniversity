﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMUModels.ObjectData
{
    public class ContactUsMsgObj
    {
        public int? StudentID { get; set; } = 0;
        public string Name { get; set; } = "";
        public string PhoneNumber { get; set; } = "";
        public string Email { get; set; } = "";
        public string Message { get; set; } = "";
    }
}
