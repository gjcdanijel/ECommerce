using MediatR;

namespace ECommerce.Modules.Customers.Contracts;

public record CustomerDto(int Id, string FirstName, string LastName, string Email);

public record GetCustomerByIdQuery(int CustomerId) : IRequest<CustomerDto?>;

public record CustomerExistsQuery(int CustomerId) : IRequest<bool>;
