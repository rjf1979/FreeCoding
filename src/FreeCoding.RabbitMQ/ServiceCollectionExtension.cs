using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Asmkt.Public.RabbitMQ
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddRabbitMQFactory(this IServiceCollection services,IConfigurationSection configurationSection)
        {
            return services.Configure<MQSetting>(configurationSection).AddSingleton<IFactory, Factory>();
        }
    }
}