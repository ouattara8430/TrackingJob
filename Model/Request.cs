using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackingJob.Model
{
    class Request
    {
        public string start_date { get; set; }
        public string end_date { get; set; }
        public string alert_frequence { get; set; }
        public string alert_frequence_periodicity { get; set; }
        public string decisions { get; set; }
        public string fullname { get; set; }
        public string email { get; set; }
        public string responsable_email { get; set; }
        public string tracking_officer { get; set; }
        public string decision_desc { get; set; }
        public string financing_type_name { get; set; }
        public string loan_description { get; set; }
        public string loan_amount { get; set; }
        public int customer_no { get; set; }
        public string customer_fullname { get; set; }
        public int case_id { get; set; }
    }
}
