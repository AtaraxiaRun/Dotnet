using System.Diagnostics;

namespace NetCore.Windows.WinFormConfigureAwait
{
    /// <summary>
    /// �ܽ᣺webapi�п�����ʽ����ConfigureAwait(false)����Ϊ����Ҫ�������л�������Ҫ���̣߳�Winform����Ҫ�õ�ConfigureAwait(false)����Ϊ���û���õ�await�Ϳ��ܻ��������
    /// ��� await ֮��Ĵ���ʵ���ϲ���Ҫ��ԭʼ�����������У���ôʹ�� ConfigureAwait(false) ���Ա���������Щ�ɱ���������Ҫ����Ҫ���Ŷӣ��������������������Խ����Ż������ҿ��Ա��ⲻ��Ҫ���߳̾�̬����
    /// 
    /// ԭ�Ĳ��ͣ�https://devblogs.microsoft.com/dotnet/configureawait-faq/
    /// </summary>
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

        }

        private async void Form1_LoadAsync(object sender, EventArgs e)
        {
            //1.ʹ��await DoSomethingAsync���̲߳�������
            var result = await DoSomethingAsync();  //�������˼���ǽ�����غ���Ҫ�������Ⱦ����ҳ�棬��ô���ǽ�������Ҫ���ݸ����̣߳���ô���Բ���ConfigureAwait(false)
            /*
             �������Ҳ������˼��
            private async Task OnButtonClick()
{
  string response1 = await GetSomeData().ConfigureAwait(false);
  string response2 = await GetSomeMoreData().ConfigureAwait(true);
  _dataTextBox.Text = response1 + response2;
}
          ��  GetSomeData()��GetSomeMoreData()֮��Ĵ��벻��Ҫ��UI�߳������У������ʹ��ConfigureAwait(false)��Ȼ��GetSomeMoreData()֮��Ĵ�����Ҫ��UI�߳������У�  _dataTextBox.Text = response1 + response2;
���������ʹ��ConfigureAwait(true)��

            һ����������������ã����� GetSomeData ͬ�����ء� ConfigureAwait(true) ���᳷��ConfigureAwait(false) ��Ч���������ĳ�������õ����������ǽ�������ȡ�������ķ�������
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

            //2.������.ConfigureAwait(false)����ô������ DoSomethingAsync().Result; ���DoSomethingAsync�����в�ʹ��.ConfigureAwait(false)��ô���������
            //  var result = DoSomethingAsync().Result;
            //3.����.ConfigureAwait(false)�����������̴߳������Ⱦ��
            //  var result = DoSomethingAsync().Result;
            // Debug.WriteLine("123"); //���д����ȴ�����ִ�еĽ��
            MessageBox.Show($"Operation result:{result}");
        }

        private async Task<string> DoSomethingAsync()
        {
            //����������һ����ʱ���첽����
            await Task.Delay(5000).ConfigureAwait(false); //ConfigureAwait(false)�����ǲ��ǵ�ǰ�߳�ִ����󣬲���Ҫ�����߳�������Һ����Ĳ��������Ǹ������߳��㲻�ù��ҵķ���ֵʲô�ģ����Լ��㶨���Լ������飬�����������,���������ǲ����ȥ�ģ�Ĭ�ϵ�ConfigureAwait(ture)��true ��������Ĵ��ݸ����̣߳��ͺ����ܲ��Ľ��������������ˣ������������ܣ��Ұ��ҵĽ��ȸ��㣬����Ҵ�����ȥ��������Ѿ������ˣ�����û��������Ҫ�ҵ����ݣ���ô�㲻��Ҫ���ҽ�����
            return "Done";
        }
    }
}
