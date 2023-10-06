using System;
using System.Text;
using MongoDB.Driver;
using Newtonsoft.Json;
using office_logistics_managament_cqrs_second_service.Events;
using office_logistics_managament_cqrs_second_service.Models;
using office_logistics_managament_cqrs_second_service.Services;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

public class RabbitMqConsumer
{
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private readonly string _queueName;

    public RabbitMqConsumer(string hostName, string queueName)
    {
        var factory = new ConnectionFactory
        {
            HostName = hostName,
            Port = 5672,
        };

        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
        _queueName = queueName;

       _channel.QueueDeclare(_queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

        // Bind the queue to the exchange with the specified routing key
        _channel.QueueBind(_queueName, "item_created", "item_created"); // Use the same routing key as in the publisher

    }

    public void StartConsuming()
    {
        var consumer = new EventingBasicConsumer(_channel);

        consumer.Received += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            Console.WriteLine($"Received event: {message}");
            var itemCreatedEvent = JsonConvert.DeserializeObject<ItemCreatedEvent>(message);
           
            var newItem = new Item
            {
                ItemId = itemCreatedEvent.ItemId,
                Name = itemCreatedEvent.Name,
                ItemTypeId = itemCreatedEvent.ItemTypeId,
                Quantity = itemCreatedEvent.Quantity,
                IsAssignable = itemCreatedEvent.IsAssignable,
            };
             
            try
            {
                // Use a factory method to create an instance of ItemService
                var itemService = CreateItemService();
                itemService.InsertItem(newItem);
                Console.WriteLine("Item inserted successfully.");
            }
            catch (Exception ex)
            {
                // Log the error
                Console.WriteLine($"Error inserting item: {ex.Message}");
            }
        };

        _channel.BasicConsume(queue: _queueName, autoAck: true, consumer: consumer);
    }

    public void Dispose()
    {
        _channel.Close();
        _connection.Close();
    }


    private ItemService CreateItemService()
    {
        var database = CreateMongoDbConnection(); 
        return new ItemService(database);
    }

    
    private IMongoDatabase CreateMongoDbConnection()
    {
        var connectionString = "mongodb://localhost:27017";
        var client = new MongoClient(connectionString);
        var database = client.GetDatabase("Items");
        return database;
    }
}
