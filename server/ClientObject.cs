using System;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;

namespace ChatServer
{
    public class ClientObject
    {
        protected internal string Id { get; private set; }
        protected internal NetworkStream Stream { get; private set; }
        TcpClient client;
        ServerObject server; // объект сервера
        string boardData; //номера панелей в которых находятся корабли
        bool BoardIsSend;

        public ClientObject(TcpClient tcpClient, ServerObject serverObject)
        {
            Id = Guid.NewGuid().ToString();
            client = tcpClient;
            server = serverObject;
            serverObject.AddConnection(this);
        }

        public void Process()
        {
            try
            {
                Stream = client.GetStream();
                Console.WriteLine("Пользователь подключился");

                boardData = GetMessage();
                Console.WriteLine(boardData);
                

                
                // в бесконечном цикле получаем сообщения от клиента
                while (true)
                {

                    if (!BoardIsSend && server.CountConnections() == 2)
                    {
                        server.BroadcastMessage(boardData, this.Id);
                        BoardIsSend = true;
                    }

                    try
                    {
                        

                    }
                    catch
                    {
                        Console.WriteLine("Пользователь отключился");
                        break;
                    }

                    System.Threading.Thread.Sleep(100);
                }
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                
            }
            finally
            {
                // в случае выхода из цикла закрываем ресурсы
                server.RemoveConnection(this.Id);
                Close();
            }
        }


        public void Command(string command)
        {
            string[] args = command.Split(';');
            switch(args[0])
            {
               case "SendBoard":
                    break;
            }

        }
        
        // чтение входящего сообщения и преобразование в строку
        private string GetMessage()
        {
            byte[] data = new byte[1024]; // буфер для получаемых данных
            StringBuilder builder = new StringBuilder();
            int bytes = 0;
            do
            {
                bytes = Stream.Read(data, 0, data.Length);
                builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
            }
            while (Stream.DataAvailable);

            return builder.ToString();
        }

        public void SendBoard()
        {
            
        }

        // закрытие подключения
        protected internal void Close()
        {
            if (Stream != null)
                Stream.Close();
            if (client != null)
                client.Close();
        }
    }
}
