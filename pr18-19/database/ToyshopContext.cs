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

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<User> Users { get; set; }

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

        modelBuilder.Entity<Role>(entity =>
        {
            entity.ToTable("Role");

            entity.Property(e => e.RoleId).ValueGeneratedNever();
            entity.Property(e => e.RoleName).HasMaxLength(50);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("User");

            entity.Property(e => e.UserId)
                .ValueGeneratedNever()
                .HasColumnName("UserID");
            entity.Property(e => e.UserName).HasMaxLength(50);
            entity.Property(e => e.UserPatronymic).HasMaxLength(50);
            entity.Property(e => e.UserSurname).HasMaxLength(50);

            entity.HasOne(d => d.UserRoleNavigation).WithMany(p => p.Users)
                .HasForeignKey(d => d.UserRole)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_User_Role");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
