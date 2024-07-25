# Use the .NET SDK to build the project
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

# Copy csproj files and restore dependencies for all services
COPY UserService/UserService.csproj UserService/
COPY OrderService/OrderService.csproj OrderService/
COPY NotificationService/NotificationService.csproj NotificationService/
COPY Shared/Shared.csproj Shared/
RUN dotnet restore "UserService/UserService.csproj"
RUN dotnet restore "OrderService/OrderService.csproj"
RUN dotnet restore "NotificationService/NotificationService.csproj"

# Copy all the source files
COPY . .

# Build each service separately
RUN dotnet build "UserService/UserService.csproj" -c Release -o /app/userbuild
RUN dotnet build "OrderService/OrderService.csproj" -c Release -o /app/orderbuild
RUN dotnet build "NotificationService/NotificationService.csproj" -c Release -o /app/notificationbuild

# Publish each service
FROM build AS publish
RUN dotnet publish "UserService/UserService.csproj" -c Release -o /app/userpublish /p:UseAppHost=false
RUN dotnet publish "OrderService/OrderService.csproj" -c Release -o /app/orderpublish /p:UseAppHost=false
RUN dotnet publish "NotificationService/NotificationService.csproj" -c Release -o /app/notificationpublish /p:UseAppHost=false

# Create the final runtime images for each service
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS userruntime
WORKDIR /app
COPY --from=publish /app/userpublish .
ENTRYPOINT ["dotnet", "UserService.dll"]

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS orderruntime
WORKDIR /app
COPY --from=publish /app/orderpublish .
ENTRYPOINT ["dotnet", "OrderService.dll"]

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS notificationruntime
WORKDIR /app
COPY --from=publish /app/notificationpublish .
ENTRYPOINT ["dotnet", "NotificationService.dll"]
