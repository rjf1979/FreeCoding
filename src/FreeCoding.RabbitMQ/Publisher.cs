using System;
using System.Collections.Generic;
using System.Text;
using RabbitMQ.Client;

namespace Asmkt.Public.RabbitMQ
{
    public class Publisher
    {
        public Publisher(string connectionString)
        {
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

        public void Publish(string queue, string message, IDictionary<string, object> arguments = null,bool isPersistent = true)
        {
            Model.QueueDeclare(queue, true, false, false, arguments);
            var body = Encoding.UTF8.GetBytes(message);
            var basicProperties = Model.CreateBasicProperties();
            basicProperties.DeliveryMode = 1;
            if (isPersistent) basicProperties.DeliveryMode = 2;
            Model.BasicPublish(string.Empty, queue, basicProperties, body);
        }
    }
}
