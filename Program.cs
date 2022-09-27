using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
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
            //Thread t = new Thread( worker.DoWork );
            //t.IsBackground = true;
            //t.Start();

            //while ( true ) {

            //    worker.Run();
            //}
            //t.Join();
            worker.SendAlertToAccountOfficer();
        }



        // check whether the alert date happens

    }

    class Worker
    {
        public bool KeepGoing { get; set; }

        // constructor
        public Worker ()
        {
            KeepGoing = true;
        }

        // do work
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

            //while ( KeepGoing ) {
                // run without stop condition
                List<AlertRequest> requests = AlertRepo.getRepetitiveRequests();

                // check whether the result is not empty
                if ( requests.Count != 0 ) {
                    foreach ( var request in requests ) {
                        if ( request.alert_action_periodicity != "" ) {
                            switch ( request.alert_action_periodicity ) {
                                case "Jour":
                                try {
                                    // log information
                                    LogAction.LogWrite( "Request details => \n" + "Case ID: " + request.case_id + "\nAction Period: " + request.alert_action_periodicity + "\nAction frequence: " + request.action_frequence + "\nTriggered date: " + request.triggered_date + "\nCurrent date: " + current_date.AddDays( frequence ) + "\nAction end date: " + request.end_date_action );
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
                                                                LogAction.LogWrite( "Action: Request generated: \n" + "Case ID: " + request.case_id + "\nTriggered Date: " + request.triggered_date + "\nEnd Action date: " + request.end_date_action + "\n" );
                                                                Console.WriteLine( "Request generated for the year " + request.triggered_date.Value.Year + " - " + "due date action: " + request.due_date_action + " - " + "end date action: " + request.end_date_action );
                                                            }
                                                            else {
                                                                LogAction.LogWrite( "Action: Request not generated: \n" + "Case ID: " + request.case_id + "\nTriggered Date: " + request.triggered_date + "\nEnd Action date: " + request.end_date_action + "\n" );
                                                                Console.WriteLine( "Aucune requete a generer pour cette annee" );
                                                            }
                                                        }

                                                    }
                                                    else {
                                                        Console.WriteLine( "la condition du jour d'execution n'est pas satisfaite " );
                                                    }
                                            }
                                            else {
                                                Console.WriteLine( "la condition du jour de declenchement n'est pas satisfaite pour l'execution" );
                                            }
                                        }
                                        else {
                                            Console.WriteLine( "la condition du mois de declenchement n'est pas satisfaite pour l'execution" );
                                        }
                                    }
                                    else {
                                        Console.WriteLine( "la condition de l'annee de declenchement n'est pas satisfaite pour l'execution" );
                                    }
                                }
                                    catch ( Exception ex ) {
                                        ErrorLog.LogWrite( "Error message: " + ex.Message );
                                    }
                                    break;

                                case "Semaine":
                                    try {
                                    // log information
                                    LogAction.LogWrite( "Request details => \n" + "Case ID: " + request.case_id + "\nAction Period: " + request.alert_action_periodicity + "\nAction frequence: " + request.action_frequence + "\nTriggered date: " + request.triggered_date + "\nCurrent date: " + current_date.AddDays( frequence ) + "\nAction end date: " + request.end_date_action );
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
                                                                LogAction.LogWrite( "Action: Request generated: \n" + "Case ID: " + request.case_id + "\nTriggered Date: " + request.triggered_date + "\nEnd Action date: " + request.end_date_action + "\n" );
                                                                Console.WriteLine( "Request generated for the year " + request.triggered_date.Value.Year + " - " + "due date action: " + request.due_date_action + " - " + "end date action: " + request.end_date_action );
                                                            }
                                                            else {
                                                                LogAction.LogWrite( "Action: Request not generated: \n" + "Case ID: " + request.case_id + "\nTriggered Date: " + request.triggered_date + "\nEnd Action date: " + request.end_date_action + "\n" );
                                                                Console.WriteLine( "Aucune requete a generer pour cette annee" );
                                                            }
                                                        }

                                                    }
                                                else {
                                                    Console.WriteLine( "la condition du jour d'execution n'est pas satisfaite " );
                                                }
                                            }
                                            else {
                                                Console.WriteLine( "la condition du jour de declenchement n'est pas satisfaite pour l'execution" );
                                            }
                                        }
                                        else {
                                            Console.WriteLine( "la condition du mois de declenchement n'est pas satisfaite pour l'execution" );
                                        }
                                    }
                                    else {
                                        Console.WriteLine( "la condition de l'annee de declenchement n'est pas satisfaite pour l'execution" );
                                    }
                                }
                                    catch ( Exception ex ) {
                                        ErrorLog.LogWrite( "Error message: " + ex.Message );
                                    }
                                    break;

                                case "Mois":
                                    try {
                                    // log information
                                    LogAction.LogWrite( "Request details => \n" + "Case ID: " + request.case_id + "\nAction Period: " + request.alert_action_periodicity + "\nAction frequence: " + request.action_frequence + "\nTriggered date: " + request.triggered_date + "\nCurrent date: " + current_date.AddDays( frequence ) + "\nAction end date: " + request.end_date_action );
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
                                                                LogAction.LogWrite( "Action: Request generated: \n" + "Case ID: " + request.case_id + "\nTriggered Date: " + request.triggered_date + "\nEnd Action date: " + request.end_date_action + "\n" );
                                                                Console.WriteLine( "Request generated for the year " + request.triggered_date.Value.Year + " - " + "due date action: " + request.due_date_action + " - " + "end date action: " + request.end_date_action );
                                                            }
                                                            else {
                                                                LogAction.LogWrite( "Action: Request not generated: \n" + "Case ID: " + request.case_id + "\nTriggered Date: " + request.triggered_date + "\nEnd Action date: " + request.end_date_action + "\n" );
                                                                Console.WriteLine( "Aucune requete a generer pour cette annee" );
                                                            }
                                                        }

                                                    }
                                                else {
                                                    Console.WriteLine( "la condition du jour d'execution n'est pas satisfaite " );
                                                }
                                            }
                                            else {
                                                Console.WriteLine( "la condition du jour de declenchement n'est pas satisfaite pour l'execution" );
                                            }
                                        }
                                        else {
                                            Console.WriteLine( "la condition du mois de declenchement n'est pas satisfaite pour l'execution" );
                                        }
                                    }
                                    else {
                                        Console.WriteLine( "la condition de l'annee de declenchement n'est pas satisfaite pour l'execution" );
                                    }
                                }
                                    catch ( Exception ex ) {
                                        ErrorLog.LogWrite( "Error message: " + ex.Message );
                                    }
                                    break;

                                case "Annee":
                                    try {
                                    // log information
                                    LogAction.LogWrite( "Request details => \n" + "Case ID: " + request.case_id + "\nAction Period: " + request.alert_action_periodicity + "\nAction frequence: " + request.action_frequence + "\nTriggered date: " + request.triggered_date + "\nCurrent date: " + current_date.AddDays( frequence ) + "\nAction end date: " + request.end_date_action );
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
                                                                LogAction.LogWrite( "Action: Request generated: \n" + "Case ID: " + request.case_id + "\nTriggered Date: " + request.triggered_date + "\nEnd Action date: " + request.end_date_action + "\n" );
                                                                Console.WriteLine( "Request generated for the year " + request.triggered_date.Value.Year + " - " + "due date action: " + request.due_date_action + " - " + "end date action: " + request.end_date_action );
                                                            }
                                                            else {
                                                                LogAction.LogWrite( "Action: Request not generated: \n" + "Case ID: " + request.case_id + "\nTriggered Date: " + request.triggered_date + "\nEnd Action date: " + request.end_date_action + "\n" );
                                                                Console.WriteLine( "Aucune requete a generer pour cette annee" );
                                                            }
                                                        }

                                                    }
                                                else {
                                                    Console.WriteLine( "la condition du jour d'execution n'est pas satisfaite " );
                                                }
                                            }
                                            else {
                                                Console.WriteLine( "la condition du jour de declenchement n'est pas satisfaite pour l'execution" );
                                            }
                                        }
                                        else {
                                            Console.WriteLine( "la condition du mois de declenchement n'est pas satisfaite pour l'execution" );
                                        }
                                    }
                                    else {
                                        Console.WriteLine( "la condition de l'annee de declenchement n'est pas satisfaite pour l'execution" );
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

            //    Thread.Sleep( 200 );
            //}

            //Console.ReadLine();

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

            string email_body = string.Empty;
            string path = string.Empty;
            string mail = string.Empty;
            bool condition;

            IEnumerable<FetchRequests_Result> results = AlertRepo.FetchRequest();

            List<Request> requests = new List<Request>();


            // check whether the request is not empty
            if ( !results.ToList().Count.Equals( 0 ) ) {
                foreach ( var item in results ) {
                    Request request = new Request {
                        //start_date = item.start_date,
                        application_id = item.application_id,
                        next_alert_date = item.next_alert_date,
                        nb_occurence = item.nb_occurence,
                        next_action_date = item.next_action_date,
                        nb_occurence_action = item.nb_occurence_action,
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
                        case_id = item.case_id,
                        status = item.status,
                        triggered_date = item.triggered_date,
                        end_date_action = item.end_date_action,
                        due_date_action = item.due_date_action,
                        CODE_AGENCE = item.CODE_AGENCE,
                        NUMERO_COMPTE = item.NUMERO_COMPTE,
                        CLE_COMPTE = item.CLE_COMPTE,
                        INTITULE_COMPTE = item.INTITULE_COMPTE,
                        SECTEUR_ACTIVITE = item.SECTEUR_ACTIVITE,
                        TYPE_ENGAGEMENT = item.TYPE_ENGAGEMENT,
                        DATE_MISE_EN_PLACE = item.DATE_MISE_EN_PLACE,
                        DATE_1ERE_ECHEANCE = item.DATE_1ERE_ECHEANCE,
                        DATE_FIN_ECHEANCE = item.DATE_FIN_ECHEANCE,
                        MONTANT_DEBLOQUE = item.MONTANT_DEBLOQUE,
                        ENCOURS_CREDIT = item.ENCOURS_CREDIT,
                        IMPAYES_CREDIT = item.IMPAYES_CREDIT,
                        PERIODICITE = item.PERIODICITE,
                        NBRE_ECHEANCE = item.NBRE_ECHEANCE,
                        GESTIONNAIRE = item.GESTIONNAIRE,
                        EMAIL_CLIENT = item.EMAIL_CLIENT,
                        GESTIONNAIRE_EMAIL = item.GESTIONNAIRE_EMAIL
                    };

                    // frequence number
                    int frequence = 0;

                    // check the frequence
                    switch ( request.alert_frequence_periodicity ) {
                        case "Jour":
                            try {
                                // log information
                                LogAlert.LogWrite( "Alert details => \n" + "Case ID: " + request.case_id + "\nStart date: " + request.start_date + "\nEnd date: " + request.end_date + "\nLoan type: " + request.financing_type_name + "\nCurrent date: " + current_date + "\nFrequence: " + request.alert_frequence + "\nPeriod: " + request.alert_frequence_periodicity + "\nNombre d'occurences d'alerte: " + request.nb_occurence);
                                //DateTime dateVal = DateTime.ParseExact( request.start_date, "yyyy-MM-dd", culture );
                                //DateTime dateVal = ( DateTime ) request.start_date;
                                DateTime dateVal = ( DateTime ) request.next_alert_date;
                                frequence = Int32.Parse( request.alert_frequence );
                                // get occurence total
                                int occurence_total = 0;
                                while(request.start_date < request.end_date)
                                {
                                    occurence_total++;
                                    request.start_date = request.start_date.Value.AddDays(frequence);
                                }

                                // check whether the mail has been already sent for the day
                                //if ( AlertRepo.HasEmailBeenSent( request.case_id, request.financing_type_name, request.customer_no, DateTime.ParseExact( request.start_date, "yyyy-MM-dd", culture ), request.end_date ) ) {
                                if ( AlertRepo.HasEmailBeenSent( request.case_id, request.financing_type_name, request.customer_no ?? 0, request.start_date, request.end_date ) ) {
                                    Console.WriteLine( "Email has already been sent to " + request.email );
                                }
                                else {
                                    // fetch the request whose detail has been logged and the current date is now

                                    //if ( dateVal == current_date && dateVal <= request.end_date ) {
                                    //if (!request.nb_occurence.Equals(0)) {
                                    if (request.nb_occurence > 0) {
                                        // send email to account officer
                                        // check whether the notif is before or after the due date
                                        if (request.end_date != null)
                                        {
                                            condition = dateVal == current_date && dateVal <= request.end_date;
                                        }
                                        else
                                        {
                                            condition = dateVal == current_date;
                                        }
                                        if (condition)
                                        {
                                            // check whether the action is late for procesing by the account officer
                                            if ((current_date > request.next_action_date || current_date > request.due_date_action) && request.status == false)
                                            {
                                                // send email
                                                // set the mail template
                                                path = ConfigurationManager.AppSettings["apres_echeance"].ToString();
                                                email_body = AlertRepo.SendToAccountOfficer("late", path, request, occurence_total);
                                                //mail = Mailer.SendEmail(request.email, "APRES ECHEANCE", request.decisions);
                                                mail = Mailer.SendEmailWithCopy(request.email, "APRES ECHEANCE POUR LE DOSSIER NO. " + request.application_id, email_body, request.responsable_email);
                                            }

                                            // check whether the action is only pending for processing by the account officer
                                            // check whether the end action date is not null
                                            if(request.triggered_date <= request.due_date_action & request.status == false)
                                            {
                                                // send email
                                                // set the mail template
                                                path = ConfigurationManager.AppSettings["avant_echeance"].ToString();
                                                email_body = AlertRepo.SendToAccountOfficer("pending", path, request, occurence_total);
                                                mail = Mailer.SendEmail(request.email, "AVANT ECHEANCE POUR LE DOSSIER NO. " + request.application_id, email_body);
                                            }
                                        }
                                        //if ( mail.Equals( mail ) ) {
                                        if (mail.Contains("email sent from ")) {

                                            LogAlert.LogWrite( "Email sent successfully to " + request.fullname + " with the following email: " + request.email + " at " + DateTime.Now );
                                            // log email info
                                            AlertRepo.SaveEmailInfoAFterEmailSent( request );

                                            // update
                                            // update the occurence
                                            // check the action due date
                                            

                                            bool updated = AlertRepo.UpdateRequest( request.case_id, dateVal.AddDays( frequence ) );
                                            if ( updated ) {
                                                Console.WriteLine( "Frequence jour effectuee avec succes" );
                                            }
                                        }
                                    }
                                }
                            } catch(Exception ex ) {
                                ErrorLog.LogWrite( "Error message: " + ex.Message );
                            }
                            break;
                        case "Semaine":
                            try {

                                // log information
                                LogAlert.LogWrite( "Alert details => \n" + "Case ID: " + request.case_id + "\nStart date: " + request.start_date + "\nEnd date: " + request.end_date + "\nLoan type: " + request.financing_type_name + "\nCurrent date: " + current_date + "\nFrequence: " + request.alert_frequence + "\nPeriod: " + request.alert_frequence_periodicity );
                                int week = 7;
                                DateTime dateVal_w = ( DateTime ) request.next_alert_date;
                                frequence = Int32.Parse( request.alert_frequence );
                                int days = frequence * week;
                                // get occurence total
                                int occurence_total = 0;
                                while (request.start_date < request.end_date)
                                {
                                    occurence_total++;
                                    request.start_date = request.start_date.Value.AddDays(days);
                                }

                                // check whether the mail has been already sent for the day
                                if ( AlertRepo.HasEmailBeenSent( request.case_id, request.financing_type_name, request.customer_no ?? 0, request.start_date, request.end_date ) ) {
                                    Console.WriteLine( "Email has already been sent to " + request.email );
                                }
                                else {
                                    if (request.nb_occurence > 0)
                                    {
                                        // fetch the request whose detail has been logged and the current date is now
                                        if (request.end_date != null)
                                        {
                                            condition = dateVal_w == current_date && dateVal_w <= request.end_date;
                                        }
                                        else
                                        {
                                            condition = dateVal_w == current_date;
                                        }
                                        if (condition)
                                        {
                                            // send email to account officer

                                            // check whether the action is late for procesing by the account officer
                                            if ((current_date > request.next_action_date || current_date > request.due_date_action) && request.status == false)
                                            {
                                                // send email
                                                // set the mail template
                                                path = ConfigurationManager.AppSettings["apres_echeance"].ToString();
                                                email_body = AlertRepo.SendToAccountOfficer("late", path, request, occurence_total);
                                                //mail = Mailer.SendEmail(request.email, "APRES ECHEANCE", request.decisions);
                                                mail = Mailer.SendEmailWithCopy(request.email, "APRES ECHEANCE POUR LE DOSSIER NO. " + request.application_id, email_body, request.responsable_email);
                                            }

                                            // check whether the action is only pending for processing by the account officer
                                            // check whether the end action date is not null
                                            if (request.triggered_date <= request.due_date_action & request.status == false)
                                            {
                                                // send email
                                                // set the mail template
                                                path = ConfigurationManager.AppSettings["avant_echeance"].ToString();
                                                email_body = AlertRepo.SendToAccountOfficer("pending", path, request, occurence_total);
                                                mail = Mailer.SendEmail(request.email, "AVANT ECHEANCE POUR LE DOSSIER NO. " + request.application_id, email_body);
                                            }
                                            //if ( mail.Equals( mail ) ) {
                                            if (mail.Contains("email sent from "))
                                            {
                                                LogAlert.LogWrite("Email sent successfully to " + request.fullname + " with the following email: " + request.email + " at " + DateTime.Now);
                                                // log email info
                                                AlertRepo.SaveEmailInfoAFterEmailSent(request);

                                                // update
                                                bool updated = AlertRepo.UpdateRequest(request.case_id, dateVal_w.AddDays(days));
                                                if (updated)
                                                {
                                                    Console.WriteLine("Frequence jour effectuee avec succes");
                                                }
                                            }
                                        }
                                    }
                                }
                            } catch(Exception ex ) {
                                ErrorLog.LogWrite( "Error message: " + ex.Message );
                            }
                            break;
                        case "Mois":
                            try {
                                // log information
                                LogAlert.LogWrite( "Alert details => \n" + "Case ID: " + request.case_id + "\nStart date: " + request.start_date + "\nEnd date: " + request.end_date + "\nLoan type: " + request.financing_type_name + "\nCurrent date: " + current_date + "\nFrequence: " + request.alert_frequence + "\nPeriod: " + request.alert_frequence_periodicity );
                                DateTime dateVal_m = ( DateTime ) request.next_alert_date;
                                frequence = Int32.Parse( request.alert_frequence );
                                //int days = frequence * week;

                                // get occurence total
                                int occurence_total = 0;
                                while (request.start_date < request.end_date)
                                {
                                    occurence_total++;
                                    request.start_date = request.start_date.Value.AddMonths(frequence);
                                }
                                // check whether the mail has been already sent for the day
                                if (AlertRepo.HasEmailBeenSent(request.case_id, request.financing_type_name, request.customer_no ?? 0, request.start_date, request.end_date)) {
                                    Console.WriteLine("Email has already been sent to " + request.email);
                                }
                                else {
                                    if (request.nb_occurence > 0)
                                    {
                                        // fetch the request whose detail has been logged and the current date is now
                                        if (request.end_date != null)
                                        {
                                            condition = dateVal_m == current_date && dateVal_m <= request.end_date;
                                        }
                                        else
                                        {
                                            condition = dateVal_m == current_date;
                                        }
                                        if (condition)
                                        {
                                            // send email to account officer

                                            // check whether the action is late for procesing by the account officer
                                            if ((current_date > request.next_action_date || current_date > request.due_date_action) && request.status == false)
                                            {
                                                // send email
                                                // set the mail template
                                                path = ConfigurationManager.AppSettings["apres_echeance"].ToString();
                                                email_body = AlertRepo.SendToAccountOfficer("late", path, request, occurence_total);
                                                //mail = Mailer.SendEmail(request.email, "APRES ECHEANCE", request.decisions);
                                                mail = Mailer.SendEmailWithCopy(request.email, "APRES ECHEANCE POUR LE DOSSIER NO. " + request.application_id, email_body, request.responsable_email);
                                            }

                                            // check whether the action is only pending for processing by the account officer
                                            // check whether the end action date is not null
                                            if (request.triggered_date <= request.due_date_action & request.status == false)
                                            {
                                                // send email
                                                // set the mail template
                                                path = ConfigurationManager.AppSettings["avant_echeance"].ToString();
                                                email_body = AlertRepo.SendToAccountOfficer("pending", path, request, occurence_total);
                                                mail = Mailer.SendEmail(request.email, "AVANT ECHEANCE POUR LE DOSSIER NO. " + request.application_id, email_body);
                                            }
                                            //if ( mail.Equals( mail ) ) {
                                            if (mail.Contains("email sent from "))
                                            {
                                                LogAlert.LogWrite("Email sent successfully to " + request.fullname + " with the following email: " + request.email + " at " + DateTime.Now);
                                                // log email info
                                                AlertRepo.SaveEmailInfoAFterEmailSent(request);

                                                // update
                                                bool updated = AlertRepo.UpdateRequest(request.case_id, dateVal_m.AddMonths(frequence));
                                                if (updated)
                                                {
                                                    Console.WriteLine("Frequence jour effectuee avec succes");
                                                }
                                            }
                                        }
                                    }
                                }
                            } catch(Exception ex ) {
                                ErrorLog.LogWrite( "Error message: " + ex.Message );
                            }
                            break;
                        case "Annee":
                            try {
                                // log information
                                LogAlert.LogWrite( "Alert details => \n" + "Case ID: " + request.case_id + "\nStart date: " + request.start_date + "\nEnd date: " + request.end_date + "\nLoan type: " + request.financing_type_name + "\nCurrent date: " + current_date + "\nFrequence: " + request.alert_frequence + "\nPeriod: " + request.alert_frequence_periodicity );
                                DateTime dateVal_y = ( DateTime ) request.next_alert_date;
                                frequence = Int32.Parse( request.alert_frequence );
                                //int days = frequence * week;

                                // get occurence total
                                int occurence_total = 0;
                                while (request.start_date < request.end_date)
                                {
                                    occurence_total++;
                                    request.start_date = request.start_date.Value.AddYears(frequence);
                                }
                                // check whether the mail has been already sent for the day
                                if ( AlertRepo.HasEmailBeenSent( request.case_id, request.financing_type_name, request.customer_no ?? 0, request.start_date, request.end_date ) ) {
                                    Console.WriteLine( "Email has already been sent to " + request.email );
                                }
                                else {
                                    if (request.nb_occurence > 0)
                                    {
                                        // fetch the request whose detail has been logged and the current date is now

                                        if (request.end_date != null)
                                        {
                                            condition = dateVal_y == current_date && dateVal_y <= request.end_date;
                                        }
                                        else
                                        {
                                            condition = dateVal_y == current_date;
                                        }
                                        if (condition)
                                        {
                                            // send email to account officer

                                            // check whether the action is late for procesing by the account officer
                                            if ((current_date > request.next_action_date || current_date > request.due_date_action) && request.status == false)
                                            {
                                                // send email
                                                // set the mail template
                                                path = ConfigurationManager.AppSettings["apres_echeance"].ToString();
                                                email_body = AlertRepo.SendToAccountOfficer("late", path, request, occurence_total);
                                                //mail = Mailer.SendEmail(request.email, "APRES ECHEANCE", request.decisions);
                                                mail = Mailer.SendEmailWithCopy(request.email, "APRES ECHEANCE POUR LE DOSSIER NO. " + request.application_id, email_body, request.responsable_email);
                                            }

                                            // check whether the action is only pending for processing by the account officer
                                            // check whether the end action date is not null
                                            if (request.triggered_date <= request.due_date_action & request.status == false)
                                            {
                                                // send email
                                                // set the mail template
                                                path = ConfigurationManager.AppSettings["avant_echeance"].ToString();
                                                email_body = AlertRepo.SendToAccountOfficer("pending", path, request, occurence_total);
                                                mail = Mailer.SendEmail(request.email, "AVANT ECHEANCE POUR LE DOSSIER NO. " + request.application_id, email_body);
                                            }
                                            //if ( mail.Equals( mail ) ) {
                                            if (mail.Contains("email sent from "))
                                            {
                                                LogAlert.LogWrite("Email sent successfully to " + request.fullname + " with the following email: " + request.email + " at " + DateTime.Now);
                                                // log email info
                                                AlertRepo.SaveEmailInfoAFterEmailSent(request);

                                                // update
                                                bool updated = AlertRepo.UpdateRequest(request.case_id, dateVal_y.AddYears(frequence));
                                                if (updated)
                                                {
                                                    Console.WriteLine("Frequence jour effectuee avec succes");
                                                }
                                            }
                                        }
                                    }
                                }
                            } catch(Exception ex ) {
                                ErrorLog.LogWrite( "Error message: " + ex.Message );
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

        // send notification to customers
        public void SendNotificationToCustomers ()
        {
            try {
                // fetch the list of customers whose the loan due date is issued
                // check the type of customer
                // check the periodicity of the loan type
                // send the sms or mail to the customers
                // log the information details after sending the sms or email and get the list later on for the test
                // close the connection and abort the system

                IFormatProvider culture = new CultureInfo( "en-US", true );
                DateTime dateVal = DateTime.ParseExact( DateTime.Today.ToString( "yyyy-MM-dd" ), "yyyy-MM-dd", culture );// make a while loop


                List<LoanApproved> loans = AlertRepo.FetchLoans();

                if(loans.Count != 0 ) {
                    foreach(var item in loans ) {
                        

                        // check the type of customer
                        if(item.SECTEUR_ACTIVITE.Equals( "PARTICULIER" ) ) {
                            // check periodicite -- ANNUELLE  MENSUELLE  TRIMESTRIELLE  SEMESTRIELLE
                            switch ( item.PERIODICITE ) {
                                case "MENSUELLE":
                                    // check whether the mail has already been sent
                                    if ( item.DATE_1ERE_ECHEANCE < dateVal ) {
                                        // send another type of email to the customer or set that it
                                        Console.WriteLine( "customer email: " + item.EMAIL_CLIENT + " ===> gestionnaire email: " + item.GESTIONNAIRE_EMAIL );
                                    }
                                    else {
                                        if( item.DATE_1ERE_ECHEANCE == dateVal ) {
                                            // check the type of loan and send the sms or email
                                            string email_body = string.Empty;
                                            string path = string.Empty;
                                            if ( item.SECTEUR_ACTIVITE.Contains( "TERME" ) ) {
                                                path = ConfigurationManager.AppSettings["traite_mail"].ToString();
                                            }
                                            else {
                                                path = ConfigurationManager.AppSettings["terme_mail"].ToString();
                                            }
                                            // string html = File.ReadAllText(path);
                                            email_body = File.ReadAllText(path);
                                            email_body = email_body.Replace( "{{customer_name}}", item.INTITULE_COMPTE );
                                            email_body = email_body.Replace( "{{account_no}}", item.NUMERO_COMPTE );
                                            email_body = email_body.Replace( "{{rib_key}}", item.CLE_COMPTE );
                                            email_body = email_body.Replace( "{{loan_number}}", item.NUMERO_DOSSIER );
                                            email_body = email_body.Replace( "{{task_no}}", ( item.DUREE_DE_CREDIT - item.NBRE_ECHEANCE ).ToString() + "/" + item.DUREE_DE_CREDIT );
                                            email_body = email_body.Replace( "{{due_date}}", item.DATE_1ERE_ECHEANCE.Value.ToString( "dd/MM/yyyy" ) );
                                            email_body = email_body.Replace( "{{amount_due_date}}", item.IMPAYES_CREDIT.ToString() );
                                            //email_body = "Cher client votre";
                                             string sent = Mailer.SendEmail( item.EMAIL_CLIENT, item.TYPE_ENGAGEMENT, email_body );

                                            if ( sent.Equals( sent ) ) {
                                                // update the due date for the next due date
                                                
                                                var next_due_date = item.DATE_1ERE_ECHEANCE.Value.AddMonths( Constant.MENSUELLE );
                                                bool update = AlertRepo.UpdateLoan( item.NUMERO_DOSSIER, next_due_date );
                                                if ( update ) {
                                                    LogAlert.LogWrite( "Action: Loan approved job =====>\n" + "TYPE CUSTOMER: " + item.SECTEUR_ACTIVITE + "NUMERO_DOSSIER: " + item.NUMERO_DOSSIER + "\nUPDATE OF THE NEXT DUE DATE: " + next_due_date + "\nPERIOD: " + item.PERIODICITE );
                                                }
                                            }
                                        }
                                    }
                                    break;

                                case "TRIMESTRIELLE":

                                    // check whether the mail has already been sent
                                    if ( item.DATE_1ERE_ECHEANCE < dateVal ) {
                                        // send another type of email to the customer or set that it
                                        Console.WriteLine( "customer email: " + item.EMAIL_CLIENT + " ===> gestionnaire email: " + item.GESTIONNAIRE_EMAIL );
                                    }
                                    else {
                                        if ( item.DATE_1ERE_ECHEANCE == dateVal ) {
                                            // check the type of loan and send the sms or email
                                            string email_body = string.Empty;
                                            if ( item.SECTEUR_ACTIVITE.Contains( "TERME" ) ) {
                                                email_body = ConfigurationManager.AppSettings["traite_mail"].ToString();
                                            }
                                            else {
                                                email_body = ConfigurationManager.AppSettings["terme_mail"].ToString();
                                            }
                                            email_body = email_body.Replace( "{{customer_name}}", item.INTITULE_COMPTE );
                                            email_body = email_body.Replace( "{{account_no}}", item.NUMERO_COMPTE );
                                            email_body = email_body.Replace( "{{rib_key}}", item.CLE_COMPTE );
                                            email_body = email_body.Replace( "{{loan_number}}", item.NUMERO_DOSSIER );
                                            email_body = email_body.Replace( "{{task_no}}", ( item.DUREE_DE_CREDIT - item.NBRE_ECHEANCE ).ToString() + "/" + item.DUREE_DE_CREDIT );
                                            email_body = email_body.Replace( "{{due_date}}", item.DATE_1ERE_ECHEANCE.Value.ToString( "dd/MM/yyyy" ) );
                                            email_body = email_body.Replace( "{{amount_due_date}}", item.IMPAYES_CREDIT.ToString() );
                                            //email_body = "Cher client votre";
                                            string sent = Mailer.SendEmail( item.EMAIL_CLIENT, item.TYPE_ENGAGEMENT, email_body );

                                            if ( sent.Equals( sent ) ) {
                                                // update the due date for the next due date
                                                var next_due_date = item.DATE_1ERE_ECHEANCE.Value.AddMonths( Constant.TRIMESTRE );
                                                bool update = AlertRepo.UpdateLoan( item.NUMERO_DOSSIER, next_due_date );
                                                if ( update ) {
                                                    LogAlert.LogWrite( "Action: Loan approved job =====>\n" + "TYPE CUSTOMER: " + item.SECTEUR_ACTIVITE + "NUMERO_DOSSIER: " + item.NUMERO_DOSSIER + "\nUPDATE OF THE NEXT DUE DATE: " + next_due_date + "\nPERIOD: " + item.PERIODICITE );
                                                }
                                            }
                                        }
                                    }
                                    break;

                                case "SEMESTRIELLE":

                                    // check whether the mail has already been sent
                                    if ( item.DATE_1ERE_ECHEANCE < dateVal ) {
                                        // send another type of email to the customer or set that it
                                        Console.WriteLine( "customer email: " + item.EMAIL_CLIENT + " ===> gestionnaire email: " + item.GESTIONNAIRE_EMAIL );
                                    }
                                    else {
                                        if ( item.DATE_1ERE_ECHEANCE == dateVal ) {
                                            // check the type of loan and send the sms or email
                                            string email_body = string.Empty;
                                            if ( item.SECTEUR_ACTIVITE.Contains( "TERME" ) ) {
                                                email_body = ConfigurationManager.AppSettings["traite_mail"].ToString();
                                            }
                                            else {
                                                email_body = ConfigurationManager.AppSettings["terme_mail"].ToString();
                                            }
                                            email_body = email_body.Replace( "{{customer_name}}", item.INTITULE_COMPTE );
                                            email_body = email_body.Replace( "{{account_no}}", item.NUMERO_COMPTE );
                                            email_body = email_body.Replace( "{{rib_key}}", item.CLE_COMPTE );
                                            email_body = email_body.Replace( "{{loan_number}}", item.NUMERO_DOSSIER );
                                            email_body = email_body.Replace( "{{task_no}}", ( item.DUREE_DE_CREDIT - item.NBRE_ECHEANCE ).ToString() + "/" + item.DUREE_DE_CREDIT );
                                            email_body = email_body.Replace( "{{due_date}}", item.DATE_1ERE_ECHEANCE.Value.ToString( "dd/MM/yyyy" ) );
                                            email_body = email_body.Replace( "{{amount_due_date}}", item.IMPAYES_CREDIT.ToString() );
                                            email_body = "Cher client votre";
                                            string sent = Mailer.SendEmail( item.EMAIL_CLIENT, item.TYPE_ENGAGEMENT, email_body );

                                            if ( sent.Equals( sent ) ) {
                                                // update the due date for the next due date
                                                var next_due_date = item.DATE_1ERE_ECHEANCE.Value.AddMonths( Constant.SEMESTRE );
                                                bool update = AlertRepo.UpdateLoan( item.NUMERO_DOSSIER, next_due_date );
                                                if ( update ) {
                                                    LogAlert.LogWrite( "Action: Loan approved job =====>\n" + "TYPE CUSTOMER: " + item.SECTEUR_ACTIVITE + "NUMERO_DOSSIER: " + item.NUMERO_DOSSIER + "\nUPDATE OF THE NEXT DUE DATE: " + next_due_date + "\nPERIOD: " + item.PERIODICITE );
                                                }
                                            }
                                        }
                                    }
                                    break;

                                case "ANNUELLE":

                                    // check whether the mail has already been sent
                                    if ( item.DATE_1ERE_ECHEANCE < dateVal ) {
                                        // send another type of email to the customer or set that it
                                        Console.WriteLine( "customer email: " + item.EMAIL_CLIENT + " ===> gestionnaire email: " + item.GESTIONNAIRE_EMAIL );
                                    }
                                    else {
                                        if ( item.DATE_1ERE_ECHEANCE == dateVal ) {
                                            // check the type of loan and send the sms or email
                                            string email_body = string.Empty;
                                            if ( item.SECTEUR_ACTIVITE.Contains( "TERME" ) ) {
                                                email_body = ConfigurationManager.AppSettings["traite_mail"].ToString();
                                            }
                                            else {
                                                email_body = ConfigurationManager.AppSettings["terme_mail"].ToString();
                                            }
                                            email_body = email_body.Replace( "{{customer_name}}", item.INTITULE_COMPTE );
                                            email_body = email_body.Replace( "{{account_no}}", item.NUMERO_COMPTE );
                                            email_body = email_body.Replace( "{{rib_key}}", item.CLE_COMPTE );
                                            email_body = email_body.Replace( "{{loan_number}}", item.NUMERO_DOSSIER );
                                            email_body = email_body.Replace( "{{task_no}}", ( item.DUREE_DE_CREDIT - item.NBRE_ECHEANCE ).ToString() + "/" + item.DUREE_DE_CREDIT );
                                            email_body = email_body.Replace( "{{due_date}}", item.DATE_1ERE_ECHEANCE.Value.ToString( "dd/MM/yyyy" ) );
                                            email_body = email_body.Replace( "{{amount_due_date}}", item.IMPAYES_CREDIT.ToString() );
                                            email_body = "Cher client votre";
                                            string sent = Mailer.SendEmail( item.EMAIL_CLIENT, item.TYPE_ENGAGEMENT, email_body );

                                            if ( sent.Equals( sent ) ) {
                                                // update the due date for the next due date
                                                var next_due_date = item.DATE_1ERE_ECHEANCE.Value.AddYears( Constant.ANNUEL );
                                                bool update = AlertRepo.UpdateLoan( item.NUMERO_DOSSIER, next_due_date );
                                                if ( update ) {
                                                    LogAlert.LogWrite( "Action: Loan approved job =====>\n" + "TYPE CUSTOMER: " + item.SECTEUR_ACTIVITE + "NUMERO_DOSSIER: " + item.NUMERO_DOSSIER + "\nUPDATE OF THE NEXT DUE DATE: " + next_due_date + "\nPERIOD: " + item.PERIODICITE );
                                                }
                                            }
                                        }
                                    }
                                    break;

                            }
                        }
                        else {
                            LogAlert.LogWrite( "Action: Loan approved job =====>\n" + "TYPE CUSTOMER: " + item.SECTEUR_ACTIVITE );

                            switch (item.PERIODICITE)
                            {
                                case "MENSUELLE":
                                    // check whether the mail has already been sent
                                    if (item.DATE_1ERE_ECHEANCE < dateVal)
                                    {
                                        // send another type of email to the customer or set that it
                                        Console.WriteLine("customer email: " + item.EMAIL_CLIENT + " ===> gestionnaire email: " + item.GESTIONNAIRE_EMAIL);
                                    }
                                    else
                                    {
                                        if (item.DATE_1ERE_ECHEANCE == dateVal)
                                        {
                                            // check the type of loan and send the sms or email
                                            string email_body = string.Empty;
                                            string path = string.Empty;
                                            if (item.SECTEUR_ACTIVITE.Contains("TERME"))
                                            {
                                                path = ConfigurationManager.AppSettings["traite_mail"].ToString();
                                            }
                                            else
                                            {
                                                path = ConfigurationManager.AppSettings["terme_mail"].ToString();
                                            }
                                            // string html = File.ReadAllText(path);
                                            email_body = File.ReadAllText(path);
                                            email_body = email_body.Replace("{{customer_name}}", item.INTITULE_COMPTE);
                                            email_body = email_body.Replace("{{account_no}}", item.NUMERO_COMPTE);
                                            email_body = email_body.Replace("{{rib_key}}", item.CLE_COMPTE);
                                            email_body = email_body.Replace("{{loan_number}}", item.NUMERO_DOSSIER);
                                            email_body = email_body.Replace("{{task_no}}", (item.DUREE_DE_CREDIT - item.NBRE_ECHEANCE).ToString() + "/" + item.DUREE_DE_CREDIT);
                                            email_body = email_body.Replace("{{due_date}}", item.DATE_1ERE_ECHEANCE.Value.ToString("dd/MM/yyyy"));
                                            email_body = email_body.Replace("{{amount_due_date}}", item.IMPAYES_CREDIT.ToString());
                                            //email_body = "Cher client votre";
                                            string sent = Mailer.SendEmail(item.EMAIL_CLIENT, item.TYPE_ENGAGEMENT, email_body);

                                            if (sent.Equals(sent))
                                            {
                                                // update the due date for the next due date

                                                var next_due_date = item.DATE_1ERE_ECHEANCE.Value.AddMonths(Constant.MENSUELLE);
                                                bool update = AlertRepo.UpdateLoan(item.NUMERO_DOSSIER, next_due_date);
                                                if (update)
                                                {
                                                    LogAlert.LogWrite("Action: Loan approved job =====>\n" + "TYPE CUSTOMER: " + item.SECTEUR_ACTIVITE + "NUMERO_DOSSIER: " + item.NUMERO_DOSSIER + "\nUPDATE OF THE NEXT DUE DATE: " + next_due_date + "\nPERIOD: " + item.PERIODICITE);
                                                }
                                            }
                                        }
                                    }
                                    break;

                                case "TRIMESTRIELLE":

                                    // check whether the mail has already been sent
                                    if (item.DATE_1ERE_ECHEANCE < dateVal)
                                    {
                                        // send another type of email to the customer or set that it
                                        Console.WriteLine("customer email: " + item.EMAIL_CLIENT + " ===> gestionnaire email: " + item.GESTIONNAIRE_EMAIL);
                                    }
                                    else
                                    {
                                        if (item.DATE_1ERE_ECHEANCE == dateVal)
                                        {
                                            // check the type of loan and send the sms or email
                                            string email_body = string.Empty;
                                            if (item.SECTEUR_ACTIVITE.Contains("TERME"))
                                            {
                                                email_body = ConfigurationManager.AppSettings["traite_mail"].ToString();
                                            }
                                            else
                                            {
                                                email_body = ConfigurationManager.AppSettings["terme_mail"].ToString();
                                            }
                                            email_body = email_body.Replace("{{customer_name}}", item.INTITULE_COMPTE);
                                            email_body = email_body.Replace("{{account_no}}", item.NUMERO_COMPTE);
                                            email_body = email_body.Replace("{{rib_key}}", item.CLE_COMPTE);
                                            email_body = email_body.Replace("{{loan_number}}", item.NUMERO_DOSSIER);
                                            email_body = email_body.Replace("{{task_no}}", (item.DUREE_DE_CREDIT - item.NBRE_ECHEANCE).ToString() + "/" + item.DUREE_DE_CREDIT);
                                            email_body = email_body.Replace("{{due_date}}", item.DATE_1ERE_ECHEANCE.Value.ToString("dd/MM/yyyy"));
                                            email_body = email_body.Replace("{{amount_due_date}}", item.IMPAYES_CREDIT.ToString());
                                            //email_body = "Cher client votre";
                                            string sent = Mailer.SendEmail(item.EMAIL_CLIENT, item.TYPE_ENGAGEMENT, email_body);

                                            if (sent.Equals(sent))
                                            {
                                                // update the due date for the next due date
                                                var next_due_date = item.DATE_1ERE_ECHEANCE.Value.AddMonths(Constant.TRIMESTRE);
                                                bool update = AlertRepo.UpdateLoan(item.NUMERO_DOSSIER, next_due_date);
                                                if (update)
                                                {
                                                    LogAlert.LogWrite("Action: Loan approved job =====>\n" + "TYPE CUSTOMER: " + item.SECTEUR_ACTIVITE + "NUMERO_DOSSIER: " + item.NUMERO_DOSSIER + "\nUPDATE OF THE NEXT DUE DATE: " + next_due_date + "\nPERIOD: " + item.PERIODICITE);
                                                }
                                            }
                                        }
                                    }
                                    break;

                                case "SEMESTRIELLE":

                                    // check whether the mail has already been sent
                                    if (item.DATE_1ERE_ECHEANCE < dateVal)
                                    {
                                        // send another type of email to the customer or set that it
                                        Console.WriteLine("customer email: " + item.EMAIL_CLIENT + " ===> gestionnaire email: " + item.GESTIONNAIRE_EMAIL);
                                    }
                                    else
                                    {
                                        if (item.DATE_1ERE_ECHEANCE == dateVal)
                                        {
                                            // check the type of loan and send the sms or email
                                            string email_body = string.Empty;
                                            if (item.SECTEUR_ACTIVITE.Contains("TERME"))
                                            {
                                                email_body = ConfigurationManager.AppSettings["traite_mail"].ToString();
                                            }
                                            else
                                            {
                                                email_body = ConfigurationManager.AppSettings["terme_mail"].ToString();
                                            }
                                            email_body = email_body.Replace("{{customer_name}}", item.INTITULE_COMPTE);
                                            email_body = email_body.Replace("{{account_no}}", item.NUMERO_COMPTE);
                                            email_body = email_body.Replace("{{rib_key}}", item.CLE_COMPTE);
                                            email_body = email_body.Replace("{{loan_number}}", item.NUMERO_DOSSIER);
                                            email_body = email_body.Replace("{{task_no}}", (item.DUREE_DE_CREDIT - item.NBRE_ECHEANCE).ToString() + "/" + item.DUREE_DE_CREDIT);
                                            email_body = email_body.Replace("{{due_date}}", item.DATE_1ERE_ECHEANCE.Value.ToString("dd/MM/yyyy"));
                                            email_body = email_body.Replace("{{amount_due_date}}", item.IMPAYES_CREDIT.ToString());
                                            email_body = "Cher client votre";
                                            string sent = Mailer.SendEmail(item.EMAIL_CLIENT, item.TYPE_ENGAGEMENT, email_body);

                                            if (sent.Equals(sent))
                                            {
                                                // update the due date for the next due date
                                                var next_due_date = item.DATE_1ERE_ECHEANCE.Value.AddMonths(Constant.SEMESTRE);
                                                bool update = AlertRepo.UpdateLoan(item.NUMERO_DOSSIER, next_due_date);
                                                if (update)
                                                {
                                                    LogAlert.LogWrite("Action: Loan approved job =====>\n" + "TYPE CUSTOMER: " + item.SECTEUR_ACTIVITE + "NUMERO_DOSSIER: " + item.NUMERO_DOSSIER + "\nUPDATE OF THE NEXT DUE DATE: " + next_due_date + "\nPERIOD: " + item.PERIODICITE);
                                                }
                                            }
                                        }
                                    }
                                    break;

                                case "ANNUELLE":

                                    // check whether the mail has already been sent
                                    if (item.DATE_1ERE_ECHEANCE < dateVal)
                                    {
                                        // send another type of email to the customer or set that it
                                        Console.WriteLine("customer email: " + item.EMAIL_CLIENT + " ===> gestionnaire email: " + item.GESTIONNAIRE_EMAIL);
                                    }
                                    else
                                    {
                                        if (item.DATE_1ERE_ECHEANCE == dateVal)
                                        {
                                            // check the type of loan and send the sms or email
                                            string email_body = string.Empty;
                                            if (item.SECTEUR_ACTIVITE.Contains("TERME"))
                                            {
                                                email_body = ConfigurationManager.AppSettings["traite_mail"].ToString();
                                            }
                                            else
                                            {
                                                email_body = ConfigurationManager.AppSettings["terme_mail"].ToString();
                                            }
                                            email_body = email_body.Replace("{{customer_name}}", item.INTITULE_COMPTE);
                                            email_body = email_body.Replace("{{account_no}}", item.NUMERO_COMPTE);
                                            email_body = email_body.Replace("{{rib_key}}", item.CLE_COMPTE);
                                            email_body = email_body.Replace("{{loan_number}}", item.NUMERO_DOSSIER);
                                            email_body = email_body.Replace("{{task_no}}", (item.DUREE_DE_CREDIT - item.NBRE_ECHEANCE).ToString() + "/" + item.DUREE_DE_CREDIT);
                                            email_body = email_body.Replace("{{due_date}}", item.DATE_1ERE_ECHEANCE.Value.ToString("dd/MM/yyyy"));
                                            email_body = email_body.Replace("{{amount_due_date}}", item.IMPAYES_CREDIT.ToString());
                                            email_body = "Cher client votre";
                                            string sent = Mailer.SendEmail(item.EMAIL_CLIENT, item.TYPE_ENGAGEMENT, email_body);

                                            if (sent.Equals(sent))
                                            {
                                                // update the due date for the next due date
                                                var next_due_date = item.DATE_1ERE_ECHEANCE.Value.AddYears(Constant.ANNUEL);
                                                bool update = AlertRepo.UpdateLoan(item.NUMERO_DOSSIER, next_due_date);
                                                if (update)
                                                {
                                                    LogAlert.LogWrite("Action: Loan approved job =====>\n" + "TYPE CUSTOMER: " + item.SECTEUR_ACTIVITE + "NUMERO_DOSSIER: " + item.NUMERO_DOSSIER + "\nUPDATE OF THE NEXT DUE DATE: " + next_due_date + "\nPERIOD: " + item.PERIODICITE);
                                                }
                                            }
                                        }
                                    }
                                    break;

                            }
                        }
                    }
                }


            } catch(Exception ex ) {
                ErrorLog.LogWrite("Error while sending notification to customers.... \nError message: " + ex.Message);
            }
        }
        
        // run the function
        public void Run ()
        {
            while ( KeepGoing ) {
                GenerateNewRequest();
                SendAlertToAccountOfficer();
            }
            Thread.Sleep( 2000 );
        }
    }
}
