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
    
    public partial class TblEmployee
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TblEmployee()
        {
            this.TblInvoices = new HashSet<TblInvoice>();
        }
    
        public int ID { get; set; }
        public string NameAr { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsActive { get; set; }
        public string ProfilePic { get; set; }
        public int BranchID { get; set; }
        public int UserCategoryID { get; set; }
        public bool IsDeleted { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public string NameEn { get; set; }
        public int CredentialsID { get; set; }
    
        public virtual TblBranch TblBranch { get; set; }
        public virtual TblBranch TblBranch1 { get; set; }
        public virtual TblBranch TblBranch2 { get; set; }
        public virtual TblBranch TblBranch3 { get; set; }
        public virtual TblBranch TblBranch4 { get; set; }
        public virtual TblBranch TblBranch5 { get; set; }
        public virtual TblUserCredential TblUserCredential { get; set; }
        public virtual TblUserCredential TblUserCredential1 { get; set; }
        public virtual TblUserCredential TblUserCredential2 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TblInvoice> TblInvoices { get; set; }
    }
}
