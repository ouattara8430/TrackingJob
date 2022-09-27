using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackingJob.Model
{
    class Request
    {
        //public string start_date { get; set; }
        public DateTime? start_date { get; set; }
        //public string end_date { get; set; }
        public DateTime? end_date { get; set; }
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
        public int? customer_no { get; set; }
        public string customer_fullname { get; set; }
        public int case_id { get; set; }
        public DateTime? next_alert_date { get; set; }
        public int? nb_occurence { get; set; }
        public DateTime? next_action_date { get; set; }
        public int? nb_occurence_action { get; set; }
        public string CODE_AGENCE { get; set; }
        public string NUMERO_COMPTE { get; set; }
        public string CLE_COMPTE { get; set; }
        public string INTITULE_COMPTE { get; set; }
        public string SECTEUR_ACTIVITE { get; set; }
        public string TYPE_ENGAGEMENT { get; set; }
        public DateTime? DATE_MISE_EN_PLACE { get; set; }
        public DateTime? DATE_1ERE_ECHEANCE { get; set; }
        public DateTime? DATE_FIN_ECHEANCE { get; set; }
        public string MONTANT_DEBLOQUE { get; set; }
        public string ENCOURS_CREDIT { get; set; }
        public string IMPAYES_CREDIT { get; set; }
        public string PERIODICITE { get; set; }
        public double? NBRE_ECHEANCE { get; set; }
        public string GESTIONNAIRE { get; set; }
        public string EMAIL_CLIENT { get; set; }
        public string GESTIONNAIRE_EMAIL { get; set; }
        public bool? status { get; set; }
        public DateTime? triggered_date { get; set; }
        public DateTime? end_date_action { get; set; }
        public DateTime? due_date_action { get; set; }
        public string application_id { get; set; }
    }
}
