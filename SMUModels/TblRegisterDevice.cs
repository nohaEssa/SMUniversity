//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SMUModels
{
    using System;
    using System.Collections.Generic;
    
    public partial class TblRegisterDevice
    {
        public int ID { get; set; }
        public string Token { get; set; }
        public int DeviceTypeID { get; set; }
        public Nullable<int> StudentID { get; set; }
        public Nullable<int> LecturerID { get; set; }
        public bool IsDeleted { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public Nullable<int> BadgeNo { get; set; }
    
        public virtual TblLecturer TblLecturer { get; set; }
        public virtual TblStudent TblStudent { get; set; }
    }
}
