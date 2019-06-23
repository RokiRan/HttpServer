using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HTTPServerLib;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Threading;

namespace HttpServer
{
    class cmdObj
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

        
        private String cmd = "no", money = "no";
        private IntPtr target;
        public cmdObj()
        {

        }
        public void setTarget(IntPtr t) {
            target = t;
        }
        public cmdObj(String c, String m)
        {
            this.cmd = c;
            this.money = m;
        }
        public void setCmd(String cmd)
        {
            this.cmd = cmd;
        }
        public void set(String k, String v)
        {
            if (k == "cmd")
            {
                this.cmd = v;
            }
            else if (k == "money")
            {
                this.money = v;
            }
        }
        public void setMoney(String money)
        {
            this.money = money;
        }
        public String getCmd()
        {
            return this.cmd;
        }
        public String getMoney()
        {
            return this.money;
        }
        public string toString()
        {
            return this.cmd + ";" + this.money;
        }
        public void run()
        {
            //IntPtr target = FindWindow(null, "体育平台 - Google Chrome");
            if (this.cmd == "select")
            {
                if (target != IntPtr.Zero)
                {
                    //通过句柄设置当前窗体最大化（0：隐藏窗体，1：默认窗体，2：最小化窗体，3：最大化窗体，....）
                    SwitchToThisWindow(target, true);
                    Thread.Sleep(200);
                    for (int i = 0; i < this.money.Length; i++)
                    {
                        Thread.Sleep(30);
                        //KeyBoard.sendKey(msg[i]);
                        PostMessage(target.ToInt32(), 256, this.money[i], 0);
                    }
                    //PostMessage(target.ToInt32(), WM_KEYDOWN, 46, 0);
                }
                else
                {
                    Console.WriteLine("没有找到沙巴窗口,请重新尝试");
                    //target = FindWindow(null, "体育平台 - Google Chrome");
                }
            }
            else if (this.cmd == "bet")
            {
                if (target != IntPtr.Zero)
                {
                    //通过句柄设置当前窗体最大化（0：隐藏窗体，1：默认窗体，2：最小化窗体，3：最大化窗体，....）
                    SwitchToThisWindow(target, true);
                    Thread.Sleep(200);
                    KeyBoard.sendEnter();
                    Thread.Sleep(150);
                    KeyBoard.sendEnter();
                }
                else
                {
                   Console.WriteLine("没有找到沙巴窗口,请重新尝试");
                   // target = FindWindow(null, "体育平台 - Google Chrome");
                }
            }
        }
    }
    [ComVisible(true)]//com+可见
    public class ExampleServer : HTTPServerLib.HttpServer
    {


        public struct POINT
        {
            int x;
            int y;
        }
        [DllImport("user32.dll")]
        static extern IntPtr WindowFromPoint(POINT Point);

        [DllImport("user32.dll")]
        static extern IntPtr WindowFromPoint(int xPoint, int yPoint);

        [DllImport("user32.dll")]
        static extern bool GetCursorPos(out POINT lpPoint);

        [DllImport("user32.dll")]
        static extern bool SetWindowText(IntPtr hWnd, string lpString);

        public static POINT GetCursorPos()
        {
            POINT p;
            if (GetCursorPos(out p))
            {
                return p;
            }
            throw new Exception();
        }
        public void ready(int i)
        {
            if (i != 0)
            {
                Console.WriteLine(i.ToString() + " 秒后捕获目标！");
                Thread.Sleep(1000);
                ready(i - 1);
            }
            else
            {
                targetWin = WindowFromPoint();
                CmdObj.setTarget(targetWin);
                Console.WriteLine("捕获完成，请自行点击机会测试~");
            }
        }


        public static IntPtr WindowFromPoint()
        {
            POINT p = GetCursorPos();
            return WindowFromPoint(p);
        }
        public IntPtr targetWin;
        public int isFind = 0;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="ipAddress">IP地址</param>
        /// <param name="port">端口号</param>
        public ExampleServer(string ipAddress, int port)
            : base(ipAddress, port)
        {
            ready(3);
        }
        private cmdObj CmdObj = new cmdObj();
        
        public override void OnPost(HttpRequest request, HttpResponse response)
        {
            //获取客户端传递的参数
            string data = request.Params == null ? "" : string.Join(";", request.Params.Select(x => x.Key + "=" + x.Value).ToArray());

            string[] sArray = data.Split(';');
            string temp = "";
            //Console.WriteLine(data);
            foreach (string i in sArray)
            {
                temp += i;
                string[] ccc = i.Split('=');
                
                if (ccc.Length == 2)
                {
                    CmdObj.set(ccc[0], ccc[1]);
                }
                
            }



            //request.Params.Select(x => CmdObj.set(x.Key, x.Value));
            //设置返回信息
            //string content = string.Format(CmdObj.ToString());

            //构造响应报文
            Console.WriteLine(CmdObj.toString());
            response.SetContent(CmdObj.toString());
            response.Content_Encoding = "utf-8";
            response.StatusCode = "200";
            response.Content_Type = "text/html; charset=UTF-8";
            response.Headers["Server"] = "ExampleServer";
            response.Headers["Access-Control-Allow-Origin"] = "*";
            //发送响应
            response.Send();
            CmdObj.run();
        }

        public override void OnGet(HttpRequest request, HttpResponse response)
        {

            ///链接形式1:"http://localhost:4050/assets/styles/style.css"表示访问指定文件资源，
            ///此时读取服务器目录下的/assets/styles/style.css文件。

            ///链接形式1:"http://localhost:4050/assets/styles/"表示访问指定页面资源，
            ///此时读取服务器目录下的/assets/styles/style.index文件。

            //当文件不存在时应返回404状态码
            string requestURL = request.URL;
            requestURL = requestURL.Replace("/", @"\").Replace("\\..", "").TrimStart('\\');
            string requestFile = Path.Combine(ServerRoot, requestURL);
            //Console.WriteLine(request.URL);
            //判断地址中是否存在扩展名
            string extension = Path.GetExtension(requestFile);
            string[] sArray = request.URL.Split('/');
            if (sArray.Length == 3)
            {
                CmdObj.setCmd(sArray[1]);
                CmdObj.setMoney(sArray[2]);
                CmdObj.run();
            }
            else
            {
                Console.WriteLine("参数错误!");
            }
            //根据有无扩展名按照两种不同链接进行处
            if (extension != "")
            {
                //从文件中返回HTTP响应
                //response = response.FromFile(requestFile);
                response.SetContent("hello1");
            } 
            else
            {
                //目录存在且不存在index页面时时列举目录
                if (Directory.Exists(requestFile) && !File.Exists(requestFile + "\\index.html"))
                {
                    requestFile = Path.Combine(ServerRoot, requestFile);
                    var content = ListDirectory(requestFile, requestURL);
                    response = response.SetContent(content, Encoding.UTF8);
                    response.Content_Type = "text/html; charset=UTF-8";
                } 
                else
                {
                    //加载静态HTML页面
                    requestFile = Path.Combine(requestFile, "index.html");
                    response = response.FromFile(requestFile);
                    response.Content_Type = "text/html; charset=UTF-8";
                }
                
            }
            response.Content_Encoding = "utf-8";
            response.StatusCode = "200";
            response.Content_Type = "text/html; charset=UTF-8";
            response.Headers["Server"] = "ExampleServer";
            response.Headers["Access-Control-Allow-Origin"] = "*";
            response.SetContent(request.URL);
            //发送HTTP响应
            response.Send();
        }

        public override void OnDefault(HttpRequest request, HttpResponse response)
        {

        }

        private string ConvertPath(string[] urls)
        {
            string html = string.Empty;
            int length = ServerRoot.Length;
            foreach (var url in urls)
            {
                var s = url.StartsWith("..") ? url : url.Substring(length).TrimEnd('\\');
                html += String.Format("<li><a href=\"{0}\">{0}</a></li>", s);
            }

            return html;
        }

        private string ListDirectory(string requestDirectory, string requestURL)
        {
            //列举子目录
            var folders = requestURL.Length > 1 ? new string[] { "../" } : new string[] { };
            folders = folders.Concat(Directory.GetDirectories(requestDirectory)).ToArray();
            var foldersList = ConvertPath(folders);

            //列举文件
            var files = Directory.GetFiles(requestDirectory);
            var filesList = ConvertPath(files);

            //构造HTML
            StringBuilder builder = new StringBuilder();
            builder.Append(string.Format("<html><head><title>{0}</title></head>", requestDirectory));
            builder.Append(string.Format("<body><h1>{0}</h1><br/><ul>{1}{2}</ul></body></html>",
                 requestURL, filesList, foldersList));

            return builder.ToString();
        }
    }
}
