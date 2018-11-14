using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Docker.OrderDomain.Grpc.Context
{
    public partial class OrderDomainContext : DbContext
    {
        public OrderDomainContext()
        {
        }

        public OrderDomainContext(DbContextOptions<OrderDomainContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Order> Order { get; set; }
        public virtual DbSet<OrderProduct> OrderProduct { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder){ }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(e => e.Ref);

                entity.Property(e => e.Ref).HasColumnType("char(38)");

                entity.Property(e => e.Created).HasColumnType("datetime");

                entity.Property(e => e.TotalValue).HasColumnType("decimal(10,0)");

                entity.Property(e => e.Updated).HasColumnType("datetime");
            });

            modelBuilder.Entity<OrderProduct>(entity =>
            {
                entity.HasKey(e => e.Ref);

                entity.HasIndex(e => e.OrderRef)
                    .HasName("op_order");

                entity.Property(e => e.Ref).HasColumnType("char(38)");

                entity.Property(e => e.OrderRef)
                    .IsRequired()
                    .HasColumnType("char(38)");

                entity.Property(e => e.ProductName)
                    .IsRequired()
                    .HasColumnType("varchar(100)");

                entity.Property(e => e.Value).HasColumnType("decimal(10,0)");

                entity.HasOne(d => d.OrderRefNavigation)
                    .WithMany(p => p.OrderProduct)
                    .HasForeignKey(d => d.OrderRef)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("op_order");
            });
        }
    }
}
