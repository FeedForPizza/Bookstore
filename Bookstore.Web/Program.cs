using Bookstore.Data.Contexts;
using Bookstore.Entities;
using Bookstore.Services;
using Bookstore.Services.External;
using Bookstore.Services.Mapping;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<BookstoreDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddHttpClient("OpenLibrary", c =>
{
    c.BaseAddress = new Uri("https://openlibrary.org/api/");
});
builder.Services.AddHttpClient("GoogleBooks", c =>
{
    c.BaseAddress = new Uri("https://www.googleapis.com/books/v1/");
});
builder.Services.AddDbContext<AuthDbContext>(options =>
{

    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 5;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
})
.AddEntityFrameworkStores<AuthDbContext>();
// Add services to the container.


builder.Services.AddRazorPages();
builder.Services.AddScoped<BookstoreServices>();
builder.Services.AddScoped<OpenLibraryService>();
builder.Services.AddScoped<GoogleBooksService>();
builder.Services.AddScoped<OrdersService>();
builder.Services.AddAutoMapper(typeof(MappingProfile));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.UseAuthentication();
app.MapRazorPages();

app.Run();
