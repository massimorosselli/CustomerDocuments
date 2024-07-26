using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace CustomerDocuments.Models;

public partial class CustomerContext : DbContext
{
    public CustomerContext()
    {
    }

    public CustomerContext(DbContextOptions<CustomerContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Customer> Customers { get; set; }

    public DbSet<Document> Documents { get; set; }
    public DbSet<Placeholder> Placeholders { get; set; }
    public DbSet<UserTransaction> UserTransactions { get; set; }


    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //    => optionsBuilder.UseSqlServer("Name=DefaultConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>(entity =>
        {
            entity.ToTable("Customer");

            entity.Property(e => e.Address)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Surname)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TaxCode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Vat)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("VAT");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
