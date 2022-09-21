//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TrackingJob.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class LoanApplication
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public LoanApplication()
        {
            this.AlertRequestNews = new HashSet<AlertRequest>();
        }
    
        public string application_id { get; set; }
        public string loan_name { get; set; }
        public string loan_description { get; set; }
        public string loan_type_desc { get; set; }
        public string mode_payment { get; set; }
        public string loan_amount { get; set; }
        public Nullable<System.DateTime> loan_date { get; set; }
        public string purpose { get; set; }
        public Nullable<bool> loan_status { get; set; }
        public string remarks { get; set; }
        public string processed_by { get; set; }
        public Nullable<int> customer_no { get; set; }
        public string customer_fullname { get; set; }
        public Nullable<System.DateTime> loan_period { get; set; }
        public string loan_rate { get; set; }
        public Nullable<int> financing_code { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AlertRequest> AlertRequestNews { get; set; }
        public virtual Type Type { get; set; }
    }
}
