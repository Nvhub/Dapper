using DapperProject.Application.Interfaces;
using RabbitMQ.Client;
using System.Text;

namespace DapperProject.Infrastructure.Repositories
{
    public class RabbitMQRepository : IRabbitMQRepository
    {
        public void Publisher(string msg, string routingKey, string exchange= "")
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };

            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare(queue: routingKey, durable: false, exclusive: false, autoDelete: false, arguments: null);
            var msgByte = Encoding.UTF8.GetBytes(msg);

            channel.BasicPublish(exchange, routingKey, basicProperties: null, body: msgByte);
        }
    }
}
