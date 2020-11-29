using System.Collections.Generic;

namespace Asmkt.Public.RabbitMQ
{
    public class MQSetting
    {
        public IList<RabbitOptions> Options { get; set; }
    }

    public class RabbitOptions
    {
        public string Name { get; set; }
        public string Connection { get; set; }
    }
}
