using AutoMapper;
using EvilCorp2000Products.Profiles;
using EvilCorp2000Products.Services;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Serilog;

//serilog configuration
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.File("logs/productsLog.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

//use serilog as logger
builder.Host.UseSerilog();

builder.Services.AddDbContext<EvilCorp2000Products.DbContexts.ProductsContext>(
    DbContextOptions => DbContextOptions.UseSqlServer(
        builder.Configuration["ConnectionStrings:EvilCorpProducts"]));

builder.Services.AddControllers(options =>
    { options.ReturnHttpNotAcceptable = true; })
    .AddNewtonsoftJson()
    .AddXmlDataContractSerializerFormatters();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IProductRepository, ProductRepository>();

builder.Services.AddSingleton(new MapperConfiguration(m =>
{
    m.AddProfile(new ProductProfile());
}).CreateMapper());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//where endpoint is selected
app.UseRouting();

app.UseAuthorization();

//where endpoint is executed
app.MapControllers();

app.Run();
