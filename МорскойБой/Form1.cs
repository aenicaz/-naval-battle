using System;
using System.Drawing;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace МорскойБой
{

    public partial class Form1 : Form
    {
       
        ServerObject server;
        static Board boardOwn;
        static Board boardEnemy;
        public static Busy  Busy;

        

        public Form1()
        {
            InitializeComponent(); 
        }

       private void StartClick(object sender, EventArgs e)
        {
            server.TryConnect();

            if (checkBox1.Checked)
                Busy = Busy.on;
            else
                Busy = Busy.off;
            //Создаем корабли на доске
            boardOwn.CreateShips(boardOwn);
            //Отправляем наши позции
            server.SendMessage(boardOwn.GetPositionShips());
            
        }

       private void Form1_Load(object sender, EventArgs e)
       {
            boardOwn = new Board(5, 5, 10, this, Team.Own);
            server = ServerObject.getInstance(); //создается объект сервера
            boardOwn.CreateBoard(); //создаётся наша доска
        }

    }
}
