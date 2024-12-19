using OmgvaPOS.OrderItemManagement.Models;

namespace OmgvaPOS.OrderItemManagement.Service
{
    public interface IOrderItemDeletionService
    {
        void DeleteOrderItem(long orderItemId, bool useTransaction = false);
        public void ReturnItemsToInventory(ICollection<OrderItem> orderItems);
    }
}
