using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace pr18_19.database;

public partial class ToyshopContext : DbContext
{
    public ToyshopContext()
    {
    }

    public ToyshopContext(DbContextOptions<ToyshopContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Asstoy> Asstoys { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=LAB17-13\\SQLEXPRESS; Database=toyshop; User=исп-31; Password=1234567890; Encrypt=false");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Asstoy>(entity =>
        {
            entity.HasKey(e => e.Toy);

            entity.ToTable("asstoy");

            entity.Property(e => e.Toy)
                .HasMaxLength(50)
                .HasColumnName("toy");
            entity.Property(e => e.Age).HasColumnName("age");
            entity.Property(e => e.City)
                .HasMaxLength(50)
                .HasColumnName("city");
            entity.Property(e => e.Cost)
                .HasColumnType("money")
                .HasColumnName("cost");
            entity.Property(e => e.Fabrica)
                .HasMaxLength(50)
                .HasColumnName("fabrica");
            entity.Property(e => e.Kolvo).HasColumnName("kolvo");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
