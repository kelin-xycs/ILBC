using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

using Xilium.CefGlue.WindowsForms;

namespace ILBCStudio
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private CefWebBrowser browser;

        private void Form1_Load(object sender, EventArgs e)
        {
            browser = new CefWebBrowser();

            browser.StartUrl = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase), "Main.html");
            browser.Dock = DockStyle.Fill;

            browser.BackColor = Color.White;

            browser.ProcessMessageReceived += Browser_ProcessMessageReceived;
            
            this.Controls.Add(browser);
        }

        private void Browser_ProcessMessageReceived(Xilium.CefGlue.CefBrowser browser, Xilium.CefGlue.CefProcessId sourceProcess, Xilium.CefGlue.CefProcessMessage message)
        {
            if (message.Name == "OpenSaveFileDialog")
            {
                BeginInvoke(new Action(() => { saveFileDialog1.ShowDialog(); }));
            }
            else
            {
                MessageBox.Show("未知的消息 from Render Process ，消息名字“" + message.Name + "”。");
            }
        }

        private void saveFileDialog1_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var file = saveFileDialog1.FileName;

            file = file.Replace(@"\", @"\\");

            browser.Browser.GetMainFrame().ExecuteJavaScript("SaveFile('" + file + "');", null, 0);

            //  这里要把 FileName 重置一下，不然下次打开 对话框 时显示的是 上次带路径的文件名
            saveFileDialog1.FileName = "新建文本文档.txt";
        }
    }
}
