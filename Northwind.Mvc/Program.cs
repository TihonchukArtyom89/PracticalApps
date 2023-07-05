//Section 1 - import namespaces
using Microsoft.AspNetCore.Identity;//IdentityUser
using Microsoft.EntityFrameworkCore;//USeSqlServer
using Northwind.Mvc.Data;//ApplicationDbContext
using Packt.Shared;//AddNorthwindContext extension method
using System.Net.Http.Headers;
//Section 2 - configure the host web server builder including services
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()//enable role management
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();
builder.Services.AddNorthwindContext();
builder.Services.AddOutputCache(options => { options.DefaultExpirationTimeSpan = TimeSpan.FromSeconds(20); options.AddPolicy("views", p => p.SetVaryByQuery("alertstyle")); });
builder.Services.AddHttpClient(name: "Northwind.WebApi",
    configureClient: options =>
    {
        options.BaseAddress = new Uri("https://localhost:5002/");
        options.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(mediaType: "application/json", quality: 1.0));
    });
builder.Services.AddHealthChecks();
builder.Services.AddHttpClient(name:"Minimal.WebApi",configureClient: options => 
{
    options.BaseAddress = new Uri("https://localhost:5003/");
    options.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(mediaType: "application/json", quality: 1.0));
});
var app = builder.Build();

// Section 3 - configure the HTTP request pipeline.
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

app.UseAuthentication();
app.UseAuthorization();
app.UseOutputCache();
app.UseHealthChecks(path: "/howdoyoufeel");
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}").CacheOutput("views");
app.MapRazorPages();
app.MapGet("/nocached", () => DateTime.Now.ToString());
app.MapGet("/cached", () => DateTime.Now.ToString()).CacheOutput();
//Section 4 - start the host web server listening for HTTP requests
app.Run();//blocking call
