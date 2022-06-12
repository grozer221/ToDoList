using ToDoList;
using ToDoList.GraphQL;
using ToDoList.MsSql.Extensions;
using ToDoList.XML.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("DefaultPolicy", builder =>
    {
        builder.AllowAnyHeader()
               .WithMethods("POST")
               .WithOrigins("http://localhost:3000")
               .AllowCredentials();
    });
});

builder.Services.AddAutoMapper(typeof(AppMappingProfile));

builder.Services.AddMsSqlDataProvider(builder.Configuration.GetConnectionString("MsSql"));
builder.Services.AddXmlDataProdiver(builder.Configuration.GetConnectionString("Xml"));

builder.Services.AddGraphQLApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseCors("DefaultPolicy");
}
else
{
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseGraphQL<AppSchema>();
app.UseGraphQLAltair();

app.UseSpa(spa =>
{
    spa.Options.SourcePath = "wwwroot";
});

app.Run();
