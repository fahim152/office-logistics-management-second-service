using MongoDB.Driver;
using office_logistics_managament_cqrs_second_service.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddSingleton<ItemService>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// start db setting
string mongoConnectionString = "mongodb://localhost:27017";
var mongoDatabaseName = "Items";

// Register ItemDbContext with the MongoDB settings
builder.Services.AddSingleton<IMongoClient>(new MongoClient(mongoConnectionString));

builder.Services.AddSingleton<IMongoDatabase>(provider =>
{
    var client = provider.GetRequiredService<IMongoClient>();
    return client.GetDatabase(mongoDatabaseName);
});
// end db setting
var app = builder.Build();

var rabbitMqConsumer = new RabbitMqConsumer("localhost", "item_created");
rabbitMqConsumer.StartConsuming();
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
