namespace AspNetCore.Api.InjectionAttributes
{
    [Injection<IMyService>]
    public class MyService : IMyService
    {
        public string GetOk()
        {
            return "OK";
        }
    }
}
