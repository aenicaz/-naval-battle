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
        private bool busy = true;
        
        //Размеры клетки
        private int height = 28;
        private int width = 28;
 
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

        public bool Busy
        {
            get { return busy; }
            set { busy = value; }
        }
        
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
                case State.Ship: //отрисовка поля с кораблём
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
                case State.EnemyShip: //отрисовка поля с чужим кораблём
                    e.Graphics.FillRectangle(new SolidBrush(colorCell), rect);
                    break;
            }

             
            

        }


        //событие нажатия на клетку
        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);
            if (this.team == Team.Enemy)
            {
                if (this.StateCell == State.empty)
                {
                    this.StateCell = State.EmptyShot;
                }
                    
                
                if (this.StateCell == State.EnemyShip)
                    this.StateCell = State.PartShipIsDestr;

                ServerObject.getInstance().Shoot(this.Number);
                
                Board.boards[1].UpdateStatusShip();
                
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
}
