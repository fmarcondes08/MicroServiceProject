# Microservices Project

## Overview
This project implements a simple event-driven application using C# and .NET 6+. The application consists of three microservices: `UserService`, `OrderService`, and `NotificationService`, which interact with each other through RabbitMQ and integrate with a PostgreSQL database for basic CRUD operations.

## Requirements
- Docker and Docker Compose
- .NET 6 SDK

## Setup

### Clone the Repository
First, clone the repository to your local machine:
```bash
git clone <repository-url>
cd MicroservicesProject
```

### Environment Configuration
Make sure your `appsettings.json` in each microservice is properly configured, especially the RabbitMQ and PostgreSQL settings. Here is an example for the `NotificationService`:

#### **UserService/appsettings.json**
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=postgres;Database=microservicesdb;Username=admin;Password=password"
  }
}
```

#### **OrderService/appsettings.json**
```json
{
    "ConnectionStrings": {
    "DefaultConnection": "Host=postgres;Database=microservicesdb;Username=admin;Password=password"
  },
  "RabbitMQ": {
    "Host": "rabbitmq",
    "Port": "5672",
    "UserName": "guest",
    "Password": "guest",
    "QueueName": "orderQueue"
  }
}
```

#### **NotificationService/appsettings.json**
```json
{
  "RabbitMQ": {
    "Host": "rabbitmq",
    "Port": "5672",
    "UserName": "guest",
    "Password": "guest",
    "QueueName": "orderQueue"
  }
}
```

### Build and Run the Docker Containers
Use Docker Compose to build and run the containers:
```bash
docker-compose up --build
```

### Access the RabbitMQ Management Interface
- **URL**: `http://localhost:15672`
- **Username**: `guest`
- **Password**: `guest`

### API Endpoints

#### UserService
- **GET /api/users**: Get all users.
- **GET /api/users/{id}**: Get user by ID.
- **POST /api/users**: Create a new user.
- **PUT /api/users/{id}**: Update user by ID.
- **DELETE /api/users/{id}**: Delete user by ID.

#### OrderService
- **GET /api/orders**: Get all orders.
- **GET /api/orders/{id}**: Get order by ID.
- **POST /api/orders**: Create a new order.
- **PUT /api/orders/{id}**: Update order by ID.
- **DELETE /api/orders/{id}**: Delete order by ID.

## Justification of Design Decisions

### Microservices Architecture
The application is divided into three microservices: `UserService`, `OrderService`, and `NotificationService`. This design allows each service to be developed, deployed, and scaled independently, improving maintainability and scalability.

### Event-Driven Architecture
RabbitMQ is used for communication between the services. When an order is created, the `OrderService` publishes an event to RabbitMQ, which is then consumed by the `NotificationService`. This decouples the services, allowing them to operate asynchronously and independently.

### Docker
Each microservice is containerized using Docker. This ensures a consistent environment across different development and production setups. Docker Compose is used to orchestrate the multi-container setup, making it easy to manage and run the entire application stack.

### Database Interaction
PostgreSQL is used as the database for storing user and order data. Entity Framework Core (EF Core) is used as the ORM to interact with the database, providing efficient and secure database operations.

### Validation and Error Handling
The application includes validation checks to ensure data integrity and proper error handling. For instance, the `UserService` and `OrderService` both include methods to check if entities exist before performing operations, and appropriate HTTP status codes are returned based on the validation results.

### Dependency Injection
The application uses dependency injection, a core feature of ASP.NET Core, to manage dependencies. This makes the code more modular, testable, and easier to maintain.

## Contributing
Pull requests are welcome. For major changes, please open an issue first to discuss what you would like to change.

## License
[MIT](https://choosealicense.com/licenses/mit/)

By following these instructions and understanding the design decisions, users can set up and run the microservices application effectively, while also gaining insight into the architectural choices made during development.
