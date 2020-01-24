using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace МорскойБой
{
    class Board: Form1
    {
        Team team;
        int pos_x;
        int pos_y;
        static Form1 form;
        public Cell[,] cells;
        public List<int> positionShips = new List<int>();
        private Ship[] ships = new Ship[9];

        public Board(int x,int y, int size, Form1 _form, Team _team)
        {
            pos_x = x;
            pos_y = y;
            cells = new Cell[size,size];
            team = _team;
            form = _form;

          
        }
        public Board(int x, int y, int size, Team _team)
        {
            pos_x = x;
            pos_y = y;
            cells = new Cell[size, size];
            team = _team;
        }

        public void CreateBoard()
        {
            for (int i = 0; i < 10; i++)
                for (int k = 0; k < 10; k++)
                {
                    Cell cell = new Cell();
                    cell.Location = new Point(pos_x + i * 30, pos_y + k * 30);
                    cell.TeamCell = team;
                    cells[i, k] = cell;
                    form.Controls.Add(cell);
                }
        }
        public void CreateShips(Board boardOwn)
        {
            ships[0] = new Ship(2, boardOwn);
            ships[0] = new Ship(2, boardOwn);
            ships[0] = new Ship(2, boardOwn);
            ships[0] = new Ship(3, boardOwn);
            ships[0] = new Ship(3, boardOwn);
            ships[0] = new Ship(4, boardOwn);
            ships[0] = new Ship(1, boardOwn);
            ships[0] = new Ship(1, boardOwn);
            ships[0] = new Ship(1, boardOwn);
            ships[0] = new Ship(1, boardOwn);
        }
        
        public void createEnemyBoard(string[] data)
        {
            for (int i = 0; i < 10; i++)
                for (int k = 0; k < 10; k++)
                {

                    Cell cell = new Cell();
                    cell.Location = new Point(pos_x + i * 30, pos_y + k * 30);
                    cell.TeamCell = team;
                    for(int j = 0; j <= data.Length - 2; j++)
                        if (Convert.ToInt32(data[j]) == cell.Number)
                        {
                            cell.StateCell = State.EnemyShip;
                        }


                    cells[i, k] = cell;

                    form.Invoke(new Action(() =>
                    {
                        form.Controls.Add(cell);
                    }));
                    
                }
           
        }
        //обнорвление состояние кораблей на полное уничтожение
        public void UpdateStatusShip()
        {
            foreach (Ship ship in ships)
                if (ship.ShipIsDestr())
                    ship.DestroyShip();
        }

        public string GetPositionShips()
        {
            //Записываем позиции всех кораблей в строку
            string data = null;
            for (int i = 0; i < positionShips.Count; i++)
                data += positionShips[i].ToString() + " ";

            //Преобразуем строку в байты для отправки
            //byte[] buff = Encoding.Unicode.GetBytes(data);
            //return buff;
            return data;
        }


    }
}
