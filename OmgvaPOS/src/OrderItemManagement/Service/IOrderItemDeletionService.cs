namespace OmgvaPOS.OrderItemManagement.Service
{
    public interface IOrderItemDeletionService
    {
        void DeleteOrderItem(long orderItemId, bool useTransaction = false);
    }
}
