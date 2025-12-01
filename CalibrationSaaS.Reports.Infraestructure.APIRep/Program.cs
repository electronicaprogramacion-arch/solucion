using CalibrationSaaS.Reports.Infraestructure.APIRep;
using CalibrationSaaS.Reports.Infraestructure.APIRep.Extensions;

var app = Startup.startApp(args);
//var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddControllers();

//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

//// Add services to the container.
//builder.Services.AddControllersWithViews();

//var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (!app.Environment.IsDevelopment())
//{
//    app.UseExceptionHandler("/Home/Error");
//    //The default HSTS value is 30 days.You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
//    app.UseHsts();
//}

//app.UseHttpsRedirection();
//app.UseStaticFiles();

//app.UseRouting();

//app.UseAuthorization();
//app.PreparePuppeteerAsync(app.Environment).GetAwaiter().GetResult();
//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Home}/{action=Index}/{id?}");

//app.UseSwagger();
//app.UseSwaggerUI();

app.Run();
