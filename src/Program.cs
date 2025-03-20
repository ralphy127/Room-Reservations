using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using MongoDB.Driver;
using RoomRes.Domain.Models;
using RoomRes.Domain.Interfaces;
using RoomRes.Infrastructure.Repositories;
using RoomRes.Application.Services;
using RoomRes.Infrastructure.Seeders;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews().AddRazorOptions(options => {
        options.ViewLocationFormats.Add("Presentation/Views/{1}/{0}.cshtml");
        options.ViewLocationFormats.Add("Presentation/Views/Shared/{0}.cshtml");
    });

if(builder?.Configuration is null) {
    throw new NullReferenceException(nameof(builder));
}

MongoDBSettings? mongoDBSettings = builder.Configuration.GetSection("MongoDBSettings").Get<MongoDBSettings>();
builder.Services.Configure<MongoDBSettings>(builder.Configuration.GetSection("MongoDBSettings"));
if(mongoDBSettings is null) {
    throw new NullReferenceException(nameof(mongoDBSettings));
}

builder.Services.AddSingleton<IMongoClient>(sp => new MongoClient(mongoDBSettings.AtlasURI));
builder.Services.AddScoped<IMongoDatabase>(sp => {
    var client = sp.GetRequiredService<IMongoClient>();
    return client.GetDatabase(mongoDBSettings.DbName);
});


builder.Services.AddDbContext<RoomReservationDbContext>(
    options => options.UseMongoDB(mongoDBSettings.AtlasURI ?? throw new ArgumentNullException(nameof(mongoDBSettings.AtlasURI)), mongoDBSettings.DbName ?? throw new ArgumentNullException(nameof(mongoDBSettings.DbName)))); 

builder.Services.AddSession(options => {
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
builder.Services.AddScoped(typeof(IBaseService<>), typeof(BaseService<>));

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRoomRepository, RoomRepository>();
builder.Services.AddScoped<IRoomReservationRepository, RoomReservationRepository>();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IRoomService, RoomService>();
builder.Services.AddScoped<IRoomReservationService, RoomReservationService>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options => {
        options.LoginPath = "/User/Login";
    });

WebApplication app = builder.Build();

if (!app.Environment.IsDevelopment()) {
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseSession();

using(IServiceScope scope = app.Services.CreateScope()) {
    IServiceProvider services = scope.ServiceProvider;
    IMongoDatabase database = services.GetRequiredService<IMongoDatabase>();
    DataSeeder dataSeeder = new DataSeeder(database);
    
    await dataSeeder.SeedAsync();
}

app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}").WithStaticAssets();

app.Run();