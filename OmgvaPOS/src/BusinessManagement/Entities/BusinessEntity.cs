using OmgvaPOS.Item.Entities;
using OmgvaPOS.Payment.Entities;
using OmgvaPOS.UserManagement.Entities;

namespace OmgvaPOS.BusinessManagement.Entities
{
    public class BusinessEntity
    {
        public long Id { get; set; }
        public string StripeAccId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }

        // navigational properties
        public ICollection<ItemEntity> Items { get; set; } // business can have Items
        public ICollection<UserEntity> Users { get; set; } // can have Users
        public ICollection<StripeReaderEntity> StripeReaders { get; set; } // can have card readers
    }
}
