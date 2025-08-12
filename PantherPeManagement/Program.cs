using BLL.Services.Implements;
using BLL.Services.Interfaces;
using DAL;
using DAL.Repositories.Implements;
using DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddDbContext<Su25pantherDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IPantherAccountRepository, PantherAccountRepository>();
builder.Services.AddScoped<IPantherProfileRepository, PantherProfileRepository>();
builder.Services.AddScoped<IPantherTypeRepository, PantherTypeRepository>();
builder.Services.AddScoped<IPantherAccountService, PantherAccountService>();
builder.Services.AddScoped<IPantherProfileService, PantherProfileService>();
builder.Services.AddScoped<IPantherTypeService, PantherTypeService>();

// --------------------------------------------------------
builder.Services.AddDistributedMemoryCache();  // Cho session 
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login";          // Redirect when unauthenticated
        options.AccessDeniedPath = "/AccessDenied"; // (Create this page if needed)
    });
builder.Services.AddAuthorization();
// --------------------------------------------------------

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// Use session before auth so you can access session data in auth events
app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.Run();
