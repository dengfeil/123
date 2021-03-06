﻿using System;
using System.Collections.Generic;
using System.IO;
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
            try
            {
                ////处理未捕获的异常   
                //Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
                ////处理UI线程异常   
                //Application.ThreadException += Application_ThreadException;
                ////处理非UI线程异常   
                //AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

                //vs自动生成的代码
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new MainWindow());
            }
            catch (Exception ex)
            {

                var strDateInfo = "出现应用程序未处理的异常：" + DateTime.Now + "\r\n";


                var str = string.Format(strDateInfo + "异常类型：{0}\r\n异常消息：{1}\r\n异常信息：{2}\r\n",
                                            ex.GetType().Name, ex.Message, ex.StackTrace);


                WriteLog(str);//日志写入
                MessageBox.Show("发生1错误，请查看程序日志！", "系统错误", MessageBoxButtons.OK, MessageBoxIcon.Error);//进行弹窗提示
                //Environment.Exit(0);
            }

        }


        /// <summary>
        ///错误弹窗
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            string str;
            var strDateInfo = "出现应用程序未处理的异常：" + DateTime.Now + "\r\n";
            var error = e.Exception;
            if (error != null)
            {
                str = string.Format(strDateInfo + "异常类型：{0}\r\n异常消息：{1}\r\n异常信息：{2}\r\n",
                        error.GetType().Name, error.Message, error.StackTrace);
            }
            else
            {
                str = string.Format("应用程序线程错误:{0}", e);
            }


            WriteLog(str);
            MessageBox.Show("发生错误，请查看程序日志！", "系统错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //Environment.Exit(0);
        }


        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var error = e.ExceptionObject as Exception;
            var strDateInfo = "出现应用程序未处理的异常：" + DateTime.Now + "\r\n";
            var str = error != null ? string.Format(strDateInfo + "Application UnhandledException:{0};\n\r堆栈信息:{1}", error.Message, error.StackTrace) : string.Format("Application UnhandledError:{0}", e);


            WriteLog(str);
            MessageBox.Show("发生错误，请查看程序日志！", "系统错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //Environment.Exit(0);
        }
        /// <summary>
        /// 写文件
        /// </summary>
        /// <param name="str"></param>
        static void WriteLog(string str)
        {
            if (!Directory.Exists("ErrLog"))
            {
                Directory.CreateDirectory("ErrLog");
            }


            using (var sw = new StreamWriter(@"ErrLog\ErrLog.txt", true))
            {
                sw.WriteLine(str);
                sw.WriteLine("---------------------------------------------------------");
                sw.Close();
            }
        }

    }
}
