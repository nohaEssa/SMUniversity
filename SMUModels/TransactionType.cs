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
    
    public partial class TransactionType
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TransactionType()
        {
            this.TblBalanceTransactions = new HashSet<TblBalanceTransaction>();
            this.TblBalanceTransactions1 = new HashSet<TblBalanceTransaction>();
            this.TblBalanceTransactions2 = new HashSet<TblBalanceTransaction>();
        }
    
        public int ID { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TblBalanceTransaction> TblBalanceTransactions { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TblBalanceTransaction> TblBalanceTransactions1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TblBalanceTransaction> TblBalanceTransactions2 { get; set; }
    }
}
