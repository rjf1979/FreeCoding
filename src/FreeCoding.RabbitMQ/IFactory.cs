using Microsoft.Extensions.Logging;

namespace Asmkt.Public.RabbitMQ
{
    public interface IFactory
    {
        Publisher GetPublisher(string name);
        Subscriber GetSubscriber(string name, ILoggerFactory loggerFactory = null);
    }
}
