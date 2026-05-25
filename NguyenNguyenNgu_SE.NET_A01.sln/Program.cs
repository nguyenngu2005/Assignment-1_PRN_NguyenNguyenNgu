using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

//Subscribe DbContext
builder.Services.AddDbContext<Repositories.Models.FunewsManagementContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<Repositories.Repositories.ISystemAccountRepository, Repositories.Repositories.SystemAccountRepository>();
builder.Services.AddScoped<Services.ISystemAccountService, Services.SystemAccountService>();
// Đăng ký cho Category
builder.Services.AddScoped<Repositories.Repositories.ICategoryRepository, Repositories.Repositories.CategoryRepository>();
builder.Services.AddScoped<Services.ICategoryService, Services.CategoryService>();
// Đăng ký cho NewsArticle
builder.Services.AddScoped<Repositories.Repositories.INewsArticleRepository, Repositories.Repositories.NewsArticleRepository>();
builder.Services.AddScoped<Services.INewsArticleService, Services.NewsArticleService>();

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Thời gian Session tồn tại
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseSession();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}");

app.Run();
