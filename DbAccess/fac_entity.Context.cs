﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DbAccess
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class FACSDBEntities : DbContext
    {
        public FACSDBEntities()
            : base("name=FACSDBEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Client_Group_Master> Client_Group_Master { get; set; }
        public virtual DbSet<Client_Master> Client_Master { get; set; }
        public virtual DbSet<Contact_Type_Master> Contact_Type_Master { get; set; }
        public virtual DbSet<Disposition_Master> Disposition_Master { get; set; }
        public virtual DbSet<Patient_Master> Patient_Master { get; set; }
        public virtual DbSet<Payer_Master> Payer_Master { get; set; }
        public virtual DbSet<User_Master> User_Master { get; set; }
        public virtual DbSet<User_Type_Master> User_Type_Master { get; set; }
        public virtual DbSet<Contact_Sub_Type_Master> Contact_Sub_Type_Master { get; set; }
        public virtual DbSet<Contact_Master> Contact_Master { get; set; }
        public virtual DbSet<Address_Master> Address_Master { get; set; }
        public virtual DbSet<Address_Sub_Type_Master> Address_Sub_Type_Master { get; set; }
        public virtual DbSet<Address_Type_Master> Address_Type_Master { get; set; }
        public virtual DbSet<Patient_Type_Master> Patient_Type_Master { get; set; }
        public virtual DbSet<Patient_Receivables_Info> Patient_Receivables_Info { get; set; }
        public virtual DbSet<Patient_Treatments> Patient_Treatments { get; set; }
        public virtual DbSet<Patient_Status> Patient_Status { get; set; }
        public virtual DbSet<Payment> Payments { get; set; }
    }
}