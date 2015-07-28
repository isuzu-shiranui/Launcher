using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Launcher
{
    public partial class FirstRunForm : Form
    {

        static string FilePath = Properties.Settings.Default.InstallFolder;        //出力先フォルダ
        static string ClientFilePath = Properties.Settings.Default.ClientModFolder;//クライアントmodフォルダ

        ConsoleForm console = new ConsoleForm();
        

        public FirstRunForm()
        {
            InitializeComponent();
        }

        private void FirstRunForm_Load(object sender, EventArgs e)
        {
            pictureBox1.MouseDown += new MouseEventHandler(FirstRunForm_MouseDown);//イベントハンドラ作成
            pictureBox1.MouseMove += new MouseEventHandler(FirstRunForm_MouseMove);//イベントハンドラ作成

            textBox1.Text = FilePath;
        }

        private Point mousePoint;//マウスポイント

        //フォームを動かす
        private void FirstRunForm_MouseDown(object sender, MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
            {
                mousePoint = new Point(e.X, e.Y);//位置を記憶する
            }
        }

        //フォームを動かす
        private void FirstRunForm_MouseMove(object sender, MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
            {
                this.Left += e.X - mousePoint.X;
                this.Top += e.Y - mousePoint.Y;
            }
        }

        //閉じる
        private void button1_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.FirstRun = true;
            Properties.Settings.Default.Save();

            this.Close();
        }

        //最小化
        private void button2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        //出力先フォルダ指定       
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

        private void button4_Click(object sender, EventArgs e)
        {
            //出力先フォルダを作成
            try
            {
                DirectoryInfo di = Directory.CreateDirectory(textBox1.Text);
                ConsoleForm.ConsoleFormInstance.richTextBox1AppendText = "CreateDirectory: " + textBox1.Text;

                DirectoryInfo di2 = Directory.CreateDirectory(textBox1.Text + @"\temp");
                ConsoleForm.ConsoleFormInstance.richTextBox1AppendText = "CreateDirectory: " + textBox1.Text + @"\temp";

            }
            catch (UnauthorizedAccessException)
            {
                LauncherMessageBox.MessageBoxInstance.Label1Text ="そのフォルダへのアクセス許可がありません。別の場所を指定してください。";
                ConsoleForm.ConsoleFormInstance.richTextBox1AppendText = "Error:UnauthorizedAccessException そのフォルダへのアクセス許可がありません。別の場所を指定してください。";
            }
            catch (IOException) { ConsoleForm.ConsoleFormInstance.richTextBox1AppendText = "Already Created"; }//すでにあるので無視

            Properties.Settings.Default.FirstRun = false;
            Properties.Settings.Default.Save();

            this.Close();

            //メインフォーム表示
            SelectServer ss = new SelectServer();
            ss.Show();
            ConsoleForm.ConsoleFormInstance.richTextBox1AppendText = "Open ServerSelectForm";
        }
    }
}
