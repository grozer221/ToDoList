using ToDoList;
using ToDoList.Controllers;
using ToDoList.GraphQL;
using ToDoList.MsSql.Extensions;
using ToDoList.MySql.Extensions;
using ToDoList.XML.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();

builder.Services.AddAutoMapper(typeof(AppMappingProfile));

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddMsSqlDataProvider(builder.Configuration.GetConnectionString("MsSql"));
}
else
{
    builder.Services.AddMySqlDataProvider(Environment.GetEnvironmentVariable("JAWSDB_URL"));
}

builder.Services.AddXmlDataProdiver(builder.Configuration.GetConnectionString("Xml"));

builder.Services.AddGraphQLApi();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/error", "?statusCode={0}");
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseGraphQL<AppSchema>();
app.UseGraphQLAltair();

app.MapControllerRoute(
    name: "default",
    pattern: $"{{controller={nameof(ToDosController).ReplaceInEnd("Controller", string.Empty)}}}/{{action={nameof(ToDosController.Index)}}}/{{id?}}");

app.Run();
