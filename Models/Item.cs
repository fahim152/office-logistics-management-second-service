namespace office_logistics_managament_cqrs_second_service.Models;
using MongoDB.Bson.Serialization.Attributes;
public class Item 
{
    [BsonId]
    public int ItemId { get; set; }
    public string ?Name { get; set; } = null;
    public int ItemTypeId { get; set; }
    public int Quantity { get; set; }
    public bool IsAssignable { get; set; }
   
}
