using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab6.Objects
{
    class EnemySecond : BaseObject
    {
        public Action<EnemySecond> OnSizeEnemy;

        private int Size;
       
        public EnemySecond(float x, float y, float angle) : base(x, y, angle)
        {
            Random randomSize = new Random();
            Size = randomSize.Next(60,150);
        }

        public override void Render(Graphics g)
        {
            g.FillEllipse(new SolidBrush(Color.BurlyWood),
                -Size/2, -Size/2,
                Size, Size);

            Size--;

            if(Size == 0)
            {
                OnSizeEnemy(this);
            }

        }

        public override GraphicsPath GetGraphicsPath()
        {
            var path = base.GetGraphicsPath();
            path.AddEllipse(-Size/2, -Size/2, Size, Size);
            return path;
        }

        public override void Overlap(BaseObject obj)
        {
            base.Overlap(obj);

            if (obj is EnemySecond)
            {
                OnSizeEnemy(obj as EnemySecond);
            }
        }
    }
}
