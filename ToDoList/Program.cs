using Microsoft.AspNetCore.Authentication.Cookies;
using System.Data;
using ToDoList;
using ToDoList.Controllers;
using ToDoList.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = new PathString("/Account/Login");
        options.AccessDeniedPath = new PathString("/Account/AccessDenied");
    });
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IDbConnection>(options => AppDbContext.GetDbConnection(builder.Environment.IsDevelopment()));
//builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseDapperRepository<>));
builder.Services.AddScoped<ToDoRepository>();
builder.Services.AddScoped<CategoryRepository>();
builder.Services.AddScoped<UserRepository>();

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
