using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Restaurant.API.Middleware;
using Restaurant.Application.Behaviors;
using Restaurant.Application.Commands;
using Restaurant.Application.Interfaces;
using Restaurant.Application.Validators;
using Restaurant.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

// 1. Core API Services & Global Exception Handling
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

// 2. Register FluentValidation & MediatR Pipeline Behavior
builder.Services.AddValidatorsFromAssemblyContaining<CreateRestaurantCommandValidator>();
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

// 3. Register MediatR
builder.Services.AddMediatR(cfg => 
    cfg.RegisterServicesFromAssembly(typeof(CreateRestaurantCommand).Assembly));

// 4. Register DbContext with PostgreSQL
builder.Services.AddDbContext<RestaurantDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IRestaurantDbContext>(provider => 
    provider.GetRequiredService<RestaurantDbContext>());

var app = builder.Build();

// Activate Exception Middleware
app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();