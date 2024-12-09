using Microsoft.EntityFrameworkCore;
using OmgvaPOS.BusinessManagement.Entities;
using OmgvaPOS.Customer.Entities;
using OmgvaPOS.Discount.Entities;
using OmgvaPOS.Giftcard.Entities;
using OmgvaPOS.GiftcardPayment.Entities;
using OmgvaPOS.Item.Entities;
using OmgvaPOS.ItemVariation.Entities;
using OmgvaPOS.Order.Entities;
using OmgvaPOS.OrderItem.Entities;
using OmgvaPOS.OrderItemVariation.Entities;
using OmgvaPOS.Payment.Entities;
using OmgvaPOS.Reservation.Entities;
using OmgvaPOS.Schedule.Entities;
using OmgvaPOS.TaxManagement.Entities;
using OmgvaPOS.UserManagement.Entities;

namespace OmgvaPOS.Database.Context
{
    public class OmgvaDbContext : DbContext
    {
        public DbSet<BusinessEntity> Businesses { get; set; }
        public DbSet<CustomerEntity> Customers { get; set; }
        public DbSet<DiscountEntity> Discounts { get; set; }
        public DbSet<EmployeeScheduleEntity> EmployeeSchedules { get; set; }
        public DbSet<GiftcardEntity> Giftcards { get; set; }
        public DbSet<GiftcardPaymentEntity> GiftcardPayments { get; set; }
        public DbSet<ItemEntity> Items { get; set; }
        public DbSet<ItemVariationEntity> ItemVariations { get; set; }
        public DbSet<OrderEntity> Orders { get; set; }
        public DbSet<OrderItemEntity> OrderItems { get; set; }
        public DbSet<OrderItemVariationEntity> OrderItemVariations { get; set; }
        public DbSet<PaymentEntity> Payments { get; set; }
        public DbSet<ReservationEntity> Reservations { get; set; }
        public DbSet<StripeReaderEntity> StripeReaders { get; set; }
        public DbSet<TaxEntity> Taxes { get; set; }
        public DbSet<TaxItemEntity> TaxItems { get; set; }
        public DbSet<UserEntity> Users { get; set; }



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
