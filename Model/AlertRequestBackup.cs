//------------------------------------------------------------------------------
// <auto-generated>
//     Ce code a été généré à partir d'un modèle.
//
//     Des modifications manuelles apportées à ce fichier peuvent conduire à un comportement inattendu de votre application.
//     Les modifications manuelles apportées à ce fichier sont remplacées si le code est régénéré.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TrackingJob.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class AlertRequestBackup
    {
        public Nullable<int> case_id { get; set; }
        public Nullable<int> userId { get; set; }
        public Nullable<System.DateTime> start_date { get; set; }
        public Nullable<System.DateTime> end_date { get; set; }
        public Nullable<System.DateTime> due_date { get; set; }
        public string alert_frequence { get; set; }
        public string alert_frequence_periodicity { get; set; }
        public string action_frequence { get; set; }
        public string alert_action_periodicity { get; set; }
        public Nullable<bool> status { get; set; }
        public int executed_by { get; set; }
        public string decisions { get; set; }
        public int decision_id { get; set; }
        public Nullable<System.DateTime> due_date_action { get; set; }
        public Nullable<System.DateTime> end_date_action { get; set; }
        public string application_id { get; set; }
        public Nullable<System.DateTime> triggered_date { get; set; }
        public Nullable<System.DateTime> created_at { get; set; }
        public int backup_id { get; set; }
    }
}
