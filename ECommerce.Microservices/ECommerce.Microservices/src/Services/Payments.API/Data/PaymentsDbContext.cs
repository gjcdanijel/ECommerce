using Microsoft.EntityFrameworkCore;
using Payments.API.Models;

namespace Payments.API.Data;

public class PaymentsDbContext : DbContext
{
    public PaymentsDbContext(DbContextOptions<PaymentsDbContext> options) : base(options)
    {
    }

    public DbSet<Payment> Payments => Set<Payment>();
}
