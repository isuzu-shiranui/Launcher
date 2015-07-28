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
    public partial class ConsoleForm : Form
    {
        public ConsoleForm()
        {
            InitializeComponent();
        }

        private void ConsoleForm_Load(object sender, EventArgs e)
        {
            pictureBox1.MouseDown += new MouseEventHandler(ConsoleForm_MouseDown);//イベントハンドラ作成
            pictureBox1.MouseMove += new MouseEventHandler(ConsoleForm_MouseMove);//イベントハンドラ作成

            ConsoleForm.ConsoleFormInstance = this;

            ConsleWrite();
        }

        private Point mousePoint;//マウスポイント

        //フォームを動かす
        private void ConsoleForm_MouseDown(object sender, MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
            {
                mousePoint = new Point(e.X, e.Y);//位置を記憶する
            }
        }

        //フォームを動かす
        private void ConsoleForm_MouseMove(object sender, MouseEventArgs e)
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
            this.Close();
        }

        private void ConsleWrite()
        {
            System.OperatingSystem os = System.Environment.OSVersion;

            richTextBox1.AppendText("Application Start\n");
            if (System.Environment.Is64BitOperatingSystem) { richTextBox1.AppendText("OS:64bit\n"); }
            else { richTextBox1.AppendText("OS:32bit\n"); }

            string version = os.Version.Major + "." + os.Version.Minor;
            if (os.Platform == PlatformID.Win32NT)
            {
                if (version == "6.0") { richTextBox1.AppendText("OS:Windows Vista\n"); }
                else if (version == "6.1") { richTextBox1.AppendText("OS:Windows 7\n"); }
                else if (version == "6.2") { richTextBox1.AppendText("OS:Windows 8\n"); }
                else if (version == "6.3") { richTextBox1.AppendText("OS:Windows 8.1\n"); }
                else if (version == "6.4") { richTextBox1.AppendText("OS:Windows 10\n"); }
                else { richTextBox1.AppendText("OS:Unknown\n"); }
            }

            richTextBox1.AppendText("Launcher Version:" + Properties.Settings.Default.ClientVersion + "\n");
            richTextBox1.AppendText("Launcher Install Dir:" + Properties.Settings.Default.InstallFolder + "\n");
        }

        //ConsoleFormオブジェクトを保持するためのフィールド
        public static ConsoleForm _ConsoleFormInstance;

        //ConsoleFormオブジェクトを取得、設定するためのプロパティ
        public static ConsoleForm ConsoleFormInstance
        {
            get
            {
                return _ConsoleFormInstance;
            }
            set
            {
                _ConsoleFormInstance = value;
            }
        }

        public string richTextBox1AppendText
        {
            get
            {
                return richTextBox1.Text;
            }
            set
            {
                richTextBox1.AppendText(value + "\n");
            }
        }
    }
}
