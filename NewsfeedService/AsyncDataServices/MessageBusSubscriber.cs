using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using NewsFeedService.EventProcessing;

namespace NewsFeedService.AsyncDataServices
{
    public class MessageBusSubscriber : BackgroundService
    {
        private readonly IConfiguration _configuration;
        private readonly IEventProcessor _eventProcessor;
        private IConnection _connection;
        private IModel _Tweetchannel;
        private IModel _Userchannel;
        private string _TweetqueueName, _UserqueueName;

        public MessageBusSubscriber(IConfiguration configuration, IEventProcessor eventProcessor)
        {
            _configuration = configuration;
            _eventProcessor = eventProcessor;
            InitializeRabbitMQ();
        }

        private void InitializeRabbitMQ()
        {
            var factory = new ConnectionFactory()
            {
                HostName = _configuration["RabbitMQHost"],
                Port = int.Parse(_configuration["RabbitMQPort"])
            };
            _connection = factory.CreateConnection();
            _Tweetchannel = _connection.CreateModel();
            _Userchannel = _connection.CreateModel();
            _Userchannel.ExchangeDeclare(exchange: "user_exchange", type: ExchangeType.Fanout);
            _Userchannel.ExchangeDeclare(exchange: "tweet_exchange", type: ExchangeType.Direct);

            _UserqueueName = "user_deletion_queue";
            _TweetqueueName = "tweet_creation_queue";

            _Tweetchannel.QueueDeclare(queue: _TweetqueueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
            _Userchannel.QueueDeclare(queue: _UserqueueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

            // Bind the queues to the exchange with their respective routing keys
            _Userchannel.QueueBind(queue: _UserqueueName, exchange: "user_exchange", routingKey: "user_deletion");
            _Tweetchannel.QueueBind(queue: _TweetqueueName, exchange: "tweet_exchange", routingKey: "tweet_creation");

            Console.WriteLine("--> Listening on the message bus...");
            _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer1 = new EventingBasicConsumer(_Tweetchannel);
            consumer1.Received += (ModuleHandle, ea) =>
            {
                Console.WriteLine("--> Event Received on Channel 1!");
                var body = ea.Body;
                var notificationMessage = Encoding.UTF8.GetString(body.ToArray());
                _eventProcessor.ProcessEvent(notificationMessage);
            };
            _Tweetchannel.BasicConsume(queue: _TweetqueueName, autoAck: true, consumer: consumer1);

            var consumer2 = new EventingBasicConsumer(_Userchannel);
            consumer2.Received += (ModuleHandle, ea) =>
            {
                Console.WriteLine("--> Event Received on Channel 2!");
                var body = ea.Body;
                var notificationMessage = Encoding.UTF8.GetString(body.ToArray());
                _eventProcessor.ProcessEvent(notificationMessage);
            };
            _Userchannel.BasicConsume(queue: _UserqueueName, autoAck: true, consumer: consumer2);

            return Task.CompletedTask;
        }
        private void RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs e)
        {
            Console.WriteLine("--> Connection shutdown");
        }
        public override void Dispose()
        {
            if (_Tweetchannel.IsOpen || _Userchannel.IsOpen)
            {
                _Tweetchannel.Close();
                _Userchannel.Close();
                _connection.Close();
            }
            base.Dispose();
        }
    }
}