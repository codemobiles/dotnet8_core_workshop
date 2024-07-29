
using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using dotnet8_hero.Data;
using dotnet8_hero.Installers;
using dotnet8_hero.Interfaces;
using dotnet8_hero.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// dotnet add package Microsoft.EntityFrameworkCore.InMemory
// using Microsoft.EntityFrameworkCore;
// builder.Services.AddDbContext<DatabaseContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("ConnectionSQLServer")));

// Add services to the container.
builder.Services.InstallServiceInAssembly(builder.Configuration);

// 1# Add services manually
// builder.Services.AddTransient<IProductService, Productservice>();
// builder.Services.AddTransient<IUploadFileService, UploadFileService>();

// 2# Begin of using Auto Add Services
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(builder =>
{
    builder.RegisterAssemblyTypes(Assembly.GetEntryAssembly())
    .Where(t => t.Name.EndsWith("Service"))
    .AsImplementedInterfaces();
});
// End of using Auto Add Services

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAllOrigins");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseStaticFiles();
app.MapControllers();

app.Run();
