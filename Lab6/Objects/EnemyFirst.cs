using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab6.Objects
{
    class EnemyFirst : BaseObject
    {
        public Action<EnemyFirst> OnLifeTimeEnemy;

        public int TimeToLive = 240;  

        public EnemyFirst(float x, float y, float angle) : base(x, y, angle)
        {
        }

        public override void Render(Graphics g)
        {
            g.FillEllipse(new SolidBrush(Color.BurlyWood),
                -15, -15,
                30, 30);
            
            int resultTime = TimeToLive / 40;

            g.DrawString(
                resultTime.ToString(),
                new Font("Arial", 10),
                new SolidBrush(Color.Black),
                10, 10
            );

            TimeToLive--;

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

            if (obj is EnemyFirst)
            {
                OnLifeTimeEnemy(obj as EnemyFirst);
            }
        }
    }
}
