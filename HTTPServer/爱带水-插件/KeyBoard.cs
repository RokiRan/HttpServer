﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Windows.Forms;
namespace 爱带水_插件
{
    class KeyBoard
    {
        [DllImport("user32.dll")]
        static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, int dwExtraInfo);
        [DllImport("user32.dll")]
        public static extern Keys VkKeyScan(char ch);

        public static void sendKey(char key)
        {
            keybd_event((byte)VkKeyScan(key), 0, 0, 0);
            keybd_event((byte)VkKeyScan(key), 0, 2, 0);
        }
        public static void sendEnter()
        {
            keybd_event(13, 0, 0, 0);
            keybd_event(13, 0, 2, 0);
        }
    }
}
