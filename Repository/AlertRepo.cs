using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackingJob.Log;
using TrackingJob.Model;

namespace TrackingJob.Repository
{
    class AlertRepo
    {
        private static CommitmentDBEntities db = null;

        // fetch the list of 
        public static List<AlertRequest> getRequests ()
        {
            try {
                db = new CommitmentDBEntities();
                return db.AlertRequests.Where( x => x.start_date == DateTime.Now ).ToList();
            } catch(Exception ex ) {

            }
            return null;
        }
        
        // fetch the list of 
        public static List<AlertRequest> getRepetitiveRequests ()
        {
            try {
                db = new CommitmentDBEntities();
                return db.AlertRequests.OrderByDescending(c => c.end_date_action).Where( x => x.end_date_action != null ).ToList();
            } catch(Exception ex ) {

            }
            return null;
        }

        // get the details of user
        public static UserProfile getUser(int userId )
        {
            try {
                db = new CommitmentDBEntities();
                return db.UserProfiles.Where( x => x.userId == userId ).FirstOrDefault();
            } catch(Exception ex ) {

            }
            return null;
        }

        // save new request
        public static bool GenerateNewRequest ( AlertRequest request)
        {
            try {
                db = new CommitmentDBEntities();
                db.AlertRequests.Add( request );
                int save = db.SaveChanges();
                if ( save > 0 ) {
                    // send email
                    //Email.send( request.UserProfile1.email, "decision d'alerte", "Vous avez une requete a effectuer" );

                    // log activity
                    LogWriter.LogWrite("Title => " + "Pret accorde\n" + "Enregistrement d'une nouvelle a partir du job pour le dossier de pret no. " + request.application_id + "\n");
                }
                else {
                    // log activity
                    LogWriter.LogWrite( "Error durant l'enregistrement du pret no: " + request.application_id );
                }
                return true;
            } catch(Exception ex ) {
                ErrorLog.LogWrite( "Error message: " + ex.Message );
                return false;
            }
        }

        // historique
        public static IEnumerable<FetchRequests_Result> FetchRequest ()
        {
            try {
                using ( CommitmentDBEntities db = new CommitmentDBEntities() ) {
                    IEnumerable<FetchRequests_Result> historique_Results = db.Database.SqlQuery<FetchRequests_Result>( "FetchRequests" ).ToList();
                    return historique_Results;
                }
            }
            catch ( Exception ex ) {
                ErrorLog.LogWrite( "Error message: " + ex.Message );
                return null;
            }
        }


        // update start_date of request
        public static bool UpdateRequest ( int case_id, DateTime start_date )
        {
            try {
                IFormatProvider culture = new CultureInfo( "en-US", true );
                DateTime dateVal = DateTime.ParseExact( DateTime.Today.ToString( "yyyy-MM-dd" ), "yyyy-MM-dd", culture );
                db = new CommitmentDBEntities();
                var request = db.AlertRequests.Where( x => x.case_id == case_id ).FirstOrDefault();
                if(request != null ) {

                    // save old data
                    //AlertRequestBackup backup = new AlertRequestBackup {
                    //    case_id = request.case_id,
                    //    userId = request.userId,
                    //    start_date = request.start_date,
                    //    end_date = request.end_date,
                    //    due_date = request.due_date,
                    //    alert_frequence = request.alert_frequence,
                    //    alert_frequence_periodicity = request.alert_frequence_periodicity,
                    //    action_frequence = request.action_frequence,
                    //    alert_action_periodicity = request.alert_action_periodicity,
                    //    status = request.status,
                    //    executed_by = request.executed_by,
                    //    decisions = request.decisions,
                    //    decision_id = request.decision_id,
                    //    due_date_action = request.due_date_action,
                    //    end_date_action = request.end_date_action,
                    //    application_id = request.application_id,
                    //    triggered_date = request.triggered_date,
                    //    created_at = request.created_at
                    //};

                    //// save
                    //db.AlertRequestBackups.Add(backup);
                    //db.SaveChanges();

                    // update request
                    //request.start_date = request.start_date.Value.AddDays( frequence );
                    request.start_date = start_date;


                    db.Entry( request ).State = EntityState.Modified;
                    int res = db.SaveChanges();
                    if(res > 0 ) {
                        return true;
                    }
                }
                return false;
            } catch(Exception ex ) {
                ErrorLog.LogWrite( "Error message: " + ex.Message );
                return false;
            }
        }

