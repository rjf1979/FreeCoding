using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Asmkt.Public.RabbitMQ
{
    public class Factory : IFactory
    {
        private readonly IDictionary<string, Publisher> _rabbitPublishers = new Dictionary<string, Publisher>();
        private readonly IDictionary<string, string> _rabbitConnections = new Dictionary<string, string>();

        public Factory(IOptions<MQSetting> options)
        {
            foreach (var option in options.Value.Options)
            {
                _rabbitConnections.Add(option.Name, option.Connection);
                _rabbitPublishers.Add(option.Name, new Publisher(option.Connection));
            }
        }

#if DEBUG
        public Factory(MQSetting rabbitSetting)
        {
            foreach (var option in rabbitSetting.Options)
            {
                _rabbitConnections.Add(option.Name, option.Connection);
                _rabbitPublishers.Add(option.Name, new Publisher(option.Connection));
            }
        }
#endif

        public Publisher GetPublisher(string name)
        {
            lock (this)
            {
                if (_rabbitPublishers.ContainsKey(name))
                {
                    return _rabbitPublishers[name];
                }

                return null;
            }
        }

        public Subscriber GetSubscriber(string name, ILoggerFactory loggerFactory = null)
        {
            if (_rabbitConnections.ContainsKey(name))
            {
                return new Subscriber(_rabbitConnections[name], loggerFactory);
            }

            return null;
        }
    }
}
