using MongoDB.Driver;
using office_logistics_managament_cqrs_second_service.Models;

namespace office_logistics_managament_cqrs_second_service.Services
{
    public class ItemService
    {
        private readonly IMongoCollection<Item> _itemCollection;

        public ItemService(IMongoDatabase database)
        {
            _itemCollection = database.GetCollection<Item>("Items");
        }

        public IEnumerable<Item> GetAllItems()
        {
            return _itemCollection.Find(_ => true).ToList();
        }

        public void InsertItem(Item item)
        {
            _itemCollection.InsertOne(item);
        }
    }
}
