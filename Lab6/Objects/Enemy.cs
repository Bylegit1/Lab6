using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab6.Objects
{
    class Enemy : BaseObject
    {
        public Action<Enemy> OnLifeTimeEnemy;

        public int TimeToLive = 5;  
        public int TimerLoopsCount = 0;

        public Enemy(float x, float y, float angle) : base(x, y, angle)
        {
        }

        public override void Render(Graphics g)
        {
            g.FillEllipse(new SolidBrush(Color.BurlyWood),
                -15, -15,
                30, 30);

            g.DrawString(
                TimeToLive.ToString(),
                new Font("Arial", 10),
                new SolidBrush(Color.Black),
                10, 10
            );

            TimerLoopsCount++;
            if (TimerLoopsCount == 35)  
            {
                TimerLoopsCount = 0;
                TimeToLive -= 1;
            }

            if (TimeToLive == 0)
            {
                OnLifeTimeEnemy(this);
            }
        }

        public override GraphicsPath GetGraphicsPath()
        {
            var path = base.GetGraphicsPath();
            path.AddEllipse(-15, -15, 30, 30);
            return path;
        }

        public override void Overlap(BaseObject obj)
        {
            base.Overlap(obj);

            if (obj is Enemy)
            {
                OnLifeTimeEnemy(obj as Enemy);
            }
        }
    }
}
