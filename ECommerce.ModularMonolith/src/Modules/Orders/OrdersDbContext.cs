using Microsoft.EntityFrameworkCore;
using ECommerce.Modules.Orders.Models;

namespace ECommerce.Modules.Orders;

public class OrdersDbContext : DbContext
{
    public OrdersDbContext(DbContextOptions<OrdersDbContext> options) : base(options)
    {
    }

    public DbSet<Order> Orders => Set<Order>();
}
