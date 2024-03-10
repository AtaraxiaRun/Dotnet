using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Asp.Net.Core.InternetProtocol
{
    /// <summary>
    /// Tcp服务器通信:高可靠性，需要长时间保持连接的应用，如实时通信、在线游戏，链接成功了一次就可以一直长时间用，不用像Http那样每次都得重新连接
    /// </summary>
    public class TcpServerInternetProtocol
    {
        public static async Task StartServer()
        {
            var listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            listener.Bind(new IPEndPoint(IPAddress.Any, 8080)); // 监听再所有网络接口的8080端口
            listener.Listen(10); //设置最大的连接请求队列长度

            Console.WriteLine("等待连接............");
            var handler = await listener.AcceptAsync(); // 接受连接请求
            var buffer = new byte[1024];
            int bytesReceived = handler.Receive(buffer);  //接收数据
            var data = Encoding.ASCII.GetString(buffer, 0, bytesReceived);

            Console.WriteLine($"Received:{data}");

            // 发送响应
            var msg = Encoding.ASCII.GetBytes("Data received.");
            handler.Send(msg);

            handler.Shutdown(SocketShutdown.Both);
            handler.Close();
        }
    }
    /// <summary>
    /// Tcp客户端通信
    /// </summary>
    public class TcpClientInternetProtocol
    {
        public static void StartClient()
        {
            var client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            client.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8080));// 连接到服务器

            var message = Encoding.ASCII.GetBytes("Hello from then client!");
            client.Send(message); //发送数据
            Thread.Sleep(1000);
            var buffer = new byte[1024];
            int bytesReceived = client.Receive(buffer); //接收响应
            Console.WriteLine($"Received:{Encoding.ASCII.GetString(buffer, 0, bytesReceived)}");

            client.Shutdown(SocketShutdown.Both);
            client.Close();
        }
    }
}
