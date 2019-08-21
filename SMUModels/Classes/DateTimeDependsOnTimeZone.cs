using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMUModels
{
    public class DateTimeDependsOnTimeZone
    {
        public static DateTime GetDate()
        {
            DateTime _DateTime = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("Arab Standard Time"));

            return _DateTime;
        }
    }
}