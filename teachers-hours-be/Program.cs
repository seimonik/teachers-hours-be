using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;
using TH.Services;
using TH.S3Client;
using TH.Dal;
using TH.Dal.Extentions;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;
var assembly = typeof(Program).Assembly;

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(opt => opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

services.AddExcelGeneratorServices();

services.AddDbContext<TeachersHoursDbContext>(opt =>
    opt.UseNpgsql(configuration.GetConnectionString("TeachersHours")));

services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(assembly));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(o =>
{
    o.SwaggerDoc("v1", new OpenApiInfo { Title = "teachers-hours-be", Version = "v1" });
    //o.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{assembly.GetName().Name}.xml"));
});

// Add S3
var s3Config = configuration.GetSection("S3");
services.AddS3(s3Config);

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.Services.ApplyMigrations<TeachersHoursDbContext>();

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.Run();
