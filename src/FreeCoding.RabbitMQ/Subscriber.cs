using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Asmkt.Public.RabbitMQ
{
    public class Subscriber
    {
        private readonly ILogger _logger;

        public Subscriber(string connectionString, ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory?.CreateLogger<Subscriber>();
            ConnectionFactory = new ConnectionFactory
            {
                Uri = new Uri(connectionString),
                AutomaticRecoveryEnabled = true
            };
            Connection = ConnectionFactory.CreateConnection();
            Model = Connection.CreateModel();
        }
        public IConnectionFactory ConnectionFactory { get; }
        public IConnection Connection { get; }
        public IModel Model { get; }

        /// <summary>
        /// 订阅事件
        /// </summary>
        /// <param name="callBackEvent"></param>
        /// <param name="queue"></param>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public void Subscribe(string queue, Action<string> callBackEvent, IDictionary<string, object> arguments = null)
        {
            Model.QueueDeclare(queue, true, false, false, arguments);
            var consumer = new EventingBasicConsumer(Model);
            //接收事件
            consumer.Received += (sender, ea) =>
            {
                var body = ea.Body;
                var value = Encoding.UTF8.GetString(body.ToArray());

                try
                {
                    if (string.IsNullOrEmpty(value)) return;
                    callBackEvent.Invoke(value);
                }
                catch (Exception exception)
                {
                    var message = $"queue:{queue} > callBackEvent.Invoke is exception，Data:{value}";
                    _logger?.LogError(exception, message);
                }
            };
            Model.BasicConsume(queue: queue, autoAck: true, consumer: consumer);
        }

        public static Subscriber Connect(string connectionString, ILoggerFactory loggerFactory)
        {
            return new Subscriber(connectionString, loggerFactory);
        }
    }
}
