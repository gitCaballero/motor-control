using Microsoft.Extensions.DependencyInjection;
using MotorControl.Api.Services;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RentalMotor.Api.Services.Network.MessageSender;

namespace RentalMotor.Api.Services.Network.MessageConsumer
{
    public class RabbitMQMessageConsumer : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IConnection _connection;
        private readonly IModel _channel;
        public RabbitMQMessageConsumer(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
            var factory = new ConnectionFactory
            {
                HostName = "localhost",
                Password = "guest",
                UserName = "guest",
            };

            _connection = factory.CreateConnection();

            _channel = _connection.CreateModel();

        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            _channel.QueueDeclare(queue: "requestmotorsavailablesqueue", false, false, false, arguments: null);
          
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (_chanel, evt) =>
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var _motorService = scope.ServiceProvider.GetRequiredService<IMotorService>();
                    var _rabbitMQMessageSender = scope.ServiceProvider.GetRequiredService<IRabbitMQMessageSender>();
                    var motorsAvaliables = _motorService.GetMotorsByAvailablesIdAndPlate(available: true);
                    _rabbitMQMessageSender.SendMessage(motorsAvaliables, "responsemotorsavailablesqueue", motorsAvaliables.FirstOrDefault().GetType().Name);

                }

                _channel.BasicAck(evt.DeliveryTag, false);
            };
            _channel.BasicConsume("requestmotorsavailablesqueue", false, consumer);

            _channel.QueueDeclare(queue: "rentalmotorqueue", false, false, false, arguments: null);

            return Task.CompletedTask;
        }
    }
}
