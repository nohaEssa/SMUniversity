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
    
    public partial class TblArea
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TblArea()
        {
            this.TblStudents = new HashSet<TblStudent>();
            this.TblStudents1 = new HashSet<TblStudent>();
            this.TblStudents2 = new HashSet<TblStudent>();
            this.TblStudents3 = new HashSet<TblStudent>();
            this.TblStudents4 = new HashSet<TblStudent>();
            this.TblStudents5 = new HashSet<TblStudent>();
        }
    
        public int ID { get; set; }
        public string NameAr { get; set; }
        public int GovernorateID { get; set; }
        public bool IsDeleted { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public string NameEn { get; set; }
    
        public virtual TblGovernorate TblGovernorate { get; set; }
        public virtual TblGovernorate TblGovernorate1 { get; set; }
        public virtual TblGovernorate TblGovernorate2 { get; set; }
        public virtual TblGovernorate TblGovernorate3 { get; set; }
        public virtual TblGovernorate TblGovernorate4 { get; set; }
        public virtual TblGovernorate TblGovernorate5 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TblStudent> TblStudents { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TblStudent> TblStudents1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TblStudent> TblStudents2 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TblStudent> TblStudents3 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TblStudent> TblStudents4 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TblStudent> TblStudents5 { get; set; }
    }
}
