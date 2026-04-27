using Lab6.Objects;
using System.Xml.Linq;

namespace Lab6
{
    public partial class Form1 : Form
    {
        List<BaseObject> objects = new();
        Player player;
        Marker marker;
        Enemy enemy;

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
            Enemy newEnemy = new Enemy(random.Next(30, pbMain.Width - 30), random.Next(30, pbMain.Height - 30), 0);
            objects.Add(newEnemy);
            newEnemy.OnLifeTimeEnemy += (e) =>
            {
                objects.Remove(e);
                CreateEnemy();
            };

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
