using BMOS.BAL.DTOs.JWT;
using BMOS.BAL.Validators.Accounts;
using FluentValidation;
using GenZStyleApp.DAL.Models;
using GenZStyleAPP.BAL.DTOs.Account;
using GenZStyleAPP.BAL.DTOs.FireBase;
using GenZStyleAPP.BAL.DTOs.Users;
using GenZStyleAPP.BAL.Profiles.Accounts;
using GenZStyleAPP.BAL.Profiles.Users;
using GenZStyleAPP.BAL.Repository.Implementations;
using GenZStyleAPP.BAL.Repository.Interfaces;
using GenZStyleAPP.BAL.Validators.Accounts;
using GenZStyleAPP.BAL.Validators.Users;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OData.ModelBuilder;
using Microsoft.OpenApi.Models;
using ProjectParticipantManagement.BAL.DTOs.Authentications;
using ProjectParticipantManagement.BAL.Repositories.Interfaces;
using ProjectParticipantManagement.DAL.Infrastructures;
using System.Reflection;
using System.Text;

namespace GenZStyleApp_API {
    public class Program { 
    
    public static void Main(string[] args)
    {
    var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            /*#region JWT 
            builder.Services.AddSwaggerGen(options =>
            {
                // using System.Reflection;
                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));

                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "FU BMOS Shop Application API",
                    Description = "JWT Authentication API"
                });

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
                });
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
                        new string[]{}
                    }
                });
            });
            

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,

                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtAuth:Key"])),
                    ValidateLifetime = false,
                    ClockSkew = TimeSpan.Zero
                };
            });
            #endregion*/
    builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
                    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
                });
            ;
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
            //ODATA
            var modelBuilder = new ODataConventionModelBuilder();
            modelBuilder.EntitySet<GetUserResponse>("Users");
            modelBuilder.EntitySet<GetAccountResponse>("Accounts");
            modelBuilder.EntitySet<GetLoginResponse>("Authentications");

            builder.Services.AddControllers().AddOData(options => options.Select()
                                                                         .Filter()
                                                                         .OrderBy()
                                                                         .Expand()
                                                                         .Count()
                                                                         .SetMaxTop(null)
                                                                         .AddRouteComponents("odata", modelBuilder.GetEdmModel()));


            //Dependency Injections
            builder.Services.Configure<JwtAuth>(builder.Configuration.GetSection("JwtAuth"));

            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IAccountRepository, AccountRepository>();
            builder.Services.AddScoped<IAuthenticationRepository, AuthenticationRepository>();
            builder.Services.Configure<FireBaseImage>(builder.Configuration.GetSection("FireBaseImage"));
            //DI Validator
            builder.Services.AddScoped<IValidator<RegisterRequest>, RegisterValidation>();
            builder.Services.AddScoped<IValidator<GetLoginRequest>, LoginValidation>();
            builder.Services.AddScoped<IValidator<ChangePasswordRequest>, ChangePasswordValidation>();
            builder.Services.AddScoped<IValidator<UpdateUserRequest>, UpdateUserValidation>();

            builder.Services.Configure<FireBaseImage>(builder.Configuration.GetSection("FireBaseImage"));

            // Auto mapper config
            builder.Services.AddAutoMapper(typeof(AccountProfile),
                                            typeof(CustomerProfile)
                                            );
                                            

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

}
    } 
}



