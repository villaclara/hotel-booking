# Use the official .NET 8 SDK image for building
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

# Copy solution and project files
COPY ["hotel-booking.sln", "."]  
COPY ["src/HotelBooking.Domain/HotelBooking.Domain.csproj", "HotelBooking.Domain/"]  
COPY ["src/HotelBooking.Application/HotelBooking.Application.csproj", "HotelBooking.Application/"]  
COPY ["src/HotelBooking.Infrastructure/HotelBooking.Infrastructure.csproj", "HotelBooking.Infrastructure/"]  
COPY ["src/HotelBooking.API/HotelBooking.API.csproj", "HotelBooking.API/"]

# Restore dependencies
RUN dotnet restore "HotelBooking.API/HotelBooking.API.csproj"

# Copy all source code
COPY ["src/HotelBooking.Domain/", "HotelBooking.Domain/"]
COPY ["src/HotelBooking.Application/", "HotelBooking.Application/"]
COPY ["src/HotelBooking.Infrastructure/", "HotelBooking.Infrastructure/"]
COPY ["src/HotelBooking.API/", "HotelBooking.API/"]

# Build the project
RUN dotnet build "HotelBooking.API/HotelBooking.API.csproj" -c Release -o /app/build

# Publish the project
RUN dotnet publish "HotelBooking.API/HotelBooking.API.csproj" -c Release -o /app/publish

# Use the runtime image for final container
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

# Set environment variable for production
ENV DOTNET_RUNNING_IN_CONTAINER=true
ENV DOTNET_USE_POLLING_FILE_WATCHER=1

ENTRYPOINT ["dotnet", "HotelBooking.API.dll"]
