# ECommerce Microservices

Simple e-commerce backend split into microservices. Built with .NET 10, RabbitMQ for messaging, and YARP as API gateway.

## Running with Docker

```bash
docker compose up -d --build
```

That's it. RabbitMQ, all services, and the gateway will start automatically.

To stop everything:
```bash
docker compose down
```

## Services

| Service | Port | Description |
|---------|------|-------------|
| Gateway | 5000 | Entry point, routes to other services |
| Catalog | 5001 | Products |
| Customers | 5002 | Customer data |
| Orders | 5003 | Order management |
| Payments | 5004 | Payment processing |
| RabbitMQ | 15672 | Management UI (guest/guest) |

## Quick test

```bash
# get products
curl http://localhost:5000/api/products

# get customers  
curl http://localhost:5000/api/customers

# create order
curl -X POST http://localhost:5000/api/orders \
  -H "Content-Type: application/json" \
  -d '{"customerId": 1, "productId": 1, "quantity": 2}'

# pay for order
curl -X POST http://localhost:5000/api/payments \
  -H "Content-Type: application/json" \
  -d '{"orderId": 1, "method": 0}'
```

Each service has Swagger UI at `/swagger` if you need to poke around.

## How services talk to each other

REST for synchronous calls (Orders checks if customer/product exists before creating order).

RabbitMQ for async events:
- Order created → Catalog reserves stock
- Stock reserved → Order status updated
- Payment completed → Order confirmed

## Project structure

```
src/
├── Gateway/          # YARP reverse proxy
├── Shared/           # Integration events
└── Services/
    ├── Catalog.API/
    ├── Customers.API/
    ├── Orders.API/
    └── Payments.API/
```

## Local development (without Docker)

Start RabbitMQ first:
```bash
docker run -d -p 5672:5672 -p 15672:15672 rabbitmq:3-management
```

Then run each service from IDE or terminal. They're configured to use localhost by default.