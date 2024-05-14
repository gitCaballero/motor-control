using MotorControl.Api.Models;
using RabbitMQ.Client;
using RentalMotor.Api.Services.Network.MessageSender;
using System.Text;
using System.Text.Json;

namespace RentalMotor.Api.Services.Network
{
    public class RabbitMQMessageSender : IRabbitMQMessageSender
    {
        private readonly string _hostName = "localhost";
        private readonly string _password = "guest";
        private readonly string _userName = "guest";
        private IConnection _connection;
        private readonly IModel _channel;


        public RabbitMQMessageSender()
        {
            var factory = new ConnectionFactory
            {
                HostName = _hostName,
                Password = _password,
                UserName = _userName,
            };
            _connection = factory.CreateConnection();

            _channel = _connection.CreateModel();

        }

        public void SendMessage(IEnumerable<BaseMessage> messages, string queueName)
        {
            _channel.QueueDeclare(queue: queueName, false, false, false, arguments: null);

            byte[] body = GetMessageAssByteArray(messages);

            _channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: null, body: body);
        }

        private static byte[] GetMessageAssByteArray(IEnumerable<BaseMessage> messages)
        {
            var json = string.Empty;
            var typeObject = messages.FirstOrDefault()!.GetType().Name;
            var options = new JsonSerializerOptions { WriteIndented = true };
            switch (typeObject)
            {
                case "MotorModelResponse":
                    json = JsonSerializer.Serialize<IEnumerable<MotorModelResponse>>((IEnumerable<MotorModelResponse>)messages, options);
                    break;

                default:
                    break;

            }
            var body = Encoding.UTF8.GetBytes(json);
            return body;
        }
    }
}