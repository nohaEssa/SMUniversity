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
    
    public partial class TblLecturer
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TblLecturer()
        {
            this.TblBalanceTransactions = new HashSet<TblBalanceTransaction>();
            this.TblLecturerSubjects = new HashSet<TblLecturerSubject>();
            this.TblLecturerPaymentMethods = new HashSet<TblLecturerPaymentMethod>();
            this.TblManualInvoices = new HashSet<TblManualInvoice>();
            this.TblNotifications = new HashSet<TblNotification>();
            this.TblPrivateSessions = new HashSet<TblPrivateSession>();
            this.TblRegisterDevices = new HashSet<TblRegisterDevice>();
            this.TblRevenues = new HashSet<TblRevenue>();
            this.TblRevenues1 = new HashSet<TblRevenue>();
            this.TblRevenues2 = new HashSet<TblRevenue>();
            this.TblRevenues3 = new HashSet<TblRevenue>();
            this.TblRevenues4 = new HashSet<TblRevenue>();
            this.TblRevenues5 = new HashSet<TblRevenue>();
            this.TblSessions = new HashSet<TblSession>();
            this.TblSessions1 = new HashSet<TblSession>();
            this.TblSessions2 = new HashSet<TblSession>();
            this.TblSessions3 = new HashSet<TblSession>();
            this.TblSessions4 = new HashSet<TblSession>();
            this.TblSessions5 = new HashSet<TblSession>();
            this.TblVouchers = new HashSet<TblVoucher>();
        }
    
        public int ID { get; set; }
        public string FirstNameAr { get; set; }
        public string SecondNameAr { get; set; }
        public string ThirdNameAr { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public bool Gender { get; set; }
        public string Address { get; set; }
        public string ProfilePic { get; set; }
        public int BranchID { get; set; }
        public bool IsDeleted { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public int CredentialsID { get; set; }
        public string FirstNameEn { get; set; }
        public string SecondNameEn { get; set; }
        public string ThirdNameEn { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TblBalanceTransaction> TblBalanceTransactions { get; set; }
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
        public virtual ICollection<TblLecturerSubject> TblLecturerSubjects { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TblLecturerPaymentMethod> TblLecturerPaymentMethods { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TblManualInvoice> TblManualInvoices { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TblNotification> TblNotifications { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TblPrivateSession> TblPrivateSessions { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TblRegisterDevice> TblRegisterDevices { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TblRevenue> TblRevenues { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TblRevenue> TblRevenues1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TblRevenue> TblRevenues2 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TblRevenue> TblRevenues3 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TblRevenue> TblRevenues4 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TblRevenue> TblRevenues5 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TblSession> TblSessions { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TblSession> TblSessions1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TblSession> TblSessions2 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TblSession> TblSessions3 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TblSession> TblSessions4 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TblSession> TblSessions5 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TblVoucher> TblVouchers { get; set; }
    }
}
