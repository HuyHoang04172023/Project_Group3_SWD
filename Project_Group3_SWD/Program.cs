using Project_Group3_SWD.Models;
using Project_Group3_SWD.Proxy;
using Project_Group3_SWD.Repositories;
using Project_Group3_SWD.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Cấu hình dịch vụ DbContext cho Entity Framework Core
builder.Services.AddDbContext<WebkinhdoanhquanaoContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

// Cấu hình các dịch vụ tùy chỉnh (Repository, Services, etc.)
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IBlogRepository, BlogRepository>();
builder.Services.AddScoped<IBlogService, BlogService>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IBrandRepository, BrandRepository>();
builder.Services.AddScoped<IBrandService, BrandService>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<GHNService>();

// Thêm các dịch vụ khác cần thiết cho ứng dụng (MVC, Razor Pages, etc.)
builder.Services.AddControllersWithViews();

// Thêm Session
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Thời gian tồn tại của session
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Sử dụng Session
app.UseSession();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

//config area
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
           name: "areas",
           pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}"
       );

    // Default route for controllers outside of areas
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}"
    );
});

app.Run();
