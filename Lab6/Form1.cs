using Lab6.Objects;
using System.Xml.Linq;

namespace Lab6
{
    public partial class Form1 : Form
    {
        List<BaseObject> objects = new();
        Player player;
        Marker marker;
        EnemyFirst enemyFirst;
        EnemySecond enemySecond;

        int score = 0;
        Random random = new Random();

        public Form1()
        {
            InitializeComponent();

            player = new Player(pbMain.Width / 2, pbMain.Height / 2, 0);

            player.OnOverlap += (p, obj) =>
            {
                txtLog.Text = $"[{DateTime.Now:HH:mm:ss:ff}] Игрок пересекся с {obj}\n" + txtLog.Text;
            };

            player.OnMarkerOverlap += (m) =>
            {
                objects.Remove(m);
                marker = null;
            };

            player.OnEnemyFirstOverlap += (e) =>
            {
                score++;
                txtLog.Text = $"[{DateTime.Now:HH:mm:ss:ff}] Враг уничтожен! Очки: {score}\n" + txtLog.Text;
                lblScore.Text = $"Очки: {score}"; 
                objects.Remove(e);
                CreateEnemy();
            };
            player.OnEnemySecondOverlap += (e) =>
            {
                score++;
                txtLog.Text = $"[{DateTime.Now:HH:mm:ss:ff}] Враг уничтожен! Очки: {score}\n" + txtLog.Text;
                lblScore.Text = $"Очки: {score}";
                objects.Remove(e);
                CreateEnemy();
            };
            CreateEnemy();
            CreateEnemy();
            objects.Add(player);
        }


        private void CreateEnemy()
        {
            if (random.Next(0, 2) == 0)
            {
                EnemyFirst newEnemyFirst = new EnemyFirst(random.Next(30, pbMain.Width - 30), random.Next(30, pbMain.Height - 30), 0);
                objects.Add(newEnemyFirst);
                newEnemyFirst.OnLifeTimeEnemy += (e) =>
                {
                    objects.Remove(e);
                    CreateEnemy();
                };
            }
            else
            {
                EnemySecond newEnemySecond = new EnemySecond(random.Next(30, pbMain.Width - 30), random.Next(30, pbMain.Height - 30), 0);
                objects.Add(newEnemySecond);
                newEnemySecond.OnSizeEnemy += (e) =>
                {
                    objects.Remove(e);
                    CreateEnemy();
                };
            }
        }

        private void pbMain_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.Clear(Color.White);

            foreach (var obj in objects.ToList())
            {
                if (obj != player && player.Overlaps(obj, g))
                {
                    player.Overlap(obj);
                    obj.Overlap(player);
                }
            }

            foreach (var obj in objects.ToList())
            {
                g.Transform = obj.GetTransform();
                obj.Render(g);
            }
        }

        private void pbMain_Click(object sender, EventArgs e)
        {
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (marker != null)
            {
                float dx = marker.X - player.X;
                float dy = marker.Y - player.Y;

                float length = MathF.Sqrt(dx * dx + dy * dy);
                dx /= length;
                dy /= length;

                player.X += dx * 2;
                player.Y += dy * 2;

                player.vX += dx * 0.5f;
                player.vY += dy * 0.5f;

                player.Angle = 90 - MathF.Atan2(player.vX, player.vY) * 180 / MathF.PI;
            }

            player.vX += -player.vX * 0.1f;
            player.vY += -player.vY * 0.1f;

            player.X += player.vX;
            player.Y += player.vY;

            pbMain.Invalidate();
        }

        private void pbMain_MouseClick(object sender, MouseEventArgs e)
        {
            if (marker == null)
            {
                marker = new Marker(e.X, e.Y, 0);
                objects.Add(marker);
            }
            else
            {
                marker.X = e.X;
                marker.Y = e.Y;
            }
        }
    }
}
