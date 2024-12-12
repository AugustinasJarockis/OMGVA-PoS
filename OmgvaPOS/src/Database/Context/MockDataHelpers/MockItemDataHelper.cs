using OmgvaPOS.ItemManagement.Models;
using OmgvaPOS.TaxManagement.Models;

namespace OmgvaPOS.Database.Context.MockDataHelpers
{
    public class MockItemDataHelper
    {
        public static void InitializeMockItems(OmgvaDbContext dbContext, ILogger logger) {
            logger.LogDebug("Adding mock items...");
            dbContext.Items.AddRange(MockItems());
            dbContext.SaveChanges();
            logger.LogDebug("Mock items added.");
        }

        public static void RemoveAllItems(OmgvaDbContext dbContext, ILogger logger) {
            logger.LogDebug("Removing all items...");
            var allItems = dbContext.Items.ToList();
            dbContext.Items.RemoveRange(allItems);
            dbContext.SaveChanges();
            logger.LogDebug("All items removed.");
        }

        private static IEnumerable<Item> MockItems() {
            return new List<Item>
            {
                new() {
                    Name = "Pipirai",
                    InventoryQuantity = 2000,
                    Price = 5.55M,
                    Currency = "EUR",
                    ItemGroup = "Maistas",
                    ImgPath = "https://upload.wikimedia.org/wikipedia/commons/thumb/8/85/Green-Yellow-Red-Pepper-2009.jpg/800px-Green-Yellow-Red-Pepper-2009.jpg",
                    BusinessId = MockBusinessesDataHelper.OmgvaBusinessId,
                    IsArchived = false
                },
                new() {
                    Name = "Varškė",
                    InventoryQuantity = 100,
                    Price = 15M,
                    Currency = "USD",
                    ItemGroup = "Maistas",
                    ImgPath = "https://www.lrt.lt/img/2020/11/25/780753-207032-1287x836.jpg",
                    BusinessId = MockBusinessesDataHelper.OmgvaBusinessId,
                    IsArchived = false
                },
                new() {
                    Name = "Atvežimas",
                    InventoryQuantity = 1,
                    Price = 30.20M,
                    Currency = "EUR",
                    ItemGroup = "Paslaugos",
                    ImgPath = "https://swiftdeliveryandlogistics.com/wp-content/uploads/63a99832ddf2277ef743f5e4_Last-Mile-Delivery.webp",
                    BusinessId = MockBusinessesDataHelper.OmgvaBusinessId,
                    Duration = new TimeSpan(0, 30, 0),
                    IsArchived = false,
                    UserId = MockUserDataHelper.OmgvaBusinessEmployee
                }
            };
        }
    }
}
