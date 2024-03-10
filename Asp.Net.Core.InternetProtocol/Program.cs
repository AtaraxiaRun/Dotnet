namespace Asp.Net.Core.InternetProtocol
{
    /// <summary>
    /// 网络通信协议
    /// </summary>
    internal class Program
    {
        static async Task Main(string[] args)
        {
            TcpServerInternetProtocol.StartServer(); //不await 等待
            Thread.Sleep(2000);
            TcpClientInternetProtocol.StartClient(); //发送消息
            Console.ReadLine();
        }
    }
}
