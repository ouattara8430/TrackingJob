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
    
    public partial class ActivityLog
    {
        public int log_id { get; set; }
        public string action_name { get; set; }
        public string action_description { get; set; }
        public Nullable<System.DateTime> log_date { get; set; }
        public Nullable<int> userId { get; set; }
    
        public virtual UserProfile UserProfile { get; set; }
    }
}