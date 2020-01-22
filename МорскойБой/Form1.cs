using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace МорскойБой
{

    public partial class Form1 : Form
    {
        private const string host = "127.0.0.1";
        private const int port = 8888;
        static TcpClient client;
        static NetworkStream stream;

        public int[,] sea1 = new int[12, 12];
        public int[,] sea2 = new int[10, 10];
        public Graphics y;
        PaintEventArgs zzz;

        bool fla_g;

        public Form1()
        {
            InitializeComponent();
            Thread myThread = new Thread(new ThreadStart(TryConnect)); 
        }

        public void ship(object sender, PaintEventArgs e)
        {
            zzz = e;
            y = this.CreateGraphics();
            Color col = Color.FromArgb(130, 140, 230);
            Brush br = new SolidBrush(col);
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (sea1[i + 1, j + 1] == 1)
                    {
                        // y.FillRectangle(new SolidBrush(Color.FromArgb(0, 0, 200)), new Rectangle(i + 316, j + 1, 28, 28));
                        y.FillRectangle(new SolidBrush(Color.FromArgb(0, 0, 200)), new Rectangle(i * 30 + 1, j * 30 + 1, 28, 28));
                    }
                    if (sea1[i + 1, j + 1] == 0)
                    {
                        y.FillRectangle(br, new Rectangle(i * 30 + 1, j * 30 + 1, 28, 28));
                        //y.FillRectangle(br, new Rectangle(i + 316, j + 1, 28, 28));
                    }
                    if (sea2[i, j] == 0)
                    {
                        y.FillRectangle(br, new Rectangle(i * 30 + 316, j * 30 + 1, 28, 28));
                    }
                }
            }
        }


        //Connect server
        static void TryConnect()
        {
            client = new TcpClient();
            try
            {
                client.Connect(host, port); //подключение клиента
                stream = client.GetStream(); //получаем поток

                // запускаем новый поток для получения данных
                Thread receiveThread = new Thread(new ThreadStart(RecieveData));
                receiveThread.Start();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                //Disconnect();
            }
        }
        static void RecieveData()//в этом методе мы принимаем данные от сервера
        {
            while (true)
            {
                try
                {
                    //Например получаем текстовое сообщение от сервера


                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

        }
        static void Disconnect()
        {
            if (stream != null)
                stream.Close();//отключение потока
            if (client != null)
                client.Close();//отключение клиента
            Environment.Exit(0); //завершение процесса
        }
        //
       

        private void StartClick(object sender, EventArgs e)
        {
            TryConnect();
          
            /*
            if (checkBox1.Checked)
            {
                try
                {
                    TryConnect();
                   
                    fla_g = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }
            }
            /*
            else
            {
                try
                {

                    listenSocket.Connect(ipPoint);
                    fla_g = true;
                    byte[] data = new byte[16];
                    int bytes;
                    bytes = listenSocket.Receive(data, data.Length, 0);
                    MessageBox.Show(Encoding.Unicode.GetString(data, 0, bytes));
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }

            }
            */
            Random qaz = new Random();
            int str, stl, napr, r, b;
            for (int i = 0; i < 12; i++)
            {
                for (int j = 0; j < 12; j++)
                {
                    sea1[i, j] = 0;
                }
            }
            for (int i = 4; i > 0; i--)
            {
                for (int j = 5 - i; j > 0; j--)
                {
                    str = qaz.Next(1, 11);
                    stl = qaz.Next(1, 11);
                    napr = qaz.Next(0, 2);
                    if (napr == 0)
                    {
                        if (stl + i > 11)
                        {
                            j++;
                            continue;
                        }
                        else
                        {
                            r = stl + i;
                            b = str + 1;
                        }
                    }
                    else
                    {
                        if (str + i > 11)
                        {
                            j++;
                            continue;
                        }
                        else
                        {
                            r = stl + 1;
                            b = str + i;
                        }
                    }
                    for (int i1 = str - 1; i1 <= b; i1++)
                    {
                        for (int j1 = stl - 1; j1 <= r; j1++)
                            if (sea1[i1, j1] != 0)
                            {
                                napr = -1;
                                i1 = 12;
                                break;
                            }
                    }
                    if (napr > -1)
                    {
                        for (int i1 = str; i1 < b; i1++)
                        {
                            for (int j1 = stl; j1 < r; j1++)
                                sea1[i1, j1] = 1;
                        }
                    }
                    else j++;

                }
            }
            ship(this, zzz);
        }

        private void Mup(object sender, MouseEventArgs e)
        {
            if (fla_g)
            {
                if (e.X >= 315 && e.X <= 615 && e.Y >= 0 && e.Y <= 300)
                {
                    int str, stl;
                    str = e.Y / 30;
                    stl = (e.X-315) / 30;
                    byte[] data = Encoding.Unicode.GetBytes(str.ToString() + " " + stl.ToString());
                    //handler.Send(data);
                }
            }
        }
    }
}
