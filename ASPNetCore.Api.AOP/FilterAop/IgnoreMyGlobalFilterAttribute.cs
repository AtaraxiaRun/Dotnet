namespace ASPNetCore.Api.AOP.FilterAop
{
    /// <summary>
    ///排除过滤器特性， 虽然全局注册了，但是我指定的控制器，过滤器可以不作用这个filter
    ///用这个特性进行排除
    /// </summary>
    public class IgnoreMyGlobalFilterAttribute : Attribute
    {
    }
}
