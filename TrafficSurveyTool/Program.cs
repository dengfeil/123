using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TrafficSurveyTool
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Form1 Login = new Form1();
            //Login.ShowDialog();//显示登陆窗体
            //if (Login.DialogResult == DialogResult.OK)
                Application.Run(new MainWindow());//判断登陆成功时主进程显示主窗口
            //else return;

            //Application.Run(new Form1());
        }
    }
}
