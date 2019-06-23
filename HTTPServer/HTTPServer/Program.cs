using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HTTPServerLib;
using HttpServer;

namespace HTTPServerLib
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("插件启动");
            Console.WriteLine("打开本软件之前请确认沙巴窗口是否已经打开！");
            Console.WriteLine("打开后鼠标点击一次目标窗口，如果点错了，请重新打开！");
            Console.WriteLine("软件只会记录第一次鼠标点击的目标！");
            ExampleServer server = new ExampleServer("0.0.0.0", 4050);
            //server.SetRoot(@"D:\Hexo\public");
            //server.Logger = new ConsoleLogger();
            server.Start();
            
        }
    }
}
