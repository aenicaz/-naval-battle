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
    class Board
    {
        public static List<Board> boards = new List<Board>();

        
        Team team;  //Принадлежность доски

        int pos_x;  //Позиции доски
        int pos_y;

        static Form1 form;      //Форма на которой отображаем
        public Cell[,] cells;   //Клетки принадлежащие доске

        public List<string> positionShips = new List<string>(); //Позиции всех кораблей на доске
        private Ship[] ships = new Ship[10]; //Корабли
        private int AllDestroyShips;
        

        public Board(int x, int y, int size, Form1 _form, Team _team)
        {
            pos_x = x;
            pos_y = y;
            cells = new Cell[size, size];
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

        
        //СОздание своей доски
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

            boards.Add(this);
        }

        //Создание своих кораблей
        public void CreateShips(Board boardOwn)
        {
            ships[0] = new Ship(2, boardOwn);
            ships[1] = new Ship(2, boardOwn);
            ships[2] = new Ship(2, boardOwn);
            ships[3] = new Ship(3, boardOwn);
            ships[4] = new Ship(3, boardOwn);
            ships[5] = new Ship(4, boardOwn);
            ships[6] = new Ship(1, boardOwn);
            ships[7] = new Ship(1, boardOwn);
            ships[8] = new Ship(1, boardOwn);
            ships[9] = new Ship(1, boardOwn);
        }

        //Создаём доску противника, размещаем корабли
        public void createEnemyBoard(string data)
        {
            for (int i = 0; i < 10; i++)
                for (int k = 0; k < 10; k++)
                {
                    Cell cell = new Cell();
                    cell.Location = new Point(pos_x + i * 30, pos_y + k * 30);
                    cell.TeamCell = team;
                    cells[i, k] = cell;
                    form.Invoke(new Action(() =>
                    {
                        form.Controls.Add(cell);
                    }));  
                }
                 
            boards.Add(this);


            string[] shipsPos = data.Split(',');
            for (int i = 0; i < ships.Length; i++)
            {
                string pos = shipsPos[i]; //получаем позиции одного корабля
                ships[i] = new Ship(pos, boards[1]);
            }
        }
       
        //обнорвление состояние кораблей на полное уничтожение
        public void UpdateStatusShip()
        {
            for(int i = 0; i < ships.Length; i++)
            {
                if (ships[i].ShipIsDestr())
                {
                    ships[i].DestroyShip();

                    AllDestroyShips++; //увеличиваем кол. уничтоженных корраблей
                    if (AllDestroyShips >= 10)
                        MessageBox.Show("Вы победили");
                        
                }
                    
            }
            
        }

        //Получить позиции кораблей на доске
        public string GetPositionShips()
        {
            //Записываем позиции всех кораблей в строку
            string data = null;
            for (int i = 0; i < positionShips.Count; i++)
                data += positionShips[i].ToString();

            //Преобразуем строку в байты для отправки
            //byte[] buff = Encoding.Unicode.GetBytes(data);
            //return buff;
            return data;
        }


        //Обработка стрельбы
        public void Shoot(string data)   //data - номер клетки
        {
            int number = Convert.ToInt32(data);
            for (int i = 0; i <= 9; i++)
                for (int k = 0; k <= 9; k++)
                    if (number == cells[i, k].Number) //проверяем состояние клетки. Пустая или с кораблём
                    {
                        if (cells[i, k].StateCell == State.empty || cells[i, k].StateCell == State.Block)
                            cells[i, k].StateCell = State.EmptyShot;

                        if (cells[i, k].StateCell == State.Ship)
                            cells[i, k].StateCell = State.PartShipIsDestr;
                    }
                        
                        
        }
    }
}
