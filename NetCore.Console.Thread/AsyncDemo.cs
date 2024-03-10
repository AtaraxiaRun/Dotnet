using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCore.ConsoleThread
{
    /// <summary>
    /// 异步方法
    /// </summary>
    public class AsyncDemo
    {
        // HttpClient 实例最好由依赖注入提供，并且在应用程序生命周期中重用
        private static readonly HttpClient client = new HttpClient();

        // 获取网站内容的异步方法
        public async Task FetchWebsiteAsync(CancellationToken ct)
        {
            try
            {
                // 使用 HttpClient 发起异步 GET 请求
                HttpResponseMessage response = await client.GetAsync("http://www.baidu.com", ct);
                ct.ThrowIfCancellationRequested();

                Console.WriteLine("收到响应。");

                if (response.IsSuccessStatusCode)
                {
                    // 读取响应内容
                    string content = await response.Content.ReadAsStringAsync(ct);
                    Console.WriteLine("成功获取网站内容。");

                    // 这里可以对 content 进行额外的处理
                    // 比如保存到文件、数据库或者其他处理
                }
                else
                {
                    Console.WriteLine($"错误: {response.StatusCode}");
                }
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("操作被取消。");
            }
            catch (HttpRequestException ex)
            {
                // 处理 HTTP 请求异常
                Console.WriteLine("获取网站时出错: " + ex.Message);
            }
            catch (Exception ex)
            {
                // 处理其他异常
                Console.WriteLine("异常: " + ex.Message);
            }
        }

        // 使用 Task.Factory.StartNew 启动异步操作
        public Task FetchWebsiteWithStartNew(CancellationToken ct)
        {
            return Task.Factory.StartNew(async () =>
            {
                await FetchWebsiteAsync(ct);
            }, ct, TaskCreationOptions.None, TaskScheduler.Default).Unwrap();
        }

        // 使用 Task.Run 启动异步操作
        public Task FetchWebsiteWithRun(CancellationToken ct)
        {
            return Task.Run(async () =>
            {
                await FetchWebsiteAsync(ct);
            }, ct);
        }
    }
}
