using System.Diagnostics;

namespace NetCore.Windows.WinFormConfigureAwait
{
    /// <summary>
    /// 总结：webapi中可以显式调用ConfigureAwait(false)，因为不需要上下文切换，不需要主线程，Winform中需要用到ConfigureAwait(false)，因为如果没有用到await就可能会产生死锁
    /// 如果 await 之后的代码实际上不需要在原始上下文中运行，那么使用 ConfigureAwait(false) 可以避免所有这些成本：它不需要不必要的排队，它可以利用所有它可以进行优化，并且可以避免不必要的线程静态访问
    /// 
    /// 原文博客：https://devblogs.microsoft.com/dotnet/configureawait-faq/
    /// </summary>
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

        }

        private async void Form1_LoadAsync(object sender, EventArgs e)
        {
            //1.使用await DoSomethingAsync主线程不会锁定
            var result = await DoSomethingAsync();  //这个的意思就是结果返回后，需要将结果渲染到首页面，那么就是接力棒需要传递给主线程，那么可以不用ConfigureAwait(false)
            /*
             这个案例也很有意思：
            private async Task OnButtonClick()
{
  string response1 = await GetSomeData().ConfigureAwait(false);
  string response2 = await GetSomeMoreData().ConfigureAwait(true);
  _dataTextBox.Text = response1 + response2;
}
          【  GetSomeData()和GetSomeMoreData()之间的代码不需要在UI线程上运行，因此它使用ConfigureAwait(false)。然后GetSomeMoreData()之后的代码需要在UI线程上运行（  _dataTextBox.Text = response1 + response2;
），因此它使用ConfigureAwait(true)。

            一般情况下它不起作用，除非 GetSomeData 同步返回。 ConfigureAwait(true) 不会撤消ConfigureAwait(false) 的效果。在您的场景中最好的做法可能是将调用提取到单独的方法：】
            private async Task OnButtonClick()
            {
              _dataTextBox.Text = await GetDataAsync();
            }

            private async Task<string> GetDataAsync()
            {
              string response1 = await GetSomeData().ConfigureAwait(false);
              string response2 = await GetSomeMoreData().ConfigureAwait(false);
              return response1 + response2;
            }


             */

            this.button1.Text = result; 

            //2.【不用.ConfigureAwait(false)，那么阻塞】 DoSomethingAsync().Result; 如果DoSomethingAsync方法中不使用.ConfigureAwait(false)那么这里会阻塞
            //  var result = DoSomethingAsync().Result;
            //3.【用.ConfigureAwait(false)不会阻塞主线程窗体的渲染】
            //  var result = DoSomethingAsync().Result;
            // Debug.WriteLine("123"); //这行代码会等待上面执行的结果
            MessageBox.Show($"Operation result:{result}");
        }

        private async Task<string> DoSomethingAsync()
        {
            //假设这里是一个耗时的异步操作
            await Task.Delay(5000).ConfigureAwait(false); //ConfigureAwait(false)代表是不是当前线程执行完后，不需要让主线程来完成我后续的操作，就是告诉主线程你不用管我的返回值什么的，我自己搞定我自己的事情，你做你的事情,我做完我是不会回去的，默认的ConfigureAwait(ture)是true 会把上下文传递给主线程（就好像跑步的接力棒，我跑完了，接下来你来跑，我把我的进度给你，你帮我传递下去，如果我已经跑完了，下面没有任务需要我的内容，那么你不需要帮我接力）
            return "Done";
        }
    }
}
