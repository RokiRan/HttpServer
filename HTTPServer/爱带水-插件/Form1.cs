using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace 爱带水_插件
{
    [ComVisible(true)]//com+可见
    public partial class Form1 : Form
    {
        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        /// <summary>
        /// 获取窗体的句柄函数
        /// </summary>
        /// <param name="lpClassName">窗口类名</param>
        /// <param name="lpWindowName">窗口标题名</param>
        /// <returns>返回句柄</returns>
        [DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        /// <summary>
        /// 通过句柄，窗体显示函数
        /// </summary>
        /// <param name="hWnd">窗体句柄</param>
        /// <param name="cmdShow">显示方式</param>
        /// <returns>返工成功与否</returns>
        [DllImport("user32.dll", EntryPoint = "ShowWindowAsync", SetLastError = true)]
        public static extern bool ShowWindowAsync(IntPtr hWnd, int cmdShow);
        /// <summary>
        /// 通过句柄设置方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        [DllImport("user32.dll", EntryPoint = "SwitchToThisWindow", SetLastError = true)]
        public static extern bool SwitchToThisWindow(IntPtr hWnd, bool fAltTab);



        [DllImport("User32.dll", EntryPoint = "PostMessage")]
        private static extern int PostMessage(
        int hWnd,   //   handle   to   destination   window   
        int Msg,   //   message   
        int wParam,   //   first   message   parameter   
        int lParam   //   second   message   parameter   
        );
        private const int WM_KEYDOWN = 0x0100;
        private const int WM_KEYUP = 0x0100;





        public delegate void Entrust(string cmd, string money);
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Entrust callback = new Entrust(doKeboadrdEvent);
            Thread th = new Thread(ThreadChild);
            th.IsBackground = true;
            th.Start();
            label1.Text = "服务运行中";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
        }
        static void ThreadChild()
        {


            MyHttp server = new MyHttp("0.0.0.0", 4050);
            Console.WriteLine("Child Thread Start!");
            server.Start();
        }

        static void doKeboadrdEvent(string cmd,string money) {
            
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            System.Environment.Exit(System.Environment.ExitCode);

            this.Dispose();

            this.Close();
        }
    }
}
