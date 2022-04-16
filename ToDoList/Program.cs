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

builder.Services.AddSingleton<IDbConnection>(AppDbContext.GetDbConnection(builder.Environment.IsDevelopment()));
builder.Services.AddSingleton(typeof(IBaseRepository<>), typeof(BaseDapperRepository<>));
builder.Services.AddSingleton<ToDoRepository>();
builder.Services.AddSingleton<CategoryRepository>();
builder.Services.AddSingleton<UserRepository>();

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
