using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace МорскойБой
{
    class Ship
    {
        private int length; //длина корабля
        private Cell[] cells; //массив клеток ответственных за корабль
        private static Board board;

        public Ship(int _lenght, Board _board) //конструктор 
        {
            length = _lenght;
            cells = new Cell[length];
            if(board == null)
                board = _board;
            CreateShip();
        }

        public Ship(string pos , Board _board)
        {
            /*
            length = _length;
            cells = new Cell[length];
            for(int i = 0; i <= 9; i++)
                for(int k = 0; k <= 9; k++)
                {
                    for(int j = 0; j <= pos.Length; j++)
                        if(board.cells[i,k].Number == pos[j])
                        {
                            board.cells[i, k].StateCell = State.EnemyShip;
                            cells[j] = board.cells[i, j];
                        }
                }
               */
            string[] data = pos.Split(':');
            length = data.Length - 1;
            cells = new Cell[length];

            for (int i = 0; i <= 9; i++)
                for(int k = 0; k <= 9; k++)
                {
                    for (int j = 0; j <= length; j++)
                        if (_board.cells[i, k].Number.ToString() == data[j])
                        {
                            _board.cells[i, k].StateCell = State.EnemyShip;
                            cells[j] = _board.cells[i, k];
                        }
                }
            //CreateEnemyShip(pos);
        }

        public int Length()
        {
            return length;
        }
        //Размещение кораблей

        public void CreateEnemyShip(string data)
        {
            
        }
        public void CreateShip()
        {
            Random r = new Random();
            int x;
            int y;
            int vector;   // 0-расположение по горизонтали
                                         // 1-расположение по вертикали

            //логика размещения для одиночного корабля x + length - 1 < 9 && vector == 0
            if(length == 1)
            {
                while (true)
                {
                    x = r.Next(1, 10);
                    y = r.Next(1, 9);
                   
                    if (board.cells[x, y].StateCell != State.Ship && board.cells[x, y].StateCell != State.Block)
                    {
                        board.cells[x, y].StateCell = State.Ship;
                        //Добавляем номер клетки в которой есть корабль
                        board.positionShips.Add(board.cells[x, y].Number.ToString() + ":");
                        cells[0] = board.cells[x, y];
                        board.positionShips.Add(",");

                        if (x > 8)
                        {
                            //board.cells[x + 1, y].StateCell = State.Block;
                            board.cells[x - 1, y].StateCell = State.Block;
                            board.cells[x, y + 1].StateCell = State.Block;
                            board.cells[x, y - 1].StateCell = State.Block;
                            //board.cells[x + 1, y + 1].StateCell = State.Block;
                            board.cells[x - 1, y - 1].StateCell = State.Block;
                            //board.cells[x + 1, y - 1].StateCell = State.Block;
                            board.cells[x - 1, y + 1].StateCell = State.Block;
                            break;
                        }
                        else
                        {
                            board.cells[x + 1, y].StateCell = State.Block;
                            board.cells[x - 1, y].StateCell = State.Block;
                            board.cells[x, y + 1].StateCell = State.Block;
                            board.cells[x, y - 1].StateCell = State.Block;
                            board.cells[x + 1, y + 1].StateCell = State.Block;
                            board.cells[x - 1, y - 1].StateCell = State.Block;
                            board.cells[x + 1, y - 1].StateCell = State.Block;
                            board.cells[x - 1, y + 1].StateCell = State.Block;
                        }
                        break;
                    }
                }
            }

            //Для кораблей lenght > 1
            if(length > 1)
            {
               
                bool flag = true;

                while (flag)
                {
                    x = r.Next(1, 9);
                    y = r.Next(1, 9);
                    vector = r.Next(0, 2);

                    bool c = true;

                    if (x + length < 10 && vector == 0)
                    {
                        for(int i = 0; i <= length - 1; i++)
                        {
                            if(board.cells[x+i,y].StateCell == State.Block || board.cells[x + i, y].StateCell == State.Ship)
                            {
                                c = false;
                                break;
                            }
                        }
                        if (!c) //Если для корабля не подходит место, то начинаем снова
                            continue;

                        for (int i = 0; i <= length - 1; i++)
                        {
                            board.cells[x + i, y].StateCell = State.Ship;
                            board.positionShips.Add(board.cells[x + i, y].Number.ToString() + ":");
                            cells[i] = board.cells[x + i, y];

                            if(i == 0)
                            {
                                board.cells[x + i, y + 1].StateCell = State.Block;
                                board.cells[x + i, y - 1].StateCell = State.Block;
                                board.cells[x + i - 1, y - 1].StateCell = State.Block;
                                board.cells[x + i - 1, y + 1].StateCell = State.Block;
                                board.cells[x - 1 - i,y].StateCell = State.Block;
                            }
                            else if(i == length - 1)
                            {
                                board.cells[x + i, y + 1].StateCell = State.Block;
                                board.cells[x + i, y - 1].StateCell = State.Block;
                                board.cells[x + i + 1, y - 1].StateCell = State.Block;
                                board.cells[x + i + 1, y + 1].StateCell = State.Block;
                                board.cells[x + 1 + i, y].StateCell = State.Block;
                            }
                            else
                            {
                                board.cells[x + i, y + 1].StateCell = State.Block;
                                board.cells[x + i, y - 1].StateCell = State.Block;
                            }

                            flag = false;
                            
                        }
                        board.positionShips.Add(",");

                    }

                    if (x + length> 10 && vector == 0)
                    {
                        for (int i = 0; i <= length - 1; i++)
                        {
                            if (board.cells[x - i, y].StateCell == State.Block || board.cells[x - i, y].StateCell == State.Ship)
                            {
                                c = false;
                                break;
                            }
                        }

                        if (!c)
                            continue;

                        for (int i = 0; i <= length - 1; i++)
                        {
                            board.cells[x - i, y].StateCell = State.Ship;
                            board.positionShips.Add(board.cells[x-i, y].Number.ToString() + ":");
                            cells[i] = board.cells[x - i, y];

                            if (i == 0)
                            {
                                board.cells[x, y - 1].StateCell = State.Block;
                                board.cells[x, y + 1].StateCell = State.Block;
                                board.cells[x + 1, y].StateCell = State.Block;
                                board.cells[x + 1, y - 1].StateCell = State.Block;
                                board.cells[x + 1, y + 1].StateCell = State.Block;
                            }
                            else if (i == length - 1)
                            {
                                board.cells[x - i - 1, y + 1].StateCell = State.Block;
                                board.cells[x - i - 1, y - 1].StateCell = State.Block;
                                board.cells[x - i, y + 1].StateCell = State.Block;
                                board.cells[x - i, y - 1].StateCell = State.Block;
                                board.cells[x - i - 1, y].StateCell = State.Block;
                            }
                            else
                            {
                                board.cells[x - i, y + 1].StateCell = State.Block;
                                board.cells[x - i, y - 1].StateCell = State.Block;
                            }

                            flag = false;
                           
                        }
                        board.positionShips.Add(",");
                    }

                    if (y + length < 10 && vector == 1)
                    {
                        for (int i = 0; i <= length - 1; i++)
                        {
                            if (board.cells[x, y + i].StateCell == State.Block || board.cells[x, y + i].StateCell == State.Ship)
                            {
                                c = false; 
                                break;
                            }
                        }

                        if (!c)
                            continue;

                        for (int i = 0; i <= length - 1; i++)
                        {
                            board.cells[x, y + i].StateCell = State.Ship;
                            board.positionShips.Add(board.cells[x, y+i].Number.ToString() + ":");
                            cells[i] = board.cells[x, y + i];

                            if (i == 0)
                            {
                                board.cells[x - 1, y].StateCell = State.Block;
                                board.cells[x + 1, y].StateCell = State.Block;
                                board.cells[x, y-1].StateCell = State.Block;
                                board.cells[x - 1, y - 1].StateCell = State.Block;
                                board.cells[x + 1, y - 1].StateCell = State.Block;



                            }
                            else if (i == length - 1)
                            {
                                board.cells[x - 1, y + i].StateCell = State.Block;
                                board.cells[x + 1, y + i].StateCell = State.Block;
                                board.cells[x, y + i + 1].StateCell = State.Block;
                                board.cells[x - 1, y + i + 1].StateCell = State.Block;
                                board.cells[x + 1, y + i + 1].StateCell = State.Block;
                            }
                            else
                            {
                                board.cells[x + 1, y + i].StateCell = State.Block;
                                board.cells[x - 1, y + i].StateCell = State.Block;
                            }

                            flag = false;
                            
                        }
                        board.positionShips.Add(",");
                    }

                    if (y + length > 10 && vector == 1)
                    {
                        for (int i = 0; i <= length - 1; i++)
                        {
                            if (board.cells[x, y - i].StateCell == State.Block || board.cells[x, y - i].StateCell == State.Ship)
                            {
                                c = false;
                                break;
                            }
                        }

                        if (!c)
                            continue;

                        for (int i = 0; i <= length - 1; i++)
                        {
                            board.cells[x, y - i].StateCell = State.Ship;
                            board.positionShips.Add(board.cells[x, y-i].Number.ToString() + ":");
                            cells[i] = board.cells[x, y - i];

                            if (i == 0)
                            {
                                board.cells[x - 1, y].StateCell = State.Block;
                                board.cells[x + 1, y].StateCell = State.Block;
                                board.cells[x, y + 1].StateCell = State.Block;
                                board.cells[x - 1, y + 1].StateCell = State.Block;
                                board.cells[x + 1, y + 1].StateCell = State.Block;
                            }
                            else if (i == length - 1)
                            {
                                board.cells[x - 1, y - i].StateCell = State.Block;
                                board.cells[x + 1, y - i].StateCell = State.Block;
                                board.cells[x, y - i - 1].StateCell = State.Block;
                                board.cells[x - 1, y - i - 1].StateCell = State.Block;
                                board.cells[x + 1, y - i - 1].StateCell = State.Block;


                            }
                            else
                            {
                                board.cells[x + 1, y - i].StateCell = State.Block;
                                board.cells[x - 1, y - i].StateCell = State.Block;
                            }

                            flag = false;

                        }
                        board.positionShips.Add(",");
                    }
                  
                    System.Threading.Thread.Sleep(50);
                }

                
            }
        }
        //Проверка на полное уничтожение коробля
        public bool ShipIsDestr()
        {
            int count = 0;
            for (int i = 0; i < length; i++)
                if (cells[i].StateCell == State.PartShipIsDestr)
                    count++;

            //Если количество попаданий равно длине корабля, то корабль уничтожен
            if (count == length)
                return true;
            else
                return false;
           
        }


        //Уничтожаем корабль
        public void DestroyShip()
        {
            for (int i = 0; i < length; i++)
                cells[i].StateCell = State.ShipIsDestr;
        }
    }
}
