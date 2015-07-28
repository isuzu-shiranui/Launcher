using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Launcher
{
    public partial class SelectServer : Form
    {
        public SelectServer()
        {
            InitializeComponent();
        }

        public string SelectedServer;

        private MainForm mf = null;

        private void SelectServer_Load(object sender, EventArgs e)
        {
            pictureBox1.MouseDown += new MouseEventHandler(SelectServer_MouseDown);//イベントハンドラ作成
            pictureBox1.MouseMove += new MouseEventHandler(SelectServer_MouseMove);//イベントハンドラ作成

            label2.Text = label3.Text = label4.Text = "";

            pictureBox2.Location = new Point(0 - pictureBox2.Width, 88);
            pictureBox3.Location = new Point(274, 0 - pictureBox3.Height);
            pictureBox4.Location = new Point(this.Width + pictureBox4.Width, 88);
        }

        private Point mousePoint;//マウスポイント

        //フォームを動かす
        private void SelectServer_MouseDown(object sender, MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
            {
                mousePoint = new Point(e.X, e.Y);//位置を記憶する
            }
        }

        //フォームを動かす
        private void SelectServer_MouseMove(object sender, MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
            {
                this.Left += e.X - mousePoint.X;
                this.Top += e.Y - mousePoint.Y;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        int i = -256;
        int j = 319;
        int k = 806;
        private void timer1_Tick(object sender, EventArgs e)
        {
            pictureBox2.Location = new Point(i, 88);
            if (i >= 12)
            {
                timer1.Stop();
            }
            i+=10;
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            pictureBox3.Location = new Point(274, j);
            if (j <= 88)
            {
                timer2.Stop();
            }
            j-=8;
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            pictureBox4.Location = new Point(k, 88);
            if (k <= 536)
            {
                timer3.Stop();
                timer4.Start();
            }
            k-=10;
        }

        private void timer4_Tick(object sender, EventArgs e)
        {
            if (label3.Text != "")
            {
                label4.Text = "Challenge";
            }
            if (label2.Text != "")
            {
                label3.Text = "Utopia";
            }
            if (label2.Text == "")
            {
                label2.Text = "Frontia";
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            SelectedServer = "Frontia";
            if ((this.mf == null) || this.mf.IsDisposed)
            {
                MainForm mf = new MainForm();
                mf.Show();
                this.Close();
                ConsoleForm.ConsoleFormInstance.richTextBox1AppendText = "Selected Server: " + SelectedServer;
            }
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            SelectedServer = "Utopia";
            if ((this.mf == null) || this.mf.IsDisposed)
            {
                MainForm mf = new MainForm();
                mf.Show();
                this.Close();
                ConsoleForm.ConsoleFormInstance.richTextBox1AppendText = "Selected Server: " + SelectedServer;
            }
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            SelectedServer = "Challenge";
            if ((this.mf == null) || this.mf.IsDisposed)
            {
                MainForm mf = new MainForm();
                mf.Show();
                this.Close();
                ConsoleForm.ConsoleFormInstance.richTextBox1AppendText = "Selected Server: " + SelectedServer;
            }
        }
    }
}
