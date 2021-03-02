using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace DbBicyclesLab.Models
{
    public partial class DBBicyclesContext : DbContext
    {
        public DBBicyclesContext()
        {
        }

        public DBBicyclesContext(DbContextOptions<DBBicyclesContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AuthorizedDealer> AuthorizedDealers { get; set; }
        public virtual DbSet<Bicycle> Bicycles { get; set; }
        public virtual DbSet<BicycleModel> BicycleModels { get; set; }
        public virtual DbSet<Brand> Brands { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Color> Colors { get; set; }
        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<Gender> Genders { get; set; }
        public virtual DbSet<Size> Sizes { get; set; }
        public virtual DbSet<SizeColorModel> SizeColorModels { get; set; }
        public virtual DbSet<ImageModel> ImageModels { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server= (LocalDb)\\Lab1Bicycles; Database=DBBicycles; Trusted_Connection=True; ");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<AuthorizedDealer>(entity =>
            {
                entity.Property(e => e.DealerName)
                    .IsRequired()
                    .HasMaxLength(45);

                entity.Property(e => e.Description).HasMaxLength(1500);

                entity.Property(e => e.WebsiteAddress)
                    .IsRequired()
                    .HasMaxLength(2048);
            });

            modelBuilder.Entity<Bicycle>(entity =>
            {
                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(1500);

                entity.HasOne(d => d.SizeColorModel)
                    .WithMany(p => p.Bicycles)
                    .HasForeignKey(d => d.SizeColorModelId)
                    .HasConstraintName("FK_Bicycles_SizeColorModel");
            });

            modelBuilder.Entity<BicycleModel>(entity =>
            {
                entity.Property(e => e.Description).HasMaxLength(2500);

                entity.Property(e => e.ModelName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.Brand)
                    .WithMany(p => p.BicycleModels)
                    .HasForeignKey(d => d.BrandId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Models_Brands");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.BicycleModels)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Models_Categories");

                entity.HasOne(d => d.Gender)
                    .WithMany(p => p.BicycleModels)
                    .HasForeignKey(d => d.GenderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Models_Genders");
            });

            modelBuilder.Entity<Brand>(entity =>
            {
                entity.Property(e => e.BrandName)
                    .IsRequired()
                    .HasMaxLength(25);

                entity.Property(e => e.Description).HasMaxLength(1500);

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.Brands)
                    .HasForeignKey(d => d.CountryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Brands_Countries");

                entity.HasOne(d => d.Dealer)
                    .WithMany(p => p.Brands)
                    .HasForeignKey(d => d.DealerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Brands_AuthorizedDealers");
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.Property(e => e.CategoryName)
                    .IsRequired()
                    .HasMaxLength(25);

                entity.Property(e => e.Description).HasMaxLength(1500);
            });

            modelBuilder.Entity<Color>(entity =>
            {
                entity.Property(e => e.ColorName)
                    .IsRequired()
                    .HasMaxLength(25);
            });

            modelBuilder.Entity<Country>(entity =>
            {
                entity.Property(e => e.CountryName)
                    .IsRequired()
                    .HasMaxLength(20);
            });

            modelBuilder.Entity<Gender>(entity =>
            {
                entity.Property(e => e.GenderName)
                    .IsRequired()
                    .HasMaxLength(25);
            });

            modelBuilder.Entity<Size>(entity =>
            {
                entity.Property(e => e.SizeName)
                    .IsRequired()
                    .HasMaxLength(25);
            });

            modelBuilder.Entity<SizeColorModel>(entity =>
            {
                entity.ToTable("SizeColorModel");

                entity.HasOne(d => d.Color)
                    .WithMany(p => p.SizeColorModels)
                    .HasForeignKey(d => d.ColorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SizeColorModel_Colors");

                entity.HasOne(d => d.Model)
                    .WithMany(p => p.SizeColorModels)
                    .HasForeignKey(d => d.ModelId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SizeColorModel_Models");

                entity.HasOne(d => d.Size)
                    .WithMany(p => p.SizeColorModels)
                    .HasForeignKey(d => d.SizeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SizeColorModel_Sizes");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
