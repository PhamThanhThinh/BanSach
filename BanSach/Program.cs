using BanSach.Data;
using BanSach.Repo;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services
  .AddIdentity<IdentityUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
  .AddEntityFrameworkStores<ApplicationDbContext>()
  .AddDefaultUI()
  .AddDefaultTokenProviders();

builder.Services.AddControllersWithViews();

// trong tương lai sẽ khai báo service ở đây
// ...

// đăng ký dịch vụ IHomeRepository
builder.Services.AddTransient<IHomeRepository, HomeRepository>();
// đăng ký dịch vụ cho ICartRepository
builder.Services.AddTransient<ICartRepository, CartRepository>();



var app = builder.Build();

// chạy seedata
//using (var scope = app.Services.CreateScope())
//{
//  await SeedData.SeedDefaultData(scope.ServiceProvider);
//}

using (var scope = app.Services.CreateScope())
{
  await SeedData.SeedDefaultData(scope.ServiceProvider);
}

  // Configure the HTTP request pipeline.
  if (app.Environment.IsDevelopment())
  {
    app.UseMigrationsEndPoint();
  }
  else
  {
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
  }

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
