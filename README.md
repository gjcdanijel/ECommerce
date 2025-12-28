# ECommerce

Seminar paper for **Software Engineering** course - "Types of Modern Software System Architectures".

Same e-commerce app implemented in three different architectures:

- `ECommerce.Monolith` - classic monolithic architecture
- `ECommerce.ModularMonolith` - modular monolith with MediatR
- `ECommerce.Microservices` - microservices with RabbitMQ and API Gateway

## Tech stack

- .NET 10
- Entity Framework Core (InMemory)
- RabbitMQ (microservices)
- YARP API Gateway (microservices)
- Docker

## Running microservices

```bash
cd ECommerce.Microservices
docker compose up -d --build
```

Services available on ports 5000-5004, RabbitMQ UI on 15672.
