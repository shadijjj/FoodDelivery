# 🍔 Event-Driven Food Delivery Microservices Platform

A distributed, event-driven backend ecosystem built with .NET 10.0, Docker, RabbitMQ, and YARP API Gateway. Designed using Clean Architecture, CQRS, Database-per-Service, and asynchronous event messaging.

## 🛠️ Technology Stack

| Category | Technologies & Libraries |
|----------|-------------------------|
| Framework & Runtime | .NET 10.0 (C#), ASP.NET Core Web API |
| Orchestration & Containers | Docker, Docker Compose, Linux Multi-Stage Images |
| Messaging & Async Bus | RabbitMQ (AMQP) |
| API Gateway | YARP (Yet Another Reverse Proxy) |
| Databases & Persistence | PostgreSQL 18+, Entity Framework Core |
| Architecture & Patterns | CQRS, MediatR, Clean Architecture, Database-per-Service |
| Security & Auth | ASP.NET Core Identity, JWT Bearer Tokens, Role Claims |

## 🏗️ System Architecture & Event Flow

| Microservice / Component | Role & Responsibilities | Communication Type |
|--------------------------|-------------------------|-------------------|
| YARP API Gateway | Public entrypoint (Port 5000) & reverse proxy | Synchronous HTTP |
| Identity.API | User registration, authentication, & JWT issuance | Synchronous HTTP |
| Restaurant.API | Restaurant catalog & menu management | Synchronous HTTP |
| Ordering.API | Order creation & validation (CQRS) | Asynchronous Event Producer |
| RabbitMQ | Message broker storing OrderSubmittedEvent queues | AMQP Bus |
| Delivery.API | Driver assignment & status tracking | Asynchronous Event Consumer |

## 🔄 Asynchronous Checkout Sequence

1. **Client Request:** Client sends `POST /api/orders` to YARP API Gateway (:5000)
2. **Order Creation:** Gateway routes to Ordering.API, which persists the order to OrderingDb
3. **Event Publish:** Ordering.API publishes `OrderSubmittedEvent` to RabbitMQ and returns `200 OK` to the client immediately
4. **Event Consume:** Delivery.API pulls `OrderSubmittedEvent` from RabbitMQ in the background, creates a delivery record, and assigns a driver

## 📂 Project Structure

```
FoodDelivery/
├── src/
│   ├── Gateways/
│   │   └── ApiGateway/          # YARP Proxy Configuration
│   ├── Services/
│   │   ├── Identity/            # Auth & JWT Token Provider
│   │   ├── Restaurant/          # Menu & Catalog Management
│   │   ├── Ordering/            # CQRS Order Processing & Event Publisher
│   │   └── Delivery/            # Event Consumer & Driver Assignment
│   └── Shared/
│       └── Contracts/           # Shared Integration Events (RabbitMQ DTOs)
├── docker-compose.yml           # Infrastructure & Service Orchestration
└── README.md
```

## 🌐 Active Endpoints & Port Map

| Component | Endpoint / URL | Access / Credentials |
|-----------|---------------|---------------------|
| API Gateway | http://localhost:5000 | Public API Entrypoint |
| RabbitMQ Management | http://localhost:15672 | User: `guest` / Pass: `guest` |
| PostgreSQL Database | localhost:5432 | User: `postgres` / Pass: `postgrespassword` |
| Identity API (Auth) | http://localhost:5000/api/auth | User registration, login, and JWT generation |
| Restaurant API | http://localhost:5000/api/restaurants | Menu catalog & restaurant management |
| Ordering API | http://localhost:5000/api/orders | CQRS order submission (publishes to RabbitMQ) |
| Delivery API | http://localhost:5000/api/deliveries | Driver assignment (consumes from RabbitMQ) |

## 🚀 Quick Start & Local Execution

### Prerequisites
- Docker Desktop installed and running

### Spin Up Infrastructure

1. **Clone the repository:**
   ```bash
   git clone https://github.com/your-username/FoodDelivery.git
   cd FoodDelivery
   ```

2. **Build and run all 7 containers in background mode:**
   ```bash
   docker compose up -d
   ```

3. **Stream live events between publisher and consumer:**
   ```bash
   docker compose logs -f ordering-api delivery-api
   ```

4. **Tear down containers and volumes:**
   ```bash
   docker compose down -v
   ```

## 🏗️ System Architecture

```
                  +-----------------------+
                  |    Client / Postman   |
                  +-----------+-----------+
                              | Port 5000
                  +-----------v-----------+
                  |    YARP API Gateway   |
                  +-----------+-----------+
                              | Internal Docker Network
      +-----------------------+-----------------------+
      |                       |                       |
+-----v---------+     +-------v-------+     +---------v-----+
|  Identity.API |     | Restaurant.API|     |  Ordering.API |
|  (Auth & JWT) |     | (Catalog/Menu)|     | (CQRS/MediatR)|
+---------------+     +---------------+     +---------+-----+
                                                      |
                                             (OrderSubmittedEvent)
                                                      |
                                            +---------v-----+
                                            |    RabbitMQ   |
                                            +---------+-----+
                                                      |
                                              (Consumes Event)
                                                      |
                                            +---------v-----+
                                            |  Delivery.API |
                                            | (Driver Assign)|
                                            +---------------+
```

## 🔌 API Endpoints

### Identity API
| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | `/api/auth/register` | Register a new user |
| POST | `/api/auth/login` | Login and receive JWT token |

### Restaurant API
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/restaurants` | Get all restaurants |
| GET | `/api/restaurants/{id}` | Get restaurant by ID |
| POST | `/api/restaurants` | Create new restaurant |
| PUT | `/api/restaurants/{id}` | Update restaurant |
| DELETE | `/api/restaurants/{id}` | Delete restaurant |
| GET | `/api/restaurants/{id}/menu` | Get restaurant menu |

### Ordering API
| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | `/api/orders` | Create new order (publishes event) |
| GET | `/api/orders/{id}` | Get order by ID |
| GET | `/api/orders/user/{userId}` | Get orders by user |

### Delivery API
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/deliveries` | Get all deliveries |
| GET | `/api/deliveries/{id}` | Get delivery by ID |
| PUT | `/api/deliveries/{id}/status` | Update delivery status |
| POST | `/api/deliveries/{id}/assign` | Assign driver to delivery |

## 🔐 Authentication

All endpoints (except `/api/auth/*`) require JWT Bearer token authentication:

```http
Authorization: Bearer <your-jwt-token>
```

## 📝 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 🤝 Contributing

Contributions are welcome! Please feel free to submit a Pull Request.
