using System.Text.Json.Serialization;
namespace OmgvaPOS.UserManagement.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum UserRole {
    Employee,
    Owner,
    Admin
}