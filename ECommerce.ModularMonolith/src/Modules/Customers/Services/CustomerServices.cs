using MediatR;
using Microsoft.EntityFrameworkCore;
using ECommerce.Modules.Customers.Contracts;
using ECommerce.Modules.Customers.Models;

namespace ECommerce.Modules.Customers.Services;

public class GetCustomerByIdHandler : IRequestHandler<GetCustomerByIdQuery, CustomerDto?>
{
    private readonly CustomersDbContext _context;

    public GetCustomerByIdHandler(CustomersDbContext context)
    {
        _context = context;
    }

    public async Task<CustomerDto?> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
    {
        var customer = await _context.Customers.FindAsync(request.CustomerId);
        if (customer == null)
            return null;

        return new CustomerDto(customer.Id, customer.FirstName, customer.LastName, customer.Email);
    }
}

public class CustomerExistsHandler : IRequestHandler<CustomerExistsQuery, bool>
{
    private readonly CustomersDbContext _context;

    public CustomerExistsHandler(CustomersDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(CustomerExistsQuery request, CancellationToken cancellationToken)
    {
        return await _context.Customers.AnyAsync(c => c.Id == request.CustomerId, cancellationToken);
    }
}

public class CustomerService
{
    private readonly CustomersDbContext _context;

    public CustomerService(CustomersDbContext context)
    {
        _context = context;
    }

    public async Task<List<Customer>> GetAllAsync()
    {
        return await _context.Customers.ToListAsync();
    }

    public async Task<Customer?> GetByIdAsync(int id)
    {
        return await _context.Customers.FindAsync(id);
    }

    public async Task<Customer> CreateAsync(Customer customer)
    {
        _context.Customers.Add(customer);
        await _context.SaveChangesAsync();
        return customer;
    }
}
