using FruitAPI.BusinessLogic.Fruit;
using FruitAPI.BusinessLogic.FruitType;
using FruitAPI.DataAccess.Context;
using FruitAPI.DataAccess.Repository.Fruit;
using FruitAPI.DataAccess.Repository.FruitType;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContextFactory<DataContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Context")
        ?? throw new InvalidOperationException("Connection string 'Context' not found."));
});

builder.Services
    .AddControllers()
    .AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Context")
        ?? throw new InvalidOperationException("Connection string 'Context' not found."));
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(Program).Assembly);
builder.Services.AddScoped<IFruitRepository, FruitRepository>();
builder.Services.AddScoped<IFruitTypeRepository, FruitTypeRepository>();
builder.Services.AddScoped<IBLFruit, BLFruit>();
builder.Services.AddScoped<IBLFruitType, BLFruitType>();


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
