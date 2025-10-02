using FIN.Repository;
using FIN.Service.ToolService;
using FIN.Service.UserService;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Database Connection
builder.Services.AddDbContext<FinContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("FinDb")));

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IToolService, ToolService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
