using System.Drawing;
using System.Drawing.Drawing2D;
using System.Diagnostics;
using System.Windows.Forms;
using System.Reflection.Emit;

namespace Homework_1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void runBtn_Click(object sender, EventArgs e)
        {
            Graphics graph = graphBox.CreateGraphics();
            graph.Clear(Color.White);

            Graphics lab1 = labelTxt1.CreateGraphics();
            lab1.Clear(Color.White);

            Graphics hist = histoBox.CreateGraphics();
            hist.Clear(Color.White);


            int servers = int.Parse(nServers.Text);
            int hackers = int.Parse(nHackers.Text);
            float prob = float.Parse(probability.Text);


            float lineWidth = graphBox.Width / (float)servers;
            float lineHeight = graphBox.Height / (float)servers;


            graph.SmoothingMode = SmoothingMode.AntiAlias;
            graph.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graph.PixelOffsetMode = PixelOffsetMode.HighQuality;

            Font font = new Font("Arial", 10);
            Brush brush = Brushes.Black;

            lab1.DrawString("0".ToString(), font, brush, 0f,0f);

            PointF point1 = new PointF(0.0f, 0.0f);

            for (double i = 0; i < 1.5;  i+=0.5)
            {
                double n = i * servers;
                
                int label = (int)n;
                lab1.DrawString(label.ToString(), font, brush,point1);
                point1.X += (graphBox.Width / 2) - 10f; 

            }


            List<int> scores = DrawGraph(graph, servers, hackers, prob, lineWidth, lineHeight);
            DrawHistogram(hist, hackers, servers, scores, lineHeight);



        }



        private List<int> DrawGraph(Graphics graph, int servers, int hackers, float prob, float linew, float lineh)
        {
            List<int> totalscores = new List<int>();

            float lineWidth = graphBox.Width / (float)servers;
            float lineHeight = graphBox.Height / (float)servers;

            Random rnd = new Random();

   

            for (int i = 0; i < hackers; i++)
            {
                Pen p = new Pen(Color.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256)));

                int score = 0;

                PointF position = new PointF(0.0f, graphBox.Height);

                for (int j = 0; j < servers; j++)
                {
                    if (rnd.NextDouble() <= prob)
                    {
                        PointF end= new PointF(position.X, position.Y - lineHeight);
                        graph.DrawLine(p, position, end);
                        position = end;
                        score++;
                    }

                    PointF next = new PointF(position.X+lineWidth, position.Y);

                    graph.DrawLine(p, position, next); 

                    position = next;

                }
                totalscores.Add(score);
            }

            return totalscores;
        }

        private void DrawHistogram(Graphics graph, int hackers,int servers, List<int> scores, float height)
        {
            Pen pen = new Pen(Color.White);

            var g = scores.GroupBy(i => i).OrderBy(group => group.Key);
            

            var m1 = scores.GroupBy(i => i).OrderByDescending(group => group.Count()).First().Count();

            int diffScores = scores.Distinct().Count();

            float scoreHeigth = (histoBox.Width * 0.9f)  / (float)m1;
            float scoreWidth = (histoBox.Height * 0.6f ) / (float)diffScores;

            float startPoint = histoBox.Height * 0.9f;

            foreach (var grp in g)
            {

                SolidBrush blueBrush = new SolidBrush(Color.Blue);

                RectangleF rect = new RectangleF(0, startPoint, scoreHeigth * grp.Count(), scoreWidth);
                //Debug.WriteLine("{0} {1}", grp.Key, grp.Count());
                graph.FillRectangle(blueBrush, rect);
                graph.DrawRectangle(pen,0f, startPoint, scoreHeigth * grp.Count(), scoreWidth);
                startPoint -= scoreWidth;
               

            }


        }

    }
}




    /*
     We have n servers with m attackers. 
    The hacker as probability p to penetrate each server. 
    Make a graphical representation (line flat if hacker doesnt
    penetrate 
    and a jump if to 1 if he penetrates), try different n,m,p/
    At time n we want to count how many reached each level.
    Useful functions: random, drawline(s), drawrectangle.
     
     */