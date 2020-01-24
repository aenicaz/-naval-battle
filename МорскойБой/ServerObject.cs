using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace МорскойБой
{
    class ServerObject
    {
        private static ServerObject instance;
        private const string host = "127.0.0.1";
        private const int port = 8888;
        static TcpClient client;
        static NetworkStream stream;
        Board board;


        public static ServerObject getInstance()
        {
            if (instance == null)
                instance = new ServerObject();
            return instance;
        }


        public ServerObject()
        {
            board = new Board(350, 5, 10, Team.Enemy);
            
        }

        void CreateEnemyBoard(string position)
        {
            string[] data = position.Split(' ');
            board.createEnemyBoard(data); 
        }


        public void Shoot(int number)
        {
            
        }
        public void TryConnect()
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
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                //Disconnect();
            }
        }
        public void RecieveData()//в этом методе мы принимаем данные от сервера
        {

            string data = GetMessage();
            Action action = () => CreateEnemyBoard(data);
            action.Invoke();

            //1 получаем позиции кораблей и передаёт в метод создания доски
            while (true)
            {
                try
                {
                   
                    //Например получаем текстовое сообщение от сервера


                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

                System.Threading.Thread.Sleep(100);
            }
        }
        public void Disconnect()
        {
            if (stream != null)
                stream.Close();//отключение потока
            if (client != null)
                client.Close();//отключение клиента
            Environment.Exit(0); //завершение процесса
        }
        public void SendMessage(string text)
        {
                byte[] data = Encoding.Unicode.GetBytes(text);
                stream.Write(data, 0, data.Length);
        }
        private string GetMessage()
        {
            byte[] data = new byte[1024]; // буфер для получаемых данных
            StringBuilder builder = new StringBuilder();
            int bytes = 0;
            do
            {
                bytes = stream.Read(data, 0, data.Length);
                builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
            }
            while (stream.DataAvailable);

            return builder.ToString();
        }
    }
}
