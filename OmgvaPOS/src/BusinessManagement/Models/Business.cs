using OmgvaPOS.ItemManagement.Models;
using OmgvaPOS.PaymentManagement.Models;
using OmgvaPOS.UserManagement.Models;

namespace OmgvaPOS.BusinessManagement.Models
{
    public class Business
    {
        public long Id { get; set; }
        public string StripeSecretKey { get; set; }
        public string StripePublishKey { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }

        // navigational properties
        public ICollection<Item> Items { get; set; } // business can have Items
        public ICollection<User> Users { get; set; } // can have Users
        public ICollection<StripeReader> StripeReaders { get; set; } // can have card readers
    }
}