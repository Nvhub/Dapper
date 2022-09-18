namespace DapperProject.Application.Interfaces
{
    public interface IRabbitMQRepository
    {
        public void Publisher(string msg, string routingKey, string exchange = "");
    }
}
