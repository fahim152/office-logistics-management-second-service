

using MongoDB.Driver;
using office_logistics_managament_cqrs_second_service.Models;

public class ItemDbContext
{
    private readonly IMongoDatabase _database;

    public ItemDbContext(IMongoDatabase database)
    {
        _database = database;
    }

    public IMongoCollection<Item> Items => _database.GetCollection<Item>("Items");
}