using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMUModels.ObjectData
{
    public class LoginData
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }
        public int DeviceTypeID { get; set; }
    }
}
