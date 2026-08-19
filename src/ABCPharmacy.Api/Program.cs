using Microsoft.OpenApi.Models;
using System.Text.Json;


using System.Text.Json;
using ABCPharmacy.Api.Middleware;
using ABCPharmacy.Api.Repositories;
using ABCPharmacy.Api.Repositories.Interfaces;
using ABCPharmacy.Api.Services;
using ABCPharmacy.Api.Services.Interfaces;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "ABCPharmacy API", Version = "v1" });
});

// config data file paths
var dataFiles = builder.Configuration.GetSection("DataFiles");
var medicinesFile = dataFiles.GetValue<string>("Medicines") ?? "Data/medicines.json";
var salesFile = dataFiles.GetValue<string>("Sales") ?? "Data/sales.json";

// repositories using JSON files (store full path)
builder.Services.AddSingleton<IMedicineRepository>(sp =>
    new MedicineRepository(Path.Combine(builder.Environment.ContentRootPath, medicinesFile)));
builder.Services.AddSingleton<ISalesRepository>(sp =>
    new SalesRepository(Path.Combine(builder.Environment.ContentRootPath, salesFile)));

builder.Services.AddScoped<IMedicineService, MedicineService>();
builder.Services.AddScoped<ISalesService, SalesService>();

// CORS: allow UI local port (change if you pick different port)
builder.Services.AddCors(options =>
{
    options.AddPolicy("UiPolicy", p =>
    {
        p.WithOrigins("http://localhost:5002", "https://localhost:7202")
         .AllowAnyHeader()
         .AllowAnyMethod();
    });
});

var app = builder.Build();

// ensure data folder & files exist
void Ensure(string path, string initial)
{
    var dir = Path.GetDirectoryName(path);
    if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
    if (!File.Exists(path)) File.WriteAllText(path, initial);
}

Ensure(Path.Combine(app.Environment.ContentRootPath, medicinesFile),
    JsonSerializer.Serialize(new List<object>(), new JsonSerializerOptions { WriteIndented = true }));
Ensure(Path.Combine(app.Environment.ContentRootPath, salesFile),
    JsonSerializer.Serialize(new List<object>(), new JsonSerializerOptions { WriteIndented = true }));

// global exception handling middleware
app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors("UiPolicy");
app.UseAuthorization();
app.MapControllers();
app.Run();