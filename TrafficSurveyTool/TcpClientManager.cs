using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace TrafficSurveyTool
{
    public class Server 
    {
        //将远程连接的客户端IP和socket存入字典
        Dictionary<string, Socket> dicSocket = new Dictionary<string, Socket>();
        Thread th;
        //编码方式，false 表示default编码，true表示UTF-8编码
        public bool enConding = false;

        Socket socketWatch;
        Socket socketSend;
        IPAddress ip;
        int port=0;
        private void btnStart()
        {
            try
            {
                //当点击开始监听的时候 在服务器端创建一个负责监IP地址跟端口号的Socket
                socketWatch = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                //获取IP
                ip = IPAddress.Any;
                //创建端口号
                IPEndPoint portName = new IPEndPoint(ip, port);
                //监听
                socketWatch.Bind(portName);
                //ShowMsg("监听成功");
                socketWatch.Listen(10);
                //新建线程，去接收客户端发来的信息
                th = new Thread(Listen);
                th.IsBackground = true;
                th.Start(socketWatch);
            }
            catch
            {
                MessageBox.Show("btnStart_Click Error...");
            }
        }
       
        void Listen(object o)
        {
            //Socket socketWatch = o as Socket;
            //等待客户端连接 创建一个负责通讯的socket
            while (true)
            {
                socketSend = socketWatch.Accept();
                Thread thListen = new Thread(Recive);
                thListen.IsBackground = true;
                thListen.Start(socketSend);
            }
        }

        void Recive(object o)
        {
            Socket socketSend = (Socket)o;
            try
            {
                while (true)
                {
                    //客户端连接成功后，服务器应该接受客户端发来的消息

                    if (socketSend == null)
                    {
                        MessageBox.Show("请选择要发送的客户端");
                        continue;
                    }
                    byte[] buffer = new byte[1024 * 1024 * 2];
                    //实际接受到的有效字节数
                    int r = socketSend.Receive(buffer);
                    //如果客户端关闭，发送的数据就为空，然后就跳出循环
                    if (r == 0)
                    {
                        break;
                    }
                    string strMsg = string.Empty;
                    if (enConding)
                        strMsg = Encoding.UTF8.GetString(buffer, 0, r);
                    else
                    {
                        //strMsg = Encoding.Default.GetString(buffer, 0, r);
                        strMsg = byteToHexStr(buffer, r);
                    }
                    //ShowMsg(socketSend.RemoteEndPoint.ToString() + ":\n" + strMsg);
                }
            }
            catch { }
        }

        private bool btnSendMsg(string msg)
        {
            //获得选中客户端ip对应的通信Socket       
            if (socketSend == null || msg == null)
            {
                //MessageBox.Show("请选择要发送的客户端");
                return false;
            }
            //string strSend = tichboxMsg.Text;
            try
            {
                byte[] newBuffer = HexStrTobyte(msg);
                //将了标识字符的字节数组传递给客户端
                socketSend.Send(newBuffer);
                //tichboxMsg.Text = "";
                return true;
            }
            catch(Exception ex)
            {
                MessageBox.Show("[Error]Send Message Error..." + ex.Message);
                return false;
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            if (socketWatch != null)
            {
                socketWatch.Close();
            }
        }
        /// <summary>
        /// 16进制string类型数据转byte数据
        /// </summary>
        /// <param name="hexString">输入16进制string</param>
        /// <returns>byte数组</returns>
        private static byte[] HexStrTobyte(string hexString)
        {
            hexString = hexString.Replace(" ", "");
            if ((hexString.Length % 2) != 0)
                hexString += " ";
            byte[] returnBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2).Trim(), 16);
            return returnBytes;
        }
        /// <summary>
        /// byte数组转16进制string类型数据
        /// </summary>
        /// <param name="bytes">输入byte数组</param>
        /// <param name="length">数组长度</param>
        /// <returns>16进制string类型数据</returns>
        public static string byteToHexStr(byte[] bytes, int length)
        {
            string returnStr = "";
            if (bytes != null)
            {
                for (int i = 0; i < length; i++)
                {
                    returnStr += bytes[i].ToString("X2") + " ";//ToString("X2") 为C#中的字符串格式控制符
                }
            }
            return returnStr;
        }
    }
}
