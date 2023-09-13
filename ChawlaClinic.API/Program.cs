using ChawlaClinic.API;
using ChawlaClinic.BL.ServiceInterfaces;
using ChawlaClinic.BL.Services;

var builder = WebApplication.CreateBuilder(args);

ServiceExtensions.AddServiceScopes(builder.Services);

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(Convert.ToDouble(builder.Configuration.GetSection("Session:IdleTimeoutMinutes").Value));
    options.Cookie.HttpOnly = true;
});

//builder.Services.AddScoped<IUserServiceRepo, UserServiceRepo>();

var app = builder.Build();

app.UseSession();

app.Run();
