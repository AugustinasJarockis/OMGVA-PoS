using OmgvaPOS.OrderManagement.Enums;

namespace OmgvaPOS.OrderManagement.DTOs;

public class OrdersRequestCriteria
{
    public OrderStatus? RequestedOrderStatus { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    
    public OrdersRequestCriteria(
        OrderStatus? orderStatus = null, 
        int? pageNumber = null, 
        int? pageSize = null)
    {
        RequestedOrderStatus = orderStatus;
        PageSize = pageSize ?? PageSize;
        PageNumber = pageNumber ?? PageNumber;
    }
    
}