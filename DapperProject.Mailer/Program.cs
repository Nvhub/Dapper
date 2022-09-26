using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;


namespace DapperProject.Mailer
{
    public class Mail
    {
        private readonly RabbitMQ _rabbitMQ;

        public Mail(RabbitMQ rabbitMQ)
        {
            _rabbitMQ = rabbitMQ;
        }

        public static void Main(string[] args)
        {

            Console.WriteLine("Service Mailer Started ");
            var factory = new ConnectionFactory() { HostName = "localhost"};

            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare(queue: "sendMail", durable: false, exclusive: false, autoDelete: false, arguments: null);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += Consumer_Received;
            channel.BasicConsume(queue: "sendMail", autoAck: true, consumer);
            Console.ReadKey();

        }

        private static void Consumer_Received(object? sender, BasicDeliverEventArgs e)
        {
            var mail = Encoding.UTF8.GetString(e.Body.ToArray());
            Console.WriteLine($"Send Mail To {mail}");
        }
    }
}