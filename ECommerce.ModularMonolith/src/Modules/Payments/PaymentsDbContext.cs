using Microsoft.EntityFrameworkCore;
using ECommerce.Modules.Payments.Models;

namespace ECommerce.Modules.Payments;

public class PaymentsDbContext : DbContext
{
    public PaymentsDbContext(DbContextOptions<PaymentsDbContext> options) : base(options)
    {
    }

    public DbSet<Payment> Payments => Set<Payment>();
}
