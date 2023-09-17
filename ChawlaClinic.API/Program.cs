using ChawlaClinic.API;
using ChawlaClinic.BL.ServiceInterfaces;
using ChawlaClinic.BL.Services;
using ChawlaClinic.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
//using MySql.EntityFrameworkCore.Extensions;

var builder = WebApplication.CreateBuilder(args);

ServiceExtensions.AddServiceScopes(builder.Services);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//builder.Services.AddSession(options =>
//{
//    options.IdleTimeout = TimeSpan.FromMinutes(Convert.ToDouble(builder.Configuration.GetSection("Session:IdleTimeoutMinutes").Value));
//    options.Cookie.HttpOnly = true;
//});

//builder.Services.AddScoped<IUserServiceRepo, UserServiceRepo>();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySQL(builder.Configuration.GetConnectionString("MySQLCon")));


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseSession();

app.Run();
