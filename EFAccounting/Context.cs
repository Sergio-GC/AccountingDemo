using EFAccounting.Entities;
using Microsoft.EntityFrameworkCore;

namespace EFAccounting
{
    public class Context : DbContext
    {
        public DbSet<Kid> Kids { get; set; }
        public DbSet<WDay> Wdays { get; set; }
        public DbSet<Price> Prices { get; set; }
        public DbSet<SiblingRelationship> SiblingRelationships { get; set; }

        public string DbPath { get; }

        // Deliberate empty constructor
        public Context(DbContextOptions<Context> options) : base(options) { }

        // Create relations between kids to store siblings
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Create primary key for the relationship entity
            modelBuilder.Entity<SiblingRelationship>()
                .HasKey(r => new { r.FromKidId, r.ToKidId });

            // I honestly don't know :/
            modelBuilder.Entity<SiblingRelationship>()
                .HasOne(r => r.FromKid)
                .WithMany(k => k.Siblings)
                .HasForeignKey(f => f.FromKidId)
                .OnDelete(DeleteBehavior.Restrict);

            // I honestly don't know :/
            modelBuilder.Entity<SiblingRelationship>()
                .HasOne(r => r.ToKid)
                .WithMany()
                .HasForeignKey(f => f.ToKidId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
