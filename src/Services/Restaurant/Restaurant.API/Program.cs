using Microsoft.EntityFrameworkCore;
using Restaurant.Application.Commands;
using Restaurant.Application.Interfaces;
using Restaurant.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

// 1. Add Services to DI Container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 2. Register MediatR (scans Application assembly for handlers)
builder.Services.AddMediatR(cfg => 
    cfg.RegisterServicesFromAssembly(typeof(CreateRestaurantCommand).Assembly));

// 3. Register DbContext with PostgreSQL
builder.Services.AddDbContext<RestaurantDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// 4. Bind IRestaurantDbContext interface to concrete DbContext implementation
builder.Services.AddScoped<IRestaurantDbContext>(provider => 
    provider.GetRequiredService<RestaurantDbContext>());

var app = builder.Build();

// Configure HTTP Request Pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();