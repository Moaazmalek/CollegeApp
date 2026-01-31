using AutoMapper;
using CollegeApp.Configurations;
using CollegeApp.Data;
using CollegeApp.Repositories.Implementations;
using CollegeApp.Repositories.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;
using Microsoft.IdentityModel.Tokens;
using System.Text;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<CollegeDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

//Log.Logger = new LoggerConfiguration()
//    .MinimumLevel.Information()
//    .WriteTo.File("Log/log.txt",rollingInterval:RollingInterval.Day)
//    .CreateLogger();

//builder.Host.UseSerilog();

builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "College API",
        Version = "v1",
        Description = "API لإدارة طلاب الكلية"
    });
});

builder.Services.AddAutoMapper(cfg => { }, typeof(AutoMapperConfig));
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped(typeof(IStudentRepository), typeof(StudentRepository));

////named policy
//builder.Services.AddCors(options => options.AddPolicy("MyTestCORS", policy =>
//{
//    //policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();

//    //allow only particular origins
//    //policy.WithOrigins("http://localhost:4200").AllowAnyHeader().AllowAnyMethod();

//    //



//}));
//default policy
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
   
    options.AddPolicy("AllowOnlyLocalhost", policy =>
    {
        policy.WithOrigins("http://localhost:4200").AllowAnyHeader().AllowAnyMethod();
    });

    options.AddPolicy("AllowOnlyGoogle", policy =>
    {
        policy.WithOrigins("https://www.google.com","https://www.gmail.com").AllowAnyHeader().AllowAnyMethod();
    });
    options.AddPolicy("OnlyMicrosoft", policy =>
    {
        policy.WithOrigins("https://www.microsoft.com","https://www.bing.com").AllowAnyHeader().AllowAnyMethod();
    });


});

// JWT Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

})
.AddJwtBearer(options =>
{
    // Not recommended for production
    options.RequireHttpsMetadata = false;
    //Save token in HttpContext
    options.SaveToken = true;
    var jwtSecret = builder.Configuration.GetValue<string>("JWTSecret");
    if (string.IsNullOrEmpty(jwtSecret))
    {
        throw new InvalidOperationException("JWTSecret configuration value is missing or empty.");
    }
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSecret)),
        ValidateIssuer = false,
        ValidateAudience = false

    };
});

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json","College API v1");
        c.RoutePrefix = string.Empty;
    });
}

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
endpoints.MapControllers();
    endpoints.MapGet("api/Testendpoint", context => context.Response.WriteAsync(builder.Configuration.GetValue<string>("JWTSecret")));
});

app.Run();

