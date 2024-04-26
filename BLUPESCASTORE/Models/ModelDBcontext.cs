using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;

namespace BLUPESCASTORE.Models
{
    public partial class ModelDBcontext : DbContext
    {
        public ModelDBcontext()
            : base("name=ModelDBcontext")
        {
        }

        public virtual DbSet<DETTAGLIO> DETTAGLIO { get; set; }
        public virtual DbSet<ORDINE> ORDINE { get; set; }
        public virtual DbSet<ARTICOLO> ARTICOLO { get; set; }
        public virtual DbSet<USER> USER { get; set; }
        public object Users { get; internal set; }
        public virtual DbSet<CATEGORIA> CATEGORIA { get; set; }

        public virtual DbSet<PAGAMENTI> PAGAMENTI { get; set; }

        public virtual DbSet<PREFERITI> PREFERITI { get; set; }

        /*public virtual DbSet<CLIENTI> CLIENTI { get; set; }*/

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();


            modelBuilder.Entity<DETTAGLIO>()
                 .Property(e => e.PrezzoTotale)
                 .HasPrecision(19, 4);

            modelBuilder.Entity<ORDINE>()
                .Property(e => e.TotaleImporto)
                .HasPrecision(19, 4);

            modelBuilder.Entity<ORDINE>()
                .HasMany(e => e.DETTAGLIO)
                .WithOptional(e => e.ORDINE)
                .HasForeignKey(e => e.IdOrdine);

            modelBuilder.Entity<ARTICOLO>()
                .Property(e => e.Prezzo)
                .HasPrecision(19, 4);

            modelBuilder.Entity<ARTICOLO>()
                .HasMany(e => e.DETTAGLIO)
                .WithRequired(e => e.ARTICOLO)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<USER>()
                .HasMany(e => e.DETTAGLIO)
                .WithRequired(e => e.USER)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<USER>()
                .HasMany(e => e.ORDINE)
                .WithRequired(e => e.USER)
                .WillCascadeOnDelete(false);


        }
    }
}