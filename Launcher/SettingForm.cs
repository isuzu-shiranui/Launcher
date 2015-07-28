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
    public partial class SettingForm : Form
    {

        static string FilePath = Properties.Settings.Default.InstallFolder;//出力先フォルダ


        public SettingForm()
        {
            InitializeComponent();
        }

        private void SettingForm_Load(object sender, EventArgs e)
        {
            pictureBox1.MouseDown += new MouseEventHandler(SettingForm_MouseDown);//イベントハンドラ作成
            pictureBox1.MouseMove += new MouseEventHandler(SettingForm_MouseMove);//イベントハンドラ作成

            textBox1.Text = FilePath;
            ConsoleForm.ConsoleFormInstance.richTextBox1AppendText ="Open SettingForm";
           
        }

        private Point mousePoint;//マウスポイント

        //フォームを動かす
        private void SettingForm_MouseDown(object sender, MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
            {
                mousePoint = new Point(e.X, e.Y);//位置を記憶する
            }
        }

        //フォームを動かす
        private void SettingForm_MouseMove(object sender, MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
            {
                this.Left += e.X - mousePoint.X;
                this.Top += e.Y - mousePoint.Y;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.Save();
            ConsoleForm._ConsoleFormInstance.richTextBox1AppendText = "Setting Saved\nClose SettingForm";
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.Description = "フォルダを指定してください。";
            fbd.RootFolder = Environment.SpecialFolder.Desktop;
            if (fbd.ShowDialog(this) == DialogResult.OK)
            {
                FilePath = fbd.SelectedPath;
                textBox1.Text = FilePath + "FeliLauncher";

                Properties.Settings.Default.InstallFolder = textBox1.Text;
                Properties.Settings.Default.Save();
            }
        }
    }
}
