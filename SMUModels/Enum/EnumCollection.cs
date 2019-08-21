using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMUModels.Enum
{
    public class EnumCollection
    {
        public enum CourseLecturesAr
        {
            [StringValue("المحاضره الأولي")]
            Lec1,
            [StringValue("المحاضره الثانيه")]
            Lec2,
            [StringValue("المحاضره الثالثه")]
            Lec3,
            [StringValue("المحاضره الرابعه")]
            Lec4,
            [StringValue("المحاضره الخامسه")]
            Lec5,
            [StringValue("المحاضره السادسه")]
            Lec6,
            [StringValue("المحاضره السابعه")]
            Lec7,
            [StringValue("المحاضره الثامنه")]
            Lec8,
            [StringValue("المحاضره التاسعه")]
            Lec9,
            [StringValue("المحاضره العاشره")]
            Lec10
        }

        public enum CourseLecturesEn
        {
            [StringValue("1st Lecture")]
            Lec1,
            [StringValue("2nd Lecture")]
            Lec2,
            [StringValue("3rd Lecture")]
            Lec3,
            [StringValue("4th Lecture")]
            Lec4,
            [StringValue("5th Lecture")]
            Lec5,
            [StringValue("6th Lecture")]
            Lec6,
            [StringValue("7th Lecture")]
            Lec7,
            [StringValue("8th Lecture")]
            Lec8,
            [StringValue("9th Lecture")]
            Lec9,
            [StringValue("10th Lecture")]
            Lec10
        }
    }

    /// <summary>
    /// Simple attribute class for storing String Values
    /// </summary>
    public class StringValueAttribute : Attribute
    {
        private string _value;

        /// <summary>
        /// Creates a new <see cref="StringValueAttribute"/> instance.
        /// </summary>
        /// <param name="value">Value.</param>
        public StringValueAttribute(string value)
        {
            _value = value;
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <value></value>
        public string Value
        {
            get { return _value; }
        }
    }
}
