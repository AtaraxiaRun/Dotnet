using System.Reflection;

namespace AspNetCore.Api.InjectionAttributes
{
    /// <summary>
    /// ServiceCollection扩展方法
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// 自动注入程序服务
        /// </summary>
        /// <param name="services"></param>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public static IServiceCollection AddInJectedServices(this IServiceCollection services, Assembly assembly)
        {
            // 获取程序集中所有类的类型，这些类被标记了InjectionAttribute，说明就需要注入
            var typesWithInjection = assembly.GetTypes()
                .Where(t => t.GetCustomAttributes<InjectionAttribute>(true).Any())
                .Select(t => new
                {
                    ServiceType = t, //实际注入的服务类
                    Attribute = t.GetCustomAttribute<InjectionAttribute>(true) //服务特性绑定的注入接口信息
                });

            foreach (var item in typesWithInjection)
            {
                var serviceType = item.Attribute?.ServiceType ?? item.ServiceType; //获取注入的服务，如果没有对应的接口，那么直接注入他自己
                var implementationType = item.ServiceType; //注入服务
                var lifetime = item.Attribute?.Lifetime; //生命周期类型

                //根据InjectionAttribute中的Lifetime，将服务以适当的方式注册到DI容器中
                switch (lifetime)
                {
                    case ServiceLifetime.Singleton:
                        services.AddSingleton(serviceType, implementationType);
                        break;
                    case ServiceLifetime.Scoped:
                        services.AddScoped(serviceType, implementationType);
                        break;
                    case ServiceLifetime.Transient:
                        services.AddTransient(serviceType, implementationType);
                        break;
                }

            }

            return services;

        }
    }
}
