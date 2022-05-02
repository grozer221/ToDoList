using Microsoft.AspNetCore.Authentication.Cookies;
using MySql.Data.MySqlClient;
using System.Data;
using System.Data.SqlClient;
using ToDoList;
using ToDoList.Controllers;
using ToDoList.MsSql.Repositories;
using ToDoList.MySql.Repositories;
using ToDoList.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = new PathString("/Account/Login");
        options.AccessDeniedPath = new PathString("/Account/AccessDenied");
    });
builder.Services.AddControllersWithViews();

builder.Services.AddAutoMapper(typeof(AppMappingProfile));

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddScoped<IDbConnection>(options => new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ToDoList;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"));
    builder.Services.AddTransient<IToDoRepository, MSSqlToDoRepository>();
    builder.Services.AddTransient<ICategoryRepository, MSSqlCategoryRepository>();
    builder.Services.AddTransient<IUserRepository, MSSqlUserRepository>();
}
else
{
    builder.Services.AddScoped<IDbConnection>(options => new MySqlConnection(AppDbContext.ConvertMySqlConnectionString(Environment.GetEnvironmentVariable("JAWSDB_URL"))));
    builder.Services.AddTransient<IToDoRepository, MySqlToDoRepository>();
    builder.Services.AddTransient<ICategoryRepository, MySqlCategoryRepository>();
    builder.Services.AddTransient<IUserRepository, MySqlUserRepository>();
}

builder.Services.AddSingleton<EmailService>();
builder.Services.AddSingleton<AccountService>();


var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: $"{{controller={nameof(ToDosController).ReplaceInEnd("Controller", string.Empty)}}}/{{action={nameof(ToDosController.Index)}}}/{{id?}}");

app.Run();
