using BuyurtmaGo.Core;
using BuyurtmaGo.Core.Authentications.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder => builder.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader());
});

builder.Services.AddRouting(options =>
{
    options.LowercaseUrls = true;
});

builder.Services.AddDbContext<BuyurtmaGoDb>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString(nameof(BuyurtmaGoDb)), o =>
    {
        o.MigrationsHistoryTable("migrations", "sys");
    });
    options.UseSnakeCaseNamingConvention();
});

builder.Services.AddIdentity<User, Role>()
    .AddEntityFrameworkStores<BuyurtmaGoDb>()
    .AddDefaultTokenProviders();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
