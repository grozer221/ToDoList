using System.Data;
using ToDoList;
using ToDoList.Controllers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<IDbConnection>(AppDbContext.GetDbConnection(builder.Environment.IsDevelopment()));
builder.Services.AddSingleton(typeof(IBaseRepository<>), typeof(BaseDapperRepository<>));
builder.Services.AddSingleton<ToDoRepository>();
builder.Services.AddSingleton<CategoryRepository>();


var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: $"{{controller={nameof(ToDosController).ReplaceInEnd("Controller", string.Empty)}}}/{{action={nameof(ToDosController.Index)}}}/{{id?}}");

app.Run();
