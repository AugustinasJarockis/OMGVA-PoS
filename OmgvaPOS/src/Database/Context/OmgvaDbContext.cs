using Microsoft.EntityFrameworkCore;
using OmgvaPOS.BusinessManagement.Models;
using OmgvaPOS.CustomerManagement.Models;
using OmgvaPOS.DiscountManagement.Models;
using OmgvaPOS.GiftcardManagement.Models;
using OmgvaPOS.GiftcardPaymentManagement.Models;
using OmgvaPOS.ItemManagement.Models;
using OmgvaPOS.ItemVariationManagement.Models;
using OmgvaPOS.OrderItemManagement.Models;
using OmgvaPOS.OrderItemVariationManagement.Models;
using OmgvaPOS.OrderManagement.Models;
using OmgvaPOS.PaymentManagement.Models;
using OmgvaPOS.ReservationManagement.Models;
using OmgvaPOS.ScheduleManagement.Models;
using OmgvaPOS.TaxManagement.Models;
using OmgvaPOS.UserManagement.Models;

namespace OmgvaPOS.Database.Context
{
    public class OmgvaDbContext : DbContext
    {
        public DbSet<Business> Businesses { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Discount> Discounts { get; set; }
        public DbSet<EmployeeSchedule> EmployeeSchedules { get; set; }
        public DbSet<Giftcard> Giftcards { get; set; }
        public DbSet<GiftcardPaymentEntity> GiftcardPayments { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<ItemVariation> ItemVariations { get; set; }
        public DbSet<OrderManagement.Models.Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<OrderItemVariation> OrderItemVariations { get; set; }
        public DbSet<PaymentManagement.Models.Payment> Payments { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<StripeReader> StripeReaders { get; set; }
        public DbSet<Tax> Taxes { get; set; }
        public DbSet<TaxItem> TaxItems { get; set; }
        public DbSet<User> Users { get; set; }



        public OmgvaDbContext(DbContextOptions<OmgvaDbContext> options ) : base( options ) { }

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
