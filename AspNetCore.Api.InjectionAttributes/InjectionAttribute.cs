namespace AspNetCore.Api.InjectionAttributes
{
    /// <summary>
    /// 自动注册识别特性(配合程序集扫描注入)
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class InjectionAttribute : Attribute
    {
        public Type? ServiceType { get; }
        public ServiceLifetime Lifetime { get; }
        public InjectionAttribute(ServiceLifetime lifetime = ServiceLifetime.Scoped) : this(null, lifetime)
        {

        }

        public InjectionAttribute(Type? servicetype, ServiceLifetime lifetime = ServiceLifetime.Scoped)
        {
            this.ServiceType = servicetype;
            this.Lifetime = lifetime;
        }
    }

    /// <summary>
    /// 自动注册识别特性
    /// </summary>
    /// <typeparam name="TService"></typeparam>
    public class InjectionAttribute<TService> : InjectionAttribute where TService : class
    {
        public InjectionAttribute(ServiceLifetime lefetime = ServiceLifetime.Scoped) : base(typeof(TService), lefetime)
        {

        }
    }
}
