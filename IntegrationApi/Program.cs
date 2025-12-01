using CalibrationSaaS.Data.EntityFramework;
using Microsoft.EntityFrameworkCore;
using CalibrationSaaS.Domain.Repositories;
using CalibrationSaaS.Infraestructure.EntityFramework.DataAccess;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


 //builder.Services.AddDbContext<CalibrationSaaSDBContext>(options =>
 //               options.UseSqlServer(Microsoft.Extensions.Configuration
 //               .ConfigurationExtensions.GetConnectionString(builder.Configuration, "DefaultConnection"))
 //               .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
 //               , ServiceLifetime.Transient);



builder.Services.AddDbContextFactory<CalibrationSaaSDBContext>(options =>
               options.UseSqlServer(Microsoft.Extensions.Configuration
               .ConfigurationExtensions.GetConnectionString(builder.Configuration, "DefaultConnection"), b => b.MigrationsAssembly("CalibrationSaaS.Infraestructure.GrpcServices"))
               .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
               , ServiceLifetime.Transient);


builder.Services.AddScoped<CalibrationSaaSDBContext, CalibrationSaaSDBContext>();
builder.Services.AddScoped<IUOMRepository, UOMRepositoryEF<CalibrationSaaSDBContext>>();
builder.Services.AddScoped<IBasicsRepository, BasicsRepositoryEF<CalibrationSaaSDBContext>>();


builder.Services.AddTransient<CalibrationSaaS.Domain.Repositories.IIdentityRepository, IdentityClient.IdentityClient>();

builder.Services.AddTransient<CalibrationSaaS.Domain.Repositories.IPieceOfEquipmentRepository, CalibrationSaaS.Infraestructure.EntityFramework.DataAccess.PieceOfEquipmentRepositoryEF<CalibrationSaaSDBContext>>();

builder.Services.AddTransient<CalibrationSaaS.Domain.Repositories.IBasicsRepository, CalibrationSaaS.Infraestructure.EntityFramework.DataAccess.BasicsRepositoryEF<CalibrationSaaSDBContext>>();

builder.Services.AddTransient<CalibrationSaaS.Domain.Repositories.IAssetsRepository, CalibrationSaaS.Infraestructure.EntityFramework.DataAccess.AssetsRepositoryEF<CalibrationSaaSDBContext>>();

builder.Services.AddTransient<CalibrationSaaS.Domain.Repositories.ICustomerRepository, CalibrationSaaS.Infraestructure.EntityFramework.DataAccess.CustomerRepositoryEF<CalibrationSaaSDBContext>>();

builder.Services.AddTransient<CalibrationSaaS.Domain.Repositories.IWorkOrderDetailRepository, CalibrationSaaS.Infraestructure.EntityFramework.DataAccess.WODRepositoryEF<CalibrationSaaSDBContext>>();

builder.Services.AddTransient<CalibrationSaaS.Application.UseCases.PieceOfEquipmentUseCases, CalibrationSaaS.Application.UseCases.PieceOfEquipmentUseCases>();

builder.Services.AddTransient<CalibrationSaaS.Application.UseCases.AssetsUseCases, CalibrationSaaS.Application.UseCases.AssetsUseCases>();

builder.Services.AddTransient<CalibrationSaaS.Application.UseCases.CustomerUseCases, CalibrationSaaS.Application.UseCases.CustomerUseCases>();

builder.Services.AddTransient<CalibrationSaaS.Application.UseCases.WorkOrderDetailUseCase, CalibrationSaaS.Application.UseCases.WorkOrderDetailUseCase>();


builder.Services.AddTransient<CalibrationSaaS.Application.UseCases.BasicsUseCases, CalibrationSaaS.Application.UseCases.BasicsUseCases>();





builder.Services.AddHttpClient();


//webBuilder.UseIISIntegration();

builder.WebHost.UseIISIntegration();
var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
app.UseSwagger();
app.UseSwaggerUI();
//}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
