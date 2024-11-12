using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using OMGVA_PoS.Data_layer.Models;

namespace OMGVA_PoS.Data_layer.Context
{
    public class OMGVADbContext : DbContext
    {
        public DbSet<Business> Businesses { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Discount> Discounts { get; set; }
        public DbSet<EmployeeSchedule> EmployeeSchedules { get; set; }
        public DbSet<GiftCard> GiftCards { get; set; }
        public DbSet<GiftCardPayment> GiftCardPayments { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<ItemVariation> ItemVariations { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<OrderItemVariation> OrderItemVariations { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<StripeReader> StripeReaders { get; set; }
        public DbSet<Tax> Taxes { get; set; }
        public DbSet<TaxItem> TaxItems { get; set; }
        public DbSet<User> Users { get; set; }



        public OMGVADbContext(DbContextOptions<OMGVADbContext> options ) : base( options ) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                // dotnet EF is crying like a little baby because we didn't set a decimal type :/
                foreach (var property in entity.GetProperties())
                {
                    if (property.ClrType == typeof(decimal))
                    {
                        property.SetColumnType("decimal(18,2)");  
                    }
                }

                // for PoS system archiving purposes cascading deletetion is DISABLED project wide
                foreach (var foreignKey in entity.GetForeignKeys())
                {
                    foreignKey.DeleteBehavior = DeleteBehavior.Restrict; 
                }
            }

            base.OnModelCreating(modelBuilder);
        }
    }
}
