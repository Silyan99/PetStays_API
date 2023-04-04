using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PetStays_API.DBModels;
using PetStays_API.Interfaces;
using PetStays_API.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IPetStaysRepository, PetStaysRepository>();

builder.Services.AddDbContext<PetStaysContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("ConnectionStrings")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
