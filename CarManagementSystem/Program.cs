using CarManagementSystem.DataAccess;
using CarManagementSystem.DataAccess.Repositories.Interfaces;
using CarManagementSystem.DataAccess.Repositories.Services;
using CarManagementSystem.Services.Interfaces;
using CarManagementSystem.Services.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

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

// ==================== Service ====================
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ICarCompanyService, CarCompanyService>();
builder.Services.AddScoped<IVehicleCategoryService, VehicleCategoryService>();
builder.Services.AddScoped<IElectricVehicleService, ElectricVehicleService>();
builder.Services.AddScoped<IPromotionService, PromotionService>();
builder.Services.AddScoped<IFeedbackService, FeedbackService>();


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
