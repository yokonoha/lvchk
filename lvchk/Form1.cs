using NAudio.CoreAudioApi;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace lvchk
{

    public partial class Form1 : Form
    {
        private MMDevice defaultDevice;
        System.Drawing.Image imageA = Properties.Resources.al1;
            System.Drawing.Image imageB = Properties.Resources.al2;
        public Form1()
        {
            this.BackColor = Color.Black;
//            this.TransparencyKey = this.BackColor;
            InitializeComponent();
            var enumerator = new MMDeviceEnumerator();
            defaultDevice = enumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);


            timer1.Start();
            timer2.Start();
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            var screenArea = Screen.PrimaryScreen.WorkingArea;
            // 左下
            this.Location = new Point(
                0,
                screenArea.Height - this.Height
            );
        }

        Point lastPoint;

        private void timer1_Tick(object sender, EventArgs e)
        {
            
            pictureBox2.Invalidate();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            notifyIcon1.ShowBalloonTip(5);
        }

        private void pictureBox2_Paint(object sender, PaintEventArgs e)
        {
            float peak = defaultDevice.AudioMeterInformation.MasterPeakValue;


            double dB = (peak > 0) ? 20 * Math.Log10(peak) : -60.0;

            double minDB = -60.0;
            double maxDB = 10.0;

            double displayValue = ((dB - minDB) / (maxDB - minDB)) *100;


            if (displayValue < 0) displayValue = 0;
            if (displayValue > 100) displayValue = 100;


            Graphics g = e.Graphics;
            g.Clear(Color.Black);


            float ratio = (float)(displayValue / 100.0);
            int barHeight = (int)(pictureBox2.Height * ratio);

            if (barHeight > 0)
            {
                using (var brush = new System.Drawing.Drawing2D.LinearGradientBrush(
                    pictureBox2.ClientRectangle, Color.DarkGreen, Color.Red, 90f))
                {
                    g.FillRectangle(brush, 0, pictureBox2.Height - barHeight, pictureBox2.Width, barHeight);
                }
            }

            int[] labels = { -60, -40, -20, 0 };
            using (Font font = new Font("Arial", 8))
            using (Pen linePen = new Pen(Color.Gray, 1))
            {
                foreach (int labelDb in labels)
                {

                    float labelRatio = (float)((labelDb - minDB) / (maxDB - minDB));
                    int y = pictureBox2.Height - (int)(pictureBox2.Height * labelRatio);

                    if (y >= pictureBox2.Height) y = pictureBox2.Height - 15;
                    if (y <= 0) y = 5;


                    g.DrawLine(linePen, 0, y, 10, y);
                    g.DrawString($"{labelDb}dB", font, Brushes.White, 12, y - 6);
                }
            }
        }

        private void keijomodify(object sender, MouseEventArgs e)
        {

        }

        private double chokkinDB = -60.0;
        private int displayTimer = 0;
        private void timer2_Tick(object sender, EventArgs e)
        {
            float peak = defaultDevice.AudioMeterInformation.MasterPeakValue;
            double dB = (peak > 0) ? 20 * Math.Log10(peak) : -60.0;


            if (dB - chokkinDB > 3.0)
            {
                displayTimer = 1;
            }
            if (displayTimer > 0)
            {
                displayTimer--;
            }
            else
            {
            }
            chokkinDB = dB;
        }

        private void pictureBox1_DoubleClick(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            lastPoint = e.Location;

        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Left += e.X - lastPoint.X;
               // this.Top += e.Y - lastPoint.Y;
            }
        }

        private void contextMenuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem.Text == "Quit")
            {
                Application.Exit();
            }
            else
            {
                osl osl= new osl();
                osl.Show();
            }
        }
    }
}
