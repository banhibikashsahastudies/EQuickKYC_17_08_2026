using EQuickKYC.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EQuickKYC.Infrastructure.Data
{
    public class EQuickKYCDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Card> Cards { get; set; }

        public EQuickKYCDbContext(DbContextOptions<EQuickKYCDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // =========================
            // User
            // =========================

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("Users");

                entity.HasKey(x => x.Id);

                entity.Property(x => x.Id)
                    .ValueGeneratedOnAdd();

                entity.Property(x => x.FirstName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(x => x.MiddleName)
                    .HasMaxLength(50);

                entity.Property(x => x.LastName)
                    .HasMaxLength(50);

                entity.Property(x => x.Dob)
                    .IsRequired();

                entity.Property(x => x.Gender)
                    .IsRequired()
                    .HasMaxLength(1);

                entity.Property(x => x.Mobile)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(x => x.Email)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(x => x.CreatedOn)
                    .IsRequired();

                entity.Property(x => x.UpdatedOn)
                    .IsRequired(false);

                entity.Property(x => x.CreatedBy)
                    .IsRequired(false);

                entity.Property(x => x.UpdatedBy)
                    .IsRequired(false);

                entity.Property(x => x.IsDeleted)
                    .IsRequired();

                entity.Property(x => x.status)
                    .IsRequired();

                // Mobile must be unique
                entity.HasIndex(x => x.Mobile)
                    .IsUnique();

                // Email must be unique
                entity.HasIndex(x => x.Email)
                    .IsUnique();

                // AddressId must be unique because User -> Address is one-to-one
                entity.HasIndex(x => x.AddressId)
                    .IsUnique();

                // CardId must be unique because User -> Card is one-to-one
                entity.HasIndex(x => x.CardId)
                    .IsUnique();
            });

            // =========================
            // Address
            // =========================

            modelBuilder.Entity<Address>(entity =>
            {
                entity.ToTable("Addresses");

                entity.HasKey(x => x.AddressId);

                entity.Property(x => x.Country)
                    .HasMaxLength(20);

                entity.Property(x => x.State)
                    .HasMaxLength(20);

                entity.Property(x => x.City)
                    .HasMaxLength(20);

                entity.Property(x => x.ZipCode)
                    .IsRequired(false);
            });

            // =========================
            // Card
            // =========================

            modelBuilder.Entity<Card>(entity =>
            {
                entity.ToTable("Cards");

                entity.HasKey(x => x.CardId);

                entity.Property(x => x.AadhaarNo)
                    .HasMaxLength(12);

                entity.Property(x => x.PanNo)
                    .HasMaxLength(10);

                entity.Property(x => x.VoterNo)
                    .HasMaxLength(10);

                entity.Property(x => x.DrivingLicenseNo)
                    .HasMaxLength(10);
            });

            // =========================
            // User -> Address
            // Required one-to-one
            // =========================

            modelBuilder.Entity<User>()
                .HasOne(x => x.Address)
                .WithOne()
                .HasForeignKey<User>(x => x.AddressId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            // =========================
            // User -> Card
            // Required one-to-one
            // =========================

            modelBuilder.Entity<User>()
                .HasOne(x => x.Card)
                .WithOne()
                .HasForeignKey<User>(x => x.CardId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}