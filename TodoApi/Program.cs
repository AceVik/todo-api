using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.OpenApi;
using TodoApi.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

// Add services to the container.
builder.Services.AddDbContext<ToDoContext>(options =>
    options.UseInMemoryDatabase("TodoListInMemory"));

builder.Services.AddControllers();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi(); // Accessabel via /openapi/v1.json
}

// Middlewares
app.UseAuthorization();

// Endpoints
app.MapControllers();

// Run
app.Run();