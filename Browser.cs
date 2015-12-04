using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Win32;
using System.Diagnostics;
using System.Windows.Forms;

namespace JiaowuHelper
{
	class Browser
	{
		public static bool openAuthor = false;
		public static string updateUrl = "http://www.loveyu.net/Update/JiaowuHelper.php";
		public static void openUrl(string url) {
			try
			{
				//RegistryKey key = Registry.ClassesRoot.OpenSubKey(@"http\shell\open\command\");
				//string s = key.GetValue("").ToString().Split(' ')[0].Replace("\"","");
				//System.Diagnostics.Process.Start(s, url);
                //从注册表中读取默认浏览器可执行文件路径  
                RegistryKey key = Registry.ClassesRoot.OpenSubKey(@"http\shell\open\command\");
                string s = key.GetValue("").ToString();

                //s就是你的默认浏览器，不过后面带了参数，把它截去，不过需要注意的是：不同的浏览器后面的参数不一样！  
                //"D:\Program Files (x86)\Google\Chrome\Application\chrome.exe" -- "%1"  
                System.Diagnostics.Process.Start(s.Substring(0, s.Length - 8), url);
            }
			catch (Exception ex){
				MessageBox.Show(ex.Message,"打开网址异常",MessageBoxButtons.OK,MessageBoxIcon.Error);
			}
		}
		public static void openAuthorPage() {
			openUrl("http://blog.scluv.tk");
			openAuthor = false;
		}
		public static void openAppPage() {
			openUrl("https://github.com/CCluv/JiaowuHelper/");
		}
	}
}
