using OmgvaPOS.UserManagement.Enums;
using OmgvaPOS.UserManagement.Models;

namespace OmgvaPOS.OrderManagement.DTOs;

public class SimpleUserDTO
{
    public long UserId { get; set; }
    public string UserName { get; set; }
}
