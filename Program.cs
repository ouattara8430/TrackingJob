using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TrackingJob.Log;
using TrackingJob.Model;
using TrackingJob.Repository;

namespace TrackingJob
{
    class Program
    {
        static void Main ( string[] args )
        {
            Worker worker = new Worker();
            Thread t = new Thread( worker.DoWork );
            t.IsBackground = true;
            t.Start();

            while ( true ) {

                worker.GenerateNewRequest();
                worker.SendAlertToAccountOfficer();
            }
            t.Join();
            //SendAlertToAccountOfficer();
        }

        

        // check whether the alert date happens
        
    }

    class Worker
    {
        public bool KeepGoing { get; set; }

        public Worker ()
        {
            KeepGoing = true;
        }

        public void DoWork ()
        {
            while ( KeepGoing ) {
                //Console.WriteLine( "Ding" );
                Thread.Sleep( 200 );
            }
        }

        // generate a new request
        public void GenerateNewRequest ()
        {
            // get the current date
            var current_date = DateTime.Now;
            // current day
            var current_day = current_date.Day;
            // month day
            var current_month = current_date.Month;
            // current day
            var current_year = current_date.Year;

            int frequence = 0;

            while ( KeepGoing ) {
                // run without stop condition
                List<AlertRequest> requests = AlertRepo.getRepetitiveRequests();

                // check whether the result is not empty
                if ( requests.Count != 0 ) {
                    foreach ( var request in requests ) {
                        if ( request.alert_action_periodicity != "" ) {
                            switch ( request.alert_action_periodicity ) {
                                case "Jour":
                                    try {
                                        frequence = Int32.Parse( request.action_frequence );
                                        // check whether the request has already been used 
                                        // fetch data from used request using the id
                                        if ( request.triggered_date.Value.Year.Equals( current_date.Year ) ) {
                                            if ( request.triggered_date.Value.Month.Equals( current_date.Month ) ) {
                                                if ( request.triggered_date.Value.Day.Equals( current_date.Day ) ) {
                                                    if ( current_date.AddDays( frequence ) <= request.end_date_action ) {
                                                        // update the request
                                                        AlertRequest alertRequest = new AlertRequest();
                                                        alertRequest.decision_id = request.decision_id;
                                                        alertRequest.userId = request.userId ?? 0;
                                                        alertRequest.executed_by = request.executed_by;
                                                        alertRequest.application_id = request.application_id;
                                                        alertRequest.decisions = request.decisions;
                                                        alertRequest.due_date_action = ( request.due_date_action.HasValue ) ? request.due_date_action.Value.AddDays( frequence ) : request.due_date_action = null;
                                                        //alertRequest.end_date_action = ( request.end_date_action.HasValue ) ? request.end_date_action.Value.AddYears( frequence ) : request.end_date_action = null;
                                                        alertRequest.triggered_date = ( request.triggered_date.HasValue ) ? request.triggered_date.Value.AddDays( frequence ) : request.triggered_date = null;
                                                        alertRequest.start_date = ( request.start_date.HasValue ) ? request.start_date.Value.AddDays( frequence ) : request.start_date = null;
                                                        alertRequest.end_date = ( request.end_date.HasValue ) ? request.end_date.Value.AddDays( frequence ) : request.end_date = null;
                                                        alertRequest.alert_frequence = request.alert_frequence;
                                                        alertRequest.alert_frequence_periodicity = request.alert_frequence_periodicity;
                                                        alertRequest.action_frequence = request.action_frequence;
                                                        alertRequest.alert_action_periodicity = request.alert_action_periodicity;
                                                        alertRequest.created_at = DateTime.Now;
                                                        alertRequest.status = false;
                                                        bool saved = AlertRepo.GenerateNewRequest( alertRequest );



                                                        // check whether the same data has been already checked before saving a new request
                                                        if ( AlertRepo.DoesOriginalRequestExists( request.case_id, request.triggered_date, request.end_date_action ) ) {
                                                            Console.WriteLine( "Request already generated on " + request.created_at );
                                                        }
                                                        else {
                                                            // generate request
                                                            bool save = AlertRepo.GenerateNewRequest( alertRequest );

                                                            if ( save ) {
                                                                AlertRepo.SaveOriginalRequest( request );
                                                                LogWriter.LogWrite( "Action: Request generated: \n" + "Case ID: " + request.case_id + "\nTriggered Date: " + request.triggered_date + "\nEnd Action date: " + request.end_date_action + "\n" );
                                                                Console.WriteLine( "Request generated for the year " + request.triggered_date.Value.Year + " - " + "due date action: " + request.due_date_action + " - " + "end date action: " + request.end_date_action );
                                                            }
                                                            else {
                                                                LogWriter.LogWrite( "Action: Request not generated: \n" + "Case ID: " + request.case_id + "\nTriggered Date: " + request.triggered_date + "\nEnd Action date: " + request.end_date_action + "\n" );
                                                                Console.WriteLine( "Aucune requete a generer pour cette annee" );
                                                            }
                                                        }

                                                    }
                                                }
                                            }
                                        }
                                    }
                                    catch ( Exception ex ) {
                                        ErrorLog.LogWrite( "Error message: " + ex.Message );
                                    }
                                    break;

                                case "Semaine":
                                    try {
                                        int week = 7;
                                        frequence = Int32.Parse( request.action_frequence );
                                        int days = frequence * week;

                                        if ( request.triggered_date.Value.Year.Equals( current_date.Year ) ) {
                                            if ( request.triggered_date.Value.Month.Equals( current_date.Month ) ) {
                                                if ( request.triggered_date.Value.Day.Equals( current_date.Day ) ) {
                                                    if ( current_date.AddDays( days ) <= request.end_date_action ) {
                                                        // update the request
                                                        AlertRequest alertRequest = new AlertRequest();
                                                        alertRequest.decision_id = request.decision_id;
                                                        alertRequest.userId = request.userId ?? 0;
                                                        alertRequest.executed_by = request.executed_by;
                                                        alertRequest.application_id = request.application_id;
                                                        alertRequest.decisions = request.decisions;
                                                        alertRequest.due_date_action = ( request.due_date_action.HasValue ) ? request.due_date_action.Value.AddDays( days ) : request.due_date_action = null;
                                                        alertRequest.end_date_action = request.end_date_action;
                                                        alertRequest.triggered_date = ( request.triggered_date.HasValue ) ? request.triggered_date.Value.AddDays( days ) : request.triggered_date = null;
                                                        alertRequest.start_date = ( request.start_date.HasValue ) ? request.start_date.Value.AddDays( days ) : request.start_date = null;
                                                        alertRequest.end_date = ( request.end_date.HasValue ) ? request.end_date.Value.AddDays( days ) : request.end_date = null;
                                                        alertRequest.alert_frequence = request.alert_frequence;
                                                        alertRequest.alert_frequence_periodicity = request.alert_frequence_periodicity;
                                                        alertRequest.action_frequence = request.action_frequence;
                                                        alertRequest.alert_action_periodicity = request.alert_action_periodicity;
                                                        alertRequest.created_at = DateTime.Now;
                                                        alertRequest.status = false;


                                                        // check whether the same data has been already checked before saving a new request
                                                        if ( AlertRepo.DoesOriginalRequestExists( request.case_id, request.triggered_date, request.end_date_action ) ) {
                                                            Console.WriteLine( "Request already generated on " + request.created_at );
                                                        }
                                                        else {
                                                            // generate request
                                                            bool saved = AlertRepo.GenerateNewRequest( alertRequest );

                                                            if ( saved ) {
                                                                AlertRepo.SaveOriginalRequest( request );
                                                                LogWriter.LogWrite( "Action: Request generated: \n" + "Case ID: " + request.case_id + "\nTriggered Date: " + request.triggered_date + "\nEnd Action date: " + request.end_date_action + "\n" );
                                                                Console.WriteLine( "Request generated for the year " + request.triggered_date.Value.Year + " - " + "due date action: " + request.due_date_action + " - " + "end date action: " + request.end_date_action );
                                                            }
                                                            else {
                                                                LogWriter.LogWrite( "Action: Request not generated: \n" + "Case ID: " + request.case_id + "\nTriggered Date: " + request.triggered_date + "\nEnd Action date: " + request.end_date_action + "\n" );
                                                                Console.WriteLine( "Aucune requete a generer pour cette annee" );
                                                            }
                                                        }

                                                    }
                                                }
                                            }
                                        }
                                    }
                                    catch ( Exception ex ) {
                                        ErrorLog.LogWrite( "Error message: " + ex.Message );
                                    }
                                    break;

                                case "Mois":
                                    try {
                                        frequence = Int32.Parse( request.action_frequence );

                                        if ( request.triggered_date.Value.Year.Equals( current_date.Year ) ) {
                                            if ( request.triggered_date.Value.Month.Equals( current_date.Month ) ) {
                                                if ( request.triggered_date.Value.Day.Equals( current_date.Day ) ) {
                                                    if ( current_date.AddMonths( frequence ) <= request.end_date_action ) {
                                                        // update the request
                                                        AlertRequest alertRequest = new AlertRequest();
                                                        alertRequest.decision_id = request.decision_id;
                                                        alertRequest.userId = request.userId ?? 0;
                                                        alertRequest.executed_by = request.executed_by;
                                                        alertRequest.application_id = request.application_id;
                                                        alertRequest.decisions = request.decisions;
                                                        alertRequest.due_date_action = ( request.due_date_action.HasValue ) ? request.due_date_action.Value.AddMonths( frequence ) : request.due_date_action = null;
                                                        alertRequest.end_date_action = request.end_date_action;
                                                        alertRequest.triggered_date = ( request.triggered_date.HasValue ) ? request.triggered_date.Value.AddMonths( frequence ) : request.triggered_date = null;
                                                        alertRequest.start_date = ( request.start_date.HasValue ) ? request.start_date.Value.AddMonths( frequence ) : request.start_date = null;
                                                        alertRequest.end_date = ( request.end_date.HasValue ) ? request.end_date.Value.AddMonths( frequence ) : request.end_date = null;
                                                        alertRequest.alert_frequence = request.alert_frequence;
                                                        alertRequest.alert_frequence_periodicity = request.alert_frequence_periodicity;
                                                        alertRequest.action_frequence = request.action_frequence;
                                                        alertRequest.alert_action_periodicity = request.alert_action_periodicity;
                                                        alertRequest.created_at = DateTime.Now;
                                                        alertRequest.status = false;

                                                        // check whether the same data has been already checked before saving a new request
                                                        if ( AlertRepo.DoesOriginalRequestExists( request.case_id, request.triggered_date, request.end_date_action ) ) {
                                                            Console.WriteLine( "Request already generated on " + request.created_at );
                                                        }
                                                        else {
                                                            // generate request
                                                            bool saved = AlertRepo.GenerateNewRequest( alertRequest );

                                                            if ( saved ) {
                                                                AlertRepo.SaveOriginalRequest( request );
                                                                LogWriter.LogWrite( "Action: Request generated: \n" + "Case ID: " + request.case_id + "\nTriggered Date: " + request.triggered_date + "\nEnd Action date: " + request.end_date_action + "\n" );
                                                                Console.WriteLine( "Request generated for the year " + request.triggered_date.Value.Year + " - " + "due date action: " + request.due_date_action + " - " + "end date action: " + request.end_date_action );
                                                            }
                                                            else {
                                                                LogWriter.LogWrite( "Action: Request not generated: \n" + "Case ID: " + request.case_id + "\nTriggered Date: " + request.triggered_date + "\nEnd Action date: " + request.end_date_action + "\n" );
                                                                Console.WriteLine( "Aucune requete a generer pour cette annee" );
                                                            }
                                                        }

                                                    }
                                                }
                                            }
                                        }
                                    }
                                    catch ( Exception ex ) {
                                        ErrorLog.LogWrite( "Error message: " + ex.Message );
                                    }
                                    break;

                                case "Annee":
                                    try {
                                        frequence = Int32.Parse( request.action_frequence );

                                        // check whether triggered date is equal to current date
                                        if ( request.triggered_date.Value.Year.Equals( current_date.Year ) ) {
                                            if ( request.triggered_date.Value.Month.Equals( current_date.Month ) ) {
                                                if ( request.triggered_date.Value.Day.Equals( current_date.Day ) ) {
                                                    if ( current_date.AddYears( frequence ) <= request.end_date_action ) {
                                                        // update the request
                                                        AlertRequest alertRequest = new AlertRequest();
                                                        alertRequest.decision_id = request.decision_id;
                                                        alertRequest.userId = request.userId ?? 0;
                                                        alertRequest.executed_by = request.executed_by;
                                                        alertRequest.application_id = request.application_id;
                                                        alertRequest.decisions = request.decisions;
                                                        alertRequest.due_date_action = ( request.due_date_action.HasValue ) ? request.due_date_action.Value.AddYears( frequence ) : request.due_date_action = null;
                                                        alertRequest.end_date_action = request.end_date_action;
                                                        alertRequest.triggered_date = ( request.triggered_date.HasValue ) ? request.triggered_date.Value.AddYears( frequence ) : request.triggered_date = null;
                                                        alertRequest.start_date = ( request.start_date.HasValue ) ? request.start_date.Value.AddYears( frequence ) : request.start_date = null;
                                                        alertRequest.end_date = ( request.end_date.HasValue ) ? request.end_date.Value.AddYears( frequence ) : request.end_date = null;
                                                        alertRequest.alert_frequence = request.alert_frequence;
                                                        alertRequest.alert_frequence_periodicity = request.alert_frequence_periodicity;
                                                        alertRequest.action_frequence = request.action_frequence;
                                                        alertRequest.alert_action_periodicity = request.alert_action_periodicity;
                                                        alertRequest.created_at = DateTime.Now;
                                                        alertRequest.status = false;


                                                        // check whether the same data has been already checked before saving a new request
                                                        if ( AlertRepo.DoesOriginalRequestExists( request.case_id, request.triggered_date, request.end_date_action ) ) {
                                                            Console.WriteLine( "Request already generated on " + request.created_at );
                                                        }
                                                        else {
                                                            // generate request
                                                            bool saved = AlertRepo.GenerateNewRequest( alertRequest );

                                                            if ( saved ) {
                                                                AlertRepo.SaveOriginalRequest( request );
                                                                LogWriter.LogWrite( "Action: Request generated: \n" + "Case ID: " + request.case_id + "\nTriggered Date: " + request.triggered_date + "\nEnd Action date: " + request.end_date_action + "\n" );
                                                                Console.WriteLine( "Request generated for the year " + request.triggered_date.Value.Year + " - " + "due date action: " + request.due_date_action + " - " + "end date action: " + request.end_date_action );
                                                            }
                                                            else {
                                                                LogWriter.LogWrite( "Action: Request not generated: \n" + "Case ID: " + request.case_id + "\nTriggered Date: " + request.triggered_date + "\nEnd Action date: " + request.end_date_action + "\n" );
                                                                Console.WriteLine( "Aucune requete a generer pour cette annee" );
                                                            }
                                                        }

                                                    }
                                                }
                                            }
                                        }

                                    }
                                    catch ( Exception ex ) {
                                        ErrorLog.LogWrite( "Error message: " + ex.Message );
                                    }
                                    break;

                            }
                        }
                        else {
                            Console.WriteLine( "Aucune requete a generer pour l'instant" );
                        }
                    }
                }
                else {
                    Console.WriteLine( "Pas de donnees pour cette date" );
                }

                Thread.Sleep( 200 );
            }

            Console.ReadLine();

        }

        // send email
        public void SendAlertToAccountOfficer ()
        {
            IFormatProvider culture = new CultureInfo( "en-US", true );
            var current_date = DateTime.Today;
            // fetch all the request whose alert start date is today
            // check the frequency and the period for sending the alert
            // after checking, send the alert by mail
            // update the start_date to the next date and check the end of the alert or whether the document has been uploaded

            IEnumerable<FetchRequests_Result> results = AlertRepo.FetchRequest();

            List<Request> requests = new List<Request>();



            // check whether the request is not empty
            if ( !results.ToList().Count.Equals( 0 ) ) {
                foreach ( var item in results ) {
                    Request request = new Request {
                        start_date = item.start_date,
                        end_date = item.end_date,
                        alert_frequence = item.alert_frequence,
                        alert_frequence_periodicity = item.alert_frequence_periodicity,
                        decisions = item.decisions,
                        fullname = item.fullname,
                        email = item.email,
                        responsable_email = item.responsable_email,
                        decision_desc = item.decision_desc,
                        financing_type_name = item.financing_type_name,
                        loan_description = item.loan_description,
                        loan_amount = item.loan_amount,
                        customer_no = item.customer_no,
                        customer_fullname = item.customer_fullname,
                        case_id = item.case_id
                    };

                    // frequence number
                    int frequence = 0;

                    // check the frequence
                    switch ( request.alert_frequence_periodicity ) {
                        case "Jour":

                            //DateTime dateVal = DateTime.ParseExact( request.start_date, "yyyy-MM-dd", culture );
                            DateTime dateVal = ( DateTime ) request.start_date;
                            frequence = Int32.Parse( request.alert_frequence );

                            // check whether the mail has been already sent for the day
                            //if ( AlertRepo.HasEmailBeenSent( request.case_id, request.financing_type_name, request.customer_no, DateTime.ParseExact( request.start_date, "yyyy-MM-dd", culture ), request.end_date ) ) {
                            if ( AlertRepo.HasEmailBeenSent( request.case_id, request.financing_type_name, request.customer_no ?? 0, request.start_date, request.end_date ) ) {
                                Console.WriteLine( "Email has already been sent to " + request.email );
                            }
                            else {
                                // fetch the request whose detail has been logged and the current date is now

                                if ( dateVal == current_date && dateVal <= request.end_date ) {
                                    // send email to account officer
                                    //SendEmail ( string to, string subject, string message )
                                    string mail = Mailer.SendEmail( "gnougo2009@gmail.com", "SUIVI DES ENGAGEMENTS", request.decisions );
                                    //if ( mail.Equals( mail ) ) {
                                    if ( true ) {

                                        LogWriter.LogWrite( "Email sent successfully to " + request.fullname + " with the following email: " + request.email + " at " + DateTime.Now );
                                        // log email info
                                        AlertRepo.SaveEmailInfoAFterEmailSent( request );

                                        // update
                                        bool updated = AlertRepo.UpdateRequest( request.case_id, dateVal.AddDays( frequence ) );
                                        if ( updated ) {
                                            Console.WriteLine( "Frequence jour effectuee avec succes" );
                                        }
                                    }
                                }
                            }
                            break;
                        case "Semaine":

                            int week = 7;
                            DateTime dateVal_w = ( DateTime ) request.start_date;
                            frequence = Int32.Parse( request.alert_frequence );
                            int days = frequence * week;

                            // check whether the mail has been already sent for the day
                            if ( AlertRepo.HasEmailBeenSent( request.case_id, request.financing_type_name, request.customer_no ?? 0, request.start_date, request.end_date ) ) {
                                Console.WriteLine( "Email has already been sent to " + request.email );
                            }
                            else {
                                // fetch the request whose detail has been logged and the current date is now

                                if ( dateVal_w == current_date && dateVal_w <= request.end_date ) {
                                    // send email to account officer
                                    //SendEmail ( string to, string subject, string message )
                                    string mail = Mailer.SendEmail( "gnougo2009@gmail.com", "SUIVI DES ENGAGEMENTS", request.decisions );
                                    //if ( mail.Equals( mail ) ) {
                                    if ( true ) {
                                        LogWriter.LogWrite( "Email sent successfully to " + request.fullname + " with the following email: " + request.email + " at " + DateTime.Now );
                                        // log email info
                                        AlertRepo.SaveEmailInfoAFterEmailSent( request );

                                        // update
                                        bool updated = AlertRepo.UpdateRequest( request.case_id, dateVal_w.AddDays( days ) );
                                        if ( updated ) {
                                            Console.WriteLine( "Frequence jour effectuee avec succes" );
                                        }
                                    }
                                }
                            }
                            break;
                        case "Mois":

                            DateTime dateVal_m = ( DateTime ) request.start_date;
                            frequence = Int32.Parse( request.alert_frequence );
                            //int days = frequence * week;

                            // check whether the mail has been already sent for the day
                            if ( AlertRepo.HasEmailBeenSent( request.case_id, request.financing_type_name, request.customer_no ?? 0, request.start_date, request.end_date ) ) {
                                Console.WriteLine( "Email has already been sent to " + request.email );
                            }
                            else {
                                // fetch the request whose detail has been logged and the current date is now

                                if ( dateVal_m == current_date && dateVal_m <= request.end_date ) {
                                    // send email to account officer
                                    //SendEmail ( string to, string subject, string message )
                                    string mail = Mailer.SendEmail( "gnougo2009@gmail.com", "SUIVI DES ENGAGEMENTS", request.decisions );
                                    //if ( mail.Equals( mail ) ) {
                                    if ( true ) {
                                        LogWriter.LogWrite( "Email sent successfully to " + request.fullname + " with the following email: " + request.email + " at " + DateTime.Now );
                                        // log email info
                                        AlertRepo.SaveEmailInfoAFterEmailSent( request );

                                        // update
                                        bool updated = AlertRepo.UpdateRequest( request.case_id, dateVal_m.AddMonths( frequence ) );
                                        if ( updated ) {
                                            Console.WriteLine( "Frequence jour effectuee avec succes" );
                                        }
                                    }
                                }
                            }
                            break;
                        case "Annee":

                            DateTime dateVal_y = ( DateTime ) request.start_date;
                            frequence = Int32.Parse( request.alert_frequence );
                            //int days = frequence * week;

                            // check whether the mail has been already sent for the day
                            if ( AlertRepo.HasEmailBeenSent( request.case_id, request.financing_type_name, request.customer_no ?? 0, request.start_date, request.end_date ) ) {
                                Console.WriteLine( "Email has already been sent to " + request.email );
                            }
                            else {
                                // fetch the request whose detail has been logged and the current date is now

                                if ( dateVal_y == current_date && dateVal_y <= request.end_date ) {
                                    // send email to account officer
                                    //SendEmail ( string to, string subject, string message )
                                    string mail = Mailer.SendEmail( "gnougo2009@gmail.com", "SUIVI DES ENGAGEMENTS", request.decisions );
                                    //if ( mail.Equals( mail ) ) {
                                    if ( true ) {
                                        LogWriter.LogWrite( "Email sent successfully to " + request.fullname + " with the following email: " + request.email + " at " + DateTime.Now );
                                        // log email info
                                        AlertRepo.SaveEmailInfoAFterEmailSent( request );

                                        // update
                                        bool updated = AlertRepo.UpdateRequest( request.case_id, dateVal_y.AddYears( frequence ) );
                                        if ( updated ) {
                                            Console.WriteLine( "Frequence jour effectuee avec succes" );
                                        }
                                    }
                                }
                            }
                            break;
                    }
                }
            }
            else {
                Console.WriteLine( "Aucun mail n'a ete envoye au gestionnaire" );
            }

            Console.ReadLine();
        }
    }
}
