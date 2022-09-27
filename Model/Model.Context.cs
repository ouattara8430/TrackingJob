﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class CommitmentDBEntities : DbContext
    {
        public CommitmentDBEntities()
            : base("name=CommitmentDBEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<ActivityLog> ActivityLogs { get; set; }
        public virtual DbSet<AlertRequest> AlertRequests { get; set; }
        public virtual DbSet<Branch> Branches { get; set; }
        public virtual DbSet<Decision> Decisions { get; set; }
        public virtual DbSet<Document> Documents { get; set; }
        public virtual DbSet<EmailSent> EmailSents { get; set; }
        public virtual DbSet<Financing> Financings { get; set; }
        public virtual DbSet<GestionnaireReport> GestionnaireReports { get; set; }
        public virtual DbSet<LoanApplication> LoanApplications { get; set; }
        public virtual DbSet<LoanApproved> LoanApproveds { get; set; }
        public virtual DbSet<MassMail> MassMails { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Type> Types { get; set; }
        public virtual DbSet<UserProfile> UserProfiles { get; set; }
        public virtual DbSet<AlertRequestBackup> AlertRequestBackups { get; set; }
    
        public virtual ObjectResult<FetchAccountOfficerHistoric_Result> FetchAccountOfficerHistoric(Nullable<int> userId, string status)
        {
            var userIdParameter = userId.HasValue ?
                new ObjectParameter("userId", userId) :
                new ObjectParameter("userId", typeof(int));
    
            var statusParameter = status != null ?
                new ObjectParameter("status", status) :
                new ObjectParameter("status", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<FetchAccountOfficerHistoric_Result>("FetchAccountOfficerHistoric", userIdParameter, statusParameter);
        }
    
        public virtual ObjectResult<FetchHistoric_Result> FetchHistoric()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<FetchHistoric_Result>("FetchHistoric");
        }
    
        public virtual ObjectResult<FetchHistoricByPeriod_Result> FetchHistoricByPeriod(string start_date, string end_date)
        {
            var start_dateParameter = start_date != null ?
                new ObjectParameter("start_date", start_date) :
                new ObjectParameter("start_date", typeof(string));
    
            var end_dateParameter = end_date != null ?
                new ObjectParameter("end_date", end_date) :
                new ObjectParameter("end_date", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<FetchHistoricByPeriod_Result>("FetchHistoricByPeriod", start_dateParameter, end_dateParameter);
        }
    
        public virtual ObjectResult<FetchRequests_Result> FetchRequests()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<FetchRequests_Result>("FetchRequests");
        }
    }
}
