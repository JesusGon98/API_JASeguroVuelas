using API_JASeguroVuelas.Models;
using API_JASeguroVuelas.Services;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// Configurar MongoDB Settings
builder.Services.Configure<MongoDBSettings>(
    builder.Configuration.GetSection("MongoDB"));

// Registrar MongoDB Client como Singleton
builder.Services.AddSingleton<IMongoClient>(serviceProvider =>
{
    var settings = builder.Configuration.GetSection("MongoDB").Get<MongoDBSettings>();
    return new MongoClient(settings?.ConnectionString);
});

// Registrar MongoDB Database
builder.Services.AddScoped<IMongoDatabase>(serviceProvider =>
{
    var client = serviceProvider.GetRequiredService<IMongoClient>();
    var settings = builder.Configuration.GetSection("MongoDB").Get<MongoDBSettings>();
    return client.GetDatabase(settings?.DatabaseName);
});

// Registrar servicios
builder.Services.AddScoped<ContactoService>();

// Add services to the container.
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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
