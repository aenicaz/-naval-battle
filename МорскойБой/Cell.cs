using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace МорскойБой
{
    public class Cell : Control
    {
        public Cell()
        {
            this.Size = new Size(28, 28); //размеры области элемента
            Number = count; count++;
            if (count == 100)
                count = 0;
            

        }

        private int number;
        private static int count = 1;
        //Принадлежность клетки
        private Team team; 
        //Цвет клетки
        private Color colorCell = Color.FromArgb(130, 140, 230);
        
        //Color.FromArgb(0, 0, 200) - цвет корабля
        //Color.FromArgb(130, 140, 230) - стандартный цвет поля

        //Тип клетки
        private State stateCell = State.empty;
        //Доступность для заполнения
              
        //Размеры клетки
        private int height = 28;
        private int width = 28;
 
        //Номер клетки
        public int Number
        {
            get
            {
                return number;
            }
            private set
            {
                number = value;                
            }
        }

        
        //Цвет клетки
        public Color ColorCell
        {
            get
            {
                return colorCell;
            }
            set
            {
                colorCell = value;

                // The Invalidate method invokes the OnPaint method described 
                // in step 3.
                Invalidate();
            }
        }
        
        //Состояние клетки
        public State StateCell
        {
            get
            {
                return stateCell;
            }
            set
            {
                stateCell = value;
                Invalidate();
            }
        }

        //Принадлежность клетки
        public Team TeamCell
        {
            get
            {
                return team;
            }
            set
            {
                team = value;
            }
        }

        
        //Отрисовка элемента
        protected override void OnPaint(PaintEventArgs e)
        {
            Rectangle rect = new Rectangle(0, 0, width, height);
            RectangleF rectF = new RectangleF(new Point(11, 11), new Size(5,5));

            base.OnPaint(e);
            switch(stateCell)
            {
                case State.empty: //отрисовка пустого поля 
                    e.Graphics.FillRectangle(new SolidBrush(colorCell), rect);
                    break;
                case State.Ship: //отрисовка поля с кораблём у клиента
                    e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(0, 0, 200)), rect);
                    break;
                case State.PartShipIsDestr: //отрисовка части не до конца подбитого корабля
                    e.Graphics.FillRectangle(new SolidBrush(Color.Red), rect);
                    break;
                case State.ShipIsDestr: //отрисовка части полностью подбитого корабля
                    e.Graphics.FillRectangle(new SolidBrush(Color.Black), rect);
                    break;
                case State.Block: //Поле заблокированное для размещения кораблей
                    e.Graphics.FillRectangle(new SolidBrush(colorCell), rect);
                    break;
                case State.EmptyShot://отрисовка попадания в поле без корабля
                    e.Graphics.FillRectangle(new SolidBrush(colorCell), rect);
                    e.Graphics.FillEllipse(new SolidBrush(Color.Black), rectF);
                    break;
                case State.EnemyShip: //отрисовка не обнаруженного корабля на чужом поле
                    e.Graphics.FillRectangle(new SolidBrush(colorCell), rect);
                    break;
            }

             
            

        }


        //событие нажатия на клетку
        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);
            //Если наша очередь нажимать и мы нажали на клетку противника то....
            if (this.team == Team.Enemy && Form1.Busy == Busy.on)
            {
                if (this.StateCell == State.empty)  //Если нажали на пустую клетку противника
                {
                    ServerObject.getInstance().Shoot(this.Number, '-'); //Отрправили информацию о выстреле
                    this.StateCell = State.EmptyShot; //Отрисовали выстрел по пустой клетке
                    Form1.Busy = Busy.off; //Запрещаем стрелять ещё
                }
                    
                
                if (this.StateCell == State.EnemyShip) //Если попали по кораблю
                {
                    this.StateCell = State.PartShipIsDestr; //Отрисовываем попадание по части корабля
                    ServerObject.getInstance().Shoot(this.Number, '+'); //Отрправили информацию о выстреле
                }
                    

                Board.boards[1].UpdateStatusShip(); //проверяем корабли на полное уничтожение
                
            }


           

        }


    }
    
    public enum State
    {
        empty,
        PartShipIsDestr,
        ShipIsDestr,
        Block,
        Ship,
        EmptyShot,
        EnemyShip
    }

    public enum Team
    {
        Own,
        Enemy
    }

    public enum Busy
    {
        on,
        off
    }
}
