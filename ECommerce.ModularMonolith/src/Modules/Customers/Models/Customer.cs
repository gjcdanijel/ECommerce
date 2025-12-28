using ECommerce.Shared;

namespace ECommerce.Modules.Customers.Models;

public class Customer : Entity
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}
