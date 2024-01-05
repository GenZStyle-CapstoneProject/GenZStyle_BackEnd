using FluentValidation;
using GenZStyleApp.DAL.Models;
using GenZStyleAPP.BAL.DTOs.FireBase;
using GenZStyleAPP.BAL.DTOs.Users;
using GenZStyleAPP.BAL.Profiles.Accounts;
using GenZStyleAPP.BAL.Profiles.Users;
using GenZStyleAPP.BAL.Repository.Implementations;
using GenZStyleAPP.BAL.Repository.Interfaces;
using GenZStyleAPP.BAL.Validators.Accounts;
using GenZStyleAPP.BAL.Validators.Users;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.OData.ModelBuilder;
using ProjectParticipantManagement.BAL.DTOs.Authentications;
using ProjectParticipantManagement.BAL.Repositories.Interfaces;
using ProjectParticipantManagement.DAL.Infrastructures;

namespace GenZStyleApp_API {
    public class Program { 
    
    public static void Main(string[] args)
    {
    var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

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
            
            modelBuilder.EntitySet<GetLoginResponse>("Authentications");

            builder.Services.AddControllers().AddOData(options => options.Select()
                                                                         .Filter()
                                                                         .OrderBy()
                                                                         .Expand()
                                                                         .Count()
                                                                         .SetMaxTop(null)
                                                                         .AddRouteComponents("odata", modelBuilder.GetEdmModel()));


            //Dependency Injections
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IAuthenticationRepository, AuthenticationRepository>();
            builder.Services.Configure<FireBaseImage>(builder.Configuration.GetSection("FireBaseImage"));
            //DI Validator
            builder.Services.AddScoped<IValidator<RegisterRequest>, RegisterValidation>();
            builder.Services.AddScoped<IValidator<GetLoginRequest>, LoginValidation>();
            builder.Services.AddScoped<IValidator<UpdateUserRequest>, UpdateUserValidation>();


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



