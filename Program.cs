//Testing
using FoodSaver.Contexts;
using FoodSaver.Repositories;
using FoodSaver.Repositories.Interfaces;
using FoodSaver.Services;
using FoodSaver.Services.Interfaces;
using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(options =>
{
    // API auth uses JWT
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
})
// Cookie used ONLY as a temporary store for external login (Google)
.AddCookie("External", options =>
{
    options.Cookie.Name = ".FoodSaver.External";
    options.Cookie.HttpOnly = true;
    options.Cookie.SameSite = SameSiteMode.Lax;
})
.AddGoogle(GoogleDefaults.AuthenticationScheme, options =>
{
    // This tells Google handler where to "sign in" the user after callback
    options.SignInScheme = "External";

    options.ClientId = builder.Configuration["Authentication:Google:ClientID"];
    options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
    // matches authorized redirect URI in Google console
    options.CallbackPath = "/signin-google";
});



//Hangfire configuration
builder.Services.AddHangfire(config => config.UsePostgreSqlStorage(
    options =>
    {

        options.UseNpgsqlConnection(builder.Configuration.GetConnectionString("DefaultConnection"));
    }
    ));
builder.Services.AddHangfireServer();

//Database and Dependency Injection.
builder.Services.AddDbContext<FoodSaverDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IFoodSaverRepository, FoodSaverRepository>();
builder.Services.AddScoped<IFoodSaverService, FoodSaverService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IUsersRepository, UsersRepoitory>();
builder.Services.AddScoped<IUsersService, UsersServices>();
builder.Services.AddScoped<IJWTService, JWTService>();

builder.Services.AddControllers()
    //Allows the conversion of enum from numbers to the actual options text,
    //as C# by default treats enum as numbers when converting to or from JSON.
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(
            new System.Text.Json.Serialization.JsonStringEnumConverter());
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// server: allow http://localhost:5173
builder.Services.AddCors(options =>
  options.AddPolicy("dev", p =>
    p.WithOrigins("http://localhost:5173", "https://saveit-frontend-klwdw.ondigitalocean.app")
     .AllowAnyHeader()
     .AllowAnyMethod()
     .AllowCredentials()));



var app = builder.Build();

app.UseCors("dev");

// Enable Hangfire Dashboard
app.UseHangfireDashboard("/hangfire");

//Create recurring job (once app starts)
RecurringJob.AddOrUpdate<IFoodSaverService>(
    "find_expiring_foods",                    // Job ID
    s => s.SendFoodExpiryReminder(3),              // Method to run
    Cron.Daily(11));                                   // Schedule: every day

//Create recurring job (once app starts)
RecurringJob.AddOrUpdate<IFoodSaverService>(
    "delete_expired_foods",                    // Job ID
    s => s.DeleteExpiredFood(),              // Method to run
    Cron.Daily(11));                                  // Schedule: every day

//Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
