using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using OmgvaPOS.AuthManagement.Repository;
using OmgvaPOS.BusinessManagement.Repository;
using OmgvaPOS.Database.Context;
using OmgvaPOS.Middleware;
using OmgvaPOS.ReservationManagement.Repository;
using OmgvaPOS.ReservationManagement.Service;
using OmgvaPOS.TaxManagement.Repository;
using OmgvaPOS.UserManagement.Repository;
using OmgvaPOS.AuthManagement.Service;
using OmgvaPOS.CustomerManagement.Repository;
using OmgvaPOS.CustomerManagement.Service;
using OmgvaPOS.UserManagement.Service;
using OmgvaPOS.ItemManagement.Repositories;
using OmgvaPOS.TaxManagement.Services;
using OmgvaPOS.ItemManagement.Services;
using OmgvaPOS.ItemVariationManagement.Repositories;
using OmgvaPOS.ItemVariationManagement.Services;
using OmgvaPOS.GiftcardManagement.Repository;
using OmgvaPOS.GiftcardManagement.Service;
using OmgvaPOS.DiscountManagement.Service;
using OmgvaPOS.DiscountManagement.Repository;
using OmgvaPOS.BusinessManagement.Services;

var builder = WebApplication.CreateBuilder(args);
var initDatabaseAction = DbInitializerAction.DoNothing;

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(options => {
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
    });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "OMGVA POS API",
        Version = "v1"
    });

    // Define security scheme
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Type = SecuritySchemeType.ApiKey,
        Name = "Authorization",
        In = ParameterLocation.Header,
        Scheme = "Bearer",
        BearerFormat = "JWT"
    });
  // Apply security to endpoints
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                Array.Empty<string>()
            }
        });
});


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAuthenticationRepository, AuthenticationRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IBusinessService, BusinessService>();
builder.Services.AddScoped<IBusinessRepository, BusinessRepository>();
builder.Services.AddScoped<IItemService, ItemService>();
builder.Services.AddScoped<IItemRepository, ItemRepository>();
builder.Services.AddScoped<IItemVariationService, ItemVariationService>();
builder.Services.AddScoped<IItemVariationRepository, ItemVariationRepository>();
builder.Services.AddScoped<ITaxService, TaxService>();
builder.Services.AddScoped<ITaxRepository, TaxRepository>();
builder.Services.AddScoped<ITaxItemRepository, TaxItemRepository>();
builder.Services.AddScoped<IGiftcardRepository, GiftcardRepository>();
builder.Services.AddScoped<IGiftcardService, GiftcardService>();
builder.Services.AddScoped<IDiscountService, DiscountService>();
builder.Services.AddScoped<IDiscountRepository, DiscountRepository>();
builder.Services.AddScoped<IReservationRepository, ReservationRepository>();
builder.Services.AddScoped<IReservationService, ReservationService>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<ICustomerService, CustomerService>();

//in case you want to use cloud database
//go into appsettings.json and set "UseCloudDatabase": true
bool useCloudBool = builder.Configuration.GetValue<bool>("UseCloudDatabase"); 

var connectionString = useCloudBool ? 
    builder.Configuration.GetConnectionString("CloudDatabase")
    : builder.Configuration.GetConnectionString("LocalDatabase");

builder.Services.AddDbContext<OmgvaDbContext>(options => options.UseSqlServer(connectionString));

builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                      .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
                      .AddEnvironmentVariables();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})

        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = builder.Configuration["Jwt:Issuer"],
                ValidAudience = builder.Configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]))
            };
        });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    if (initDatabaseAction != DbInitializerAction.DoNothing)
    {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<OmgvaDbContext>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<DbInitializer>>();
        var dbInitializer = new DbInitializer(dbContext, logger);
        dbInitializer.InitDb(initDatabaseAction);
        logger.LogInformation($"Exiting after completing database action {initDatabaseAction}...");
        return;
    }
}

app.UseHttpsRedirection();

// Add exception handler before routing/endpoints
app.UseMiddleware<ExceptionMiddleware>();
app.UseRouting();
app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