        // log the original request
        public static void SaveOriginalRequest(AlertRequest request )
        {
            try {
                db.AlertRequestBackups.Add( new AlertRequestBackup {
                    case_id = request.case_id,
                    userId = request.userId,
                    start_date = request.start_date,
                    end_date = request.end_date,
                    due_date = request.due_date,
                    alert_frequence = request.alert_frequence,
                    alert_frequence_periodicity = request.alert_frequence_periodicity,
                    action_frequence = request.action_frequence,
                    alert_action_periodicity = request.alert_action_periodicity,
                    status = request.status,
                    executed_by = request.executed_by,
                    decisions = request.decisions,
                    decision_id = request.decision_id,
                    due_date_action = request.due_date_action,
                    end_date_action = request.end_date_action,
                    application_id = request.application_id,
                    triggered_date = request.triggered_date,
                    created_at = request.created_at
                } );
                db.SaveChanges();
                // log
                LogWriter.LogWrite( "Action: Job saving the original request with the following details: \n" + "Case ID: " + request.case_id + "\nTriggered Date: " + request.triggered_date + "\nEnd Action date: " + request.end_date_action + "\n" );


            }
            catch (Exception ex ) {
                ErrorLog.LogWrite( "Error message: " + ex.Message );
            }
        }

        // check whether the original request exists before generating a new request
        public static bool DoesOriginalRequestExists(int case_id, DateTime? triggered_date, DateTime? end_action_date )
        {
            try {
                db = new CommitmentDBEntities();

                // log
                LogWriter.LogWrite("Action: Job checking the existence of the request with the following details: \n" + "Case ID: " + case_id + "\nTriggered Date: " + triggered_date + "\nEnd Action date: " + end_action_date + "\n");

                return ( db.AlertRequestBackups.Where( x => x.case_id == case_id && x.triggered_date == triggered_date && x.end_date_action == end_action_date ).FirstOrDefault() != null ) ? true : false;
            } catch(Exception ex ) {
                ErrorLog.LogWrite( "Error message from DoesOriginalRequestExists method: " + ex.Message );
                return false;
            }
        }

        // check whether the mail has been already sent for the day
        // parameters case_id start_date end_date financing_type_name customer_no
        public static bool HasEmailBeenSent(int case_id, string financing_type_name, int customer_no, DateTime? start_date, DateTime? end_date )
        {
            try {
                db = new CommitmentDBEntities();

                // log
                LogWriter.LogWrite( "Action: Job checking the existence of the email info with the following details: \n" + "Case ID: " + case_id + "\nAlert start Date: " + start_date + "\nEAlert end date: " + end_date + "\n" );
                //var 
                //return ( db.EmailSents.Where( x => x.case_id == case_id && x.financing_type_name == financing_type_name && x.start_date == start_date && x.end_date == end_date ).FirstOrDefault() != null ) ? true : false;
                return ( db.EmailSents.Where( x => x.case_id == case_id && x.financing_type_name == financing_type_name && x.start_date == start_date && x.end_date == end_date ).ToList().Count >= 1 ) ? true : false;
            } catch(Exception ex ) {
                ErrorLog.LogWrite( "Error message from HasEmailBeenSent method: " + ex.Message );
                return false;
            }
        }

        // log request detail after email send
        public static void SaveEmailInfoAFterEmailSent(Request request )
        {

            try {
                db = new CommitmentDBEntities();
                IFormatProvider culture = new CultureInfo( "en-US", true );
                EmailSent email = new EmailSent {
                    start_date = request.start_date,
                    //end_date = DateTime.ParseExact( request.end_date, "yyyy-MM-dd", culture ),
                    end_date = request.end_date,
                    alert_frequence = request.alert_frequence,
                    alert_frequence_periodicity = request.alert_frequence_periodicity,
                    decisions = request.decisions,
                    fullname = request.fullname,
                    email = request.email,
                    responsable_email = request.responsable_email,
                    tracking_officer = request.tracking_officer,
                    decision_desc = request.decision_desc,
                    financing_type_name = request.financing_type_name,
                    loan_description = request.loan_description,
                    loan_amount = request.loan_amount,
                    customer_no = request.customer_no,
                    customer_fullname = request.customer_fullname,
                    case_id = request.case_id
                };

                db.SaveChanges();
                // log
                LogWriter.LogWrite( "Action: Job saving the email informations after sending the mail with the following details: \n" + "Case ID: " + request.case_id + "\nAlert start Date: " + request.start_date + "\nAlert end date: " + request.end_date + "\n" );
            }
            catch ( Exception ex ) {
                ErrorLog.LogWrite( "Error message from SaveEmailInfoAFterEmailSent method: " + ex.Message );
            }
        }
    }
}
