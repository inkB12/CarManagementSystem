using CarManagementSystem.BusinessObjects;
using CarManagementSystem.DataAccess.Repositories.Implements;
using CarManagementSystem.DataAccess.Repositories.Interfaces;
using CarManagementSystem.DataAccess.Repositories.Services;
using CarManagementSystem.Services.Dtos.Momo;
using CarManagementSystem.Services.Implements;
using CarManagementSystem.Services.Interfaces;
using CarManagementSystem.Services.Services;
using CarManagementSystem.WebMVC.Configurations;
using Microsoft.EntityFrameworkCore;
using CloudinaryDotNet;

var builder = WebApplication.CreateBuilder(args);

// Kết nối Couldinary

builder.Services.Configure<CloudinarySettings>(
    builder.Configuration.GetSection("Cloudinary"));


var cloudinarySettings = builder.Configuration
                                .GetSection("Cloudinary")
                                .Get<CloudinarySettings>();
var account = new Account(
    cloudinarySettings.CloudName,
    cloudinarySettings.ApiKey,
    cloudinarySettings.ApiSecret);

var cloudinary = new Cloudinary(account);
builder.Services.AddSingleton(cloudinary);

// Kết nối Momo 
builder.Services.Configure<MomoOptionDTO>(builder.Configuration.GetSection("MomoApi"));


// Đăng ký DbContext
builder.Services.AddDbContext<CarManagementDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ==================== Repository ====================
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ICarCompanyRepository, CarCompanyRepository>();
builder.Services.AddScoped<IVehicleCategoryRepository, VehicleCategoryRepository>();
builder.Services.AddScoped<IElectricVehicleRepository, ElectricVehicleRepository>();
builder.Services.AddScoped<IPromotionRepository, PromotionRepository>();
builder.Services.AddScoped<IFeedbackRepository, FeedbackRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();

// ==================== Service ====================
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ICarCompanyService, CarCompanyService>();
builder.Services.AddScoped<IVehicleCategoryService, VehicleCategoryService>();
builder.Services.AddScoped<IElectricVehicleService, ElectricVehicleService>();
builder.Services.AddScoped<IPromotionService, PromotionService>();
builder.Services.AddScoped<IFeedbackService, FeedbackService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IMomoService, MomoService>();
builder.Services.AddScoped<IReportService, ReportService>();


// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // timeout 30'
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
