using Lab6.Objects;
using System.Xml.Linq;

namespace Lab6
{
    public partial class Form1 : Form
    {
        List<BaseObject> objects = new();
        Player player;
        Marker marker;
        Enemy enemyFirst;
        Enemy enemySecond;
        Label lblScore;

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

            player.OnEnemyOverlap += (e) =>
            {
                score++;
                txtLog.Text = $"[{DateTime.Now:HH:mm:ss:ff}] Враг уничтожен! Очки: {score}\n" + txtLog.Text;
                lblScole.Text = $"Очки: {score}"; 
                objects.Remove(e);

                if (e == enemyFirst)
                {
                    enemyFirst = null;
                    CreateEnemy("Враг 1");
                }
                else if (e == enemySecond)
                {
                    enemySecond = null;
                    CreateEnemy("Враг 2");
                }
            };
            CreateEnemy("Враг 1");
            CreateEnemy("Враг 2");
            objects.Add(player);
        }


        private Enemy CreateEnemy(string name)
        {
            Enemy enemy = null;

            if (enemyFirst == null)
            {
                enemyFirst = new Enemy(random.Next(30, pbMain.Width - 30), random.Next(30, pbMain.Height - 30), 0);
                enemy = enemyFirst;
            }
            else if (enemySecond == null)
            {
                enemySecond = new Enemy(random.Next(30, pbMain.Width - 30), random.Next(30, pbMain.Height - 30), 0);
                enemy = enemySecond;
            }

            objects.Add(enemy);
            return enemy;

        }

        private void pbMain_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.Clear(Color.White);

            updatePlayer();

            foreach (var obj in objects.ToList())
            {
                if (obj != player && player.Overlaps(obj, g))
                {
                    player.Overlap(obj);
                    obj.Overlap(player);
                }
            }

            foreach (var obj in objects)
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
            pbMain.Invalidate();
        }

        private void updatePlayer()
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
