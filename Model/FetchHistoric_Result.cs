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
    
    public partial class FetchHistoric_Result
    {
        public int userId { get; set; }
        public string username { get; set; }
        public string email { get; set; }
        public Nullable<int> TotalRequest { get; set; }
        public Nullable<int> Approved { get; set; }
        public Nullable<int> Rejected { get; set; }
        public Nullable<int> LateProcess { get; set; }
    }
}
