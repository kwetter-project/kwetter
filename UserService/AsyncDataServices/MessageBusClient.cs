using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using UserService.Dtos;

namespace UserService.AsyncDataServices
{
    public class MessageBusClient : IMessageBusClient
    {
        private readonly IConfiguration _configuration;
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public MessageBusClient(IConfiguration configuration)
        {
            _configuration = configuration;
            var factory = new ConnectionFactory() { HostName = _configuration["RabbitMQHost"], Port = int.Parse(_configuration["RabbitMQPort"]) };
            try
            {
                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();
                _channel.ExchangeDeclare("user_exchange", ExchangeType.Direct);


                _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;
                Console.WriteLine("--> connected to message bus");

            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Could not connect to the message bus : {ex.Message}");
            }
        }
        private void SendMessage(string message)
        {
            var body = Encoding.UTF8.GetBytes(message);
            _channel.BasicPublish(exchange: "user_exchange", routingKey: "user_deletion", basicProperties: null, body: body);
            Console.WriteLine($"--> We have sent {message}");
        }
        public void Dispose()
        {
            Console.WriteLine("--> MEssage bus Disposed");
            if (_channel.IsOpen)
            {
                _channel.Close();
                _connection.Close();
            }
        }
        private void RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs e)
        {
            Console.WriteLine("--> RabbitMQ Connection shutdown");


        }

        public void PublishUserDeleted(UserDeletePublishedDto userDeletePublishedDto)
        {
            var message = JsonSerializer.Serialize(userDeletePublishedDto);
            if (_connection.IsOpen)
            {
                Console.WriteLine("--> RabbitMQ connection open, sending message...");
                SendMessage(message);
            }
            else
            {
                Console.WriteLine("--> RabbitMQ connection is closed, not sending");
            }
        }
    }
}