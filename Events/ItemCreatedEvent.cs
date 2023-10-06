namespace office_logistics_managament_cqrs_second_service.Events;

public class ItemCreatedEvent
{
    public int ItemId { get; set; }
    public string ?Name { get; set; }
    public int Quantity { get; set; }
    public int ItemTypeId { get; set; }
    public bool IsAssignable { get; set; }

}
