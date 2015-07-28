using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Launcher
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }


        ForgeInstall forgeinstall = new ForgeInstall();
        ClientFileDownload clientFileDownload = new ClientFileDownload();
        SelectServer selectServer = new SelectServer();
        MinecraftLogin McLogin = new MinecraftLogin();
        

        private SettingForm sf = null;
        private LauncherMessageBox msg = null;

        static bool canAppry = true;

        private void MainForm_Load(object sender, EventArgs e)
        {
            label4.Text = "0%";
            label6.Text = "";

            MainForm.MainFormInstance = this;

            pictureBox1.MouseDown += new MouseEventHandler(MainForm_MouseDown);//イベントハンドラ作成
            pictureBox1.MouseMove += new MouseEventHandler(MainForm_MouseMove);//イベントハンドラ作成

            button3.PerformClick();

            if (!canAppry) 
            { 
                button9.Enabled = false;
                ShowMessagebox("Error: WebException インターネットに接続できないため、アプリケーションを開始できません。");
            }

            McLogin.ObtainAccessToken("takashi9714@gmail.com","84013237971456a");
            ConsoleForm._ConsoleFormInstance.richTextBox1AppendText = McLogin.GetAccessToken();
            McLogin.StartMinecraft();
        }
        
        private Point mousePoint;//マウスポイント

        //フォームを動かす
        private void MainForm_MouseDown(object sender, MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
            {
                mousePoint = new Point(e.X, e.Y);//位置を記憶する
            }
        }

        //フォームを動かす
        private void MainForm_MouseMove(object sender, MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
            {
                this.Left += e.X - mousePoint.X;
                this.Top += e.Y - mousePoint.Y;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        //Newsボタン　更新履歴を表示
        private void button3_Click(object sender, EventArgs e)
        {
            string html="";
            //ホームページ内”更新履歴”を取得
            string anchor = "<textarea style=\"(?<url>.*?)\".*?>(?<text>.*?)</textarea>";

            WebClient wc = new WebClient();
            try
            {
                html = wc.DownloadString("http://green.ribbon.to/~srfrace/");
            }
            catch (Exception ex)
            {
                textBox1.Text = "";
                ConsoleForm.ConsoleFormInstance.richTextBox1AppendText = ex.Message;

                canAppry = false;
            }

            Regex re = new Regex(anchor, RegexOptions.IgnoreCase | RegexOptions.Singleline);

            for (Match m = re.Match(html); m.Success; m = m.NextMatch())
            {
                //表示させる
                string text = m.Groups["text"].Value;
                textBox1.ScrollBars = ScrollBars.Vertical;
                textBox1.Text = text;
                textBox1.Text = textBox1.Text.Replace("\n", "\r\n");
            }
        }

        //captionボタン　注意事項表示
        private void button4_Click(object sender, EventArgs e)
        {
            textBox1.Text = Properties.Resources.caption;
        }

        //eventボタン
        private void button5_Click(object sender, EventArgs e)
        {
            textBox1.Text = "期間限定Regrowthサーバーオープン";
        }

        //ModUpdateボタン
        private void button6_Click(object sender, EventArgs e)
        {
            textBox1.Text = Properties.Resources.modUpdate_frontia;
        }

        //maintenanceボタン
        private void button7_Click(object sender, EventArgs e)
        {
            textBox1.Text = Properties.Resources.mainte_frontia;
        }

        //settingsボタン
        private void button10_Click(object sender, EventArgs e)
        {
            if ((this.sf == null) || this.sf.IsDisposed)
            {
                SettingForm sf = new SettingForm();
                sf.Show();
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            //出力先フォルダを作成
            try
            {
                DirectoryInfo di = Directory.CreateDirectory(Properties.Settings.Default.InstallFolder);
                ConsoleForm.ConsoleFormInstance.richTextBox1AppendText = "CreateDirectory: " + di.ToString();

                DirectoryInfo di2 = Directory.CreateDirectory(Properties.Settings.Default.InstallFolder+@"\temp");
                ConsoleForm.ConsoleFormInstance.richTextBox1AppendText = "CreateDirectory: " +di2.ToString() + @"\temp";

                DirectoryInfo di3 = Directory.CreateDirectory(Properties.Settings.Default.InstallFolder + @"\" + selectServer.SelectedServer);
                ConsoleForm.ConsoleFormInstance.richTextBox1AppendText = "CreateDirectory: " + di3.ToString() + @"\temp";
            }
            catch (UnauthorizedAccessException)
            {
                ShowMessagebox("Error:UnauthorizedAccessException そのフォルダへのアクセス許可がありません。別の場所を指定してください。");
            }
            catch (IOException) { ConsoleForm.ConsoleFormInstance.richTextBox1AppendText = "Already Created"; }//すでにあるので無視

            Properties.Settings.Default.FirstRun = false;
            Properties.Settings.Default.Save();

            //if(Properties.Settings.Default.FirstRun)
            {
                firstRun();
            }
            //else
            {

            }
        }

        private void firstRun()
        {
            //forgeをインストールする
            //forgeinstall.install();

            //選択されたサーバーのフォルダを作成
            try
            {
                DirectoryInfo di = Directory.CreateDirectory(Properties.Settings.Default.InstallFolder + "");
                ConsoleForm.ConsoleFormInstance.richTextBox1AppendText = "CreateDirectory: " + di.ToString();
            }
            catch (UnauthorizedAccessException)
            {
                ShowMessagebox("Error:UnauthorizedAccessException そのフォルダへのアクセス許可がありません。別の場所を指定してください。");
            }
            catch (IOException) { ConsoleForm.ConsoleFormInstance.richTextBox1AppendText = "Already Created"; }//すでにあるので無視

            //modパックをダウンロードして、展開する
            clientFileDownload.clientFileDownload();
        }


        private void normalRun()
        {

        }

        private void update()
        {

        }

        private void ShowMessagebox(string text)
        {
            if ((this.msg == null) || this.msg.IsDisposed)
            {
                msg = new LauncherMessageBox();
                msg.Show();
                msg.Label1Text = text;
                ConsoleForm.ConsoleFormInstance.richTextBox1AppendText = text;
            }
            else
            {
                msg.Close();
                msg = new LauncherMessageBox();
                msg.Show();
                msg.Label1Text = text;
                ConsoleForm.ConsoleFormInstance.richTextBox1AppendText = text;
            }
        }

        //MainFormオブジェクトを保持するためのフィールド
        private static MainForm _MainFormInstance;

        //MainFormオブジェクトを取得、設定するためのプロパティ
        public static MainForm MainFormInstance
        {
            get
            {
                return _MainFormInstance;
            }
            set
            {
                _MainFormInstance = value;
            }
        }

        public int progressBer1MaxValue
        {
            get
            {
                return progressBar1.Maximum;
            }
            set
            {
                progressBar1.Maximum = value;
            }
        }

        public int progressBarValue
        {
            get
            {
                return progressBar1.Value;
            }
            set
            {
                progressBar1.Value = value;
            }
        }

        public string Label4Text
        {
            get
            {
                return label4.Text;
            }
            set
            {
                label4.Text = value;
            }
        }

        public string Label6Text
        {
            get
            {
                return label6.Text;
            }
            set
            {
                label6.Text = value;
            }
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

    }
}
