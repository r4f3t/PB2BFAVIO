﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PB2B.Entity
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class LKSDBEntities1 : DbContext
    {
        public LKSDBEntities1()
            : base("name=LKSDBEntities1")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<A_MNTL_STOK_2018> A_MNTL_STOK_2018 { get; set; }
        public virtual DbSet<C_INT_CARIONEKRAN_006> C_INT_CARIONEKRAN_006 { get; set; }
        public virtual DbSet<ISRG_FaturaDetaY_006> ISRG_FaturaDetaY_006 { get; set; }
        public virtual DbSet<ISRG_Faturalar_006> ISRG_Faturalar_006 { get; set; }
        public virtual DbSet<ISRG_Hesap_Extresi_006_04> ISRG_Hesap_Extresi_006_04 { get; set; }
    }
}
