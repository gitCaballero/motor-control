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

        public void SendMessage(IEnumerable<BaseMessage> messages, string queueName, string typeObject)
        {
           
            _channel.QueueDeclare(queue: queueName, false, false, false, arguments: null);

            foreach (var message in messages)
            {
                byte[] body = GetMessageAssByteArray(message, typeObject);

                _channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: null, body: body);
            }
        }

        private static byte[] GetMessageAssByteArray(BaseMessage message, string typeObject)
        {
            var json = string.Empty;

            var options = new JsonSerializerOptions { WriteIndented = true };
            switch (typeObject)
            {
                case "MotorModelResponse":
                    json = JsonSerializer.Serialize<MotorModelResponse>((MotorModelResponse)message, options);
                    break;

                default:
                    break;
                    
            }
            var body = Encoding.UTF8.GetBytes(json);
            return body;
        }
    }
}