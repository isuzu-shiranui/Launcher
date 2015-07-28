using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Diagnostics;
using Launcher.Properties;
using System.Windows.Forms;


namespace Launcher
{
    public partial class ForgeInstall
    {
        public bool DownloadFileCompleted = false;

        private LauncherMessageBox msg = null;

        //WebClientフィールド
        WebClient downloadClient = null;

        public void install()
        {
            string ForgeVersion = Properties.Settings.Default.UtopiaForgeVersion;

            //ダウンロードしたファイルの保存先
            string fileName = Properties.Settings.Default.InstallFolder + @"\temp\forge.exe";
            //ダウンロード基のURL
            Uri u = new Uri("http://files.minecraftforge.net/maven/net/minecraftforge/forge/" + ForgeVersion + "/forge-" + ForgeVersion + "-installer-win.exe");

            //WebClientの作成
            if (downloadClient == null)
            {
                downloadClient = new System.Net.WebClient();
                //イベントハンドラの作成
                downloadClient.DownloadProgressChanged += new System.Net.DownloadProgressChangedEventHandler(downloadClient_DownloadProgressChanged);
                downloadClient.DownloadFileCompleted += new System.ComponentModel.AsyncCompletedEventHandler(downloadClient_DownloadFileCompleted);
            }

            MainForm.MainFormInstance.Label6Text = "Forgeダウンロード中 . . .";
            ConsoleForm.ConsoleFormInstance.richTextBox1AppendText = "Forgeダウンロード中 . . .";

            //同期ダウンロードを開始する
            try
            {
                downloadClient.DownloadFile(u, fileName);

                MainForm.MainFormInstance.progressBarValue = 100;
                MainForm.MainFormInstance.Label4Text = "100%";
                MainForm.MainFormInstance.Label6Text = "Forgeダウンロード完了。Frogeインストーラ起動までしばらくかかります。";
                ConsoleForm.ConsoleFormInstance.richTextBox1AppendText = "Forge Download Completed";

                ShowMessagebox("Forgeをインストールして下さい。\nインストールしてある場合はとばしてかまいません。");


                Process p = Process.Start(Properties.Settings.Default.InstallFolder + @"\temp\forge.exe");
                ConsoleForm.ConsoleFormInstance.richTextBox1AppendText = "Process Start:" + Properties.Settings.Default.InstallFolder + @"\temp\forge.exe";
                p.WaitForExit();


            }
            catch (WebException e)
            {
                ShowMessagebox(e.Message);    
            }
            
        }

        private void downloadClient_DownloadProgressChanged(object sender, System.Net.DownloadProgressChangedEventArgs e)
        {
            MainForm.MainFormInstance.Label4Text = e.ProgressPercentage.ToString() + "%";
            MainForm.MainFormInstance.progressBer1MaxValue = (int)e.TotalBytesToReceive;
            MainForm.MainFormInstance.progressBarValue = (int)e.BytesReceived;
            ConsoleForm.ConsoleFormInstance.richTextBox1AppendText = "Downloading: " + (int)e.BytesReceived + "/" + (int)e.TotalBytesToReceive + " (" + e.ProgressPercentage.ToString() + "%)";
        }

        private void downloadClient_DownloadFileCompleted(object sender,System.ComponentModel.AsyncCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                ShowMessagebox("エラー:" + e.Error.Message);
                MainForm.MainFormInstance.Label6Text = "エラー";
                ConsoleForm.ConsoleFormInstance.richTextBox1AppendText = "エラー:" + e.Error.Message;
            }
        }

        public static void DeleteFile(string stFilePath)
        {
            System.IO.FileInfo cFileInfo = new System.IO.FileInfo(stFilePath);

            // ファイルが存在しているか判断する
            if (cFileInfo.Exists)
            {
                // 読み取り専用属性がある場合は、読み取り専用属性を解除する
                if ((cFileInfo.Attributes & System.IO.FileAttributes.ReadOnly) == System.IO.FileAttributes.ReadOnly)
                {
                    cFileInfo.Attributes = System.IO.FileAttributes.Normal;
                }

                // ファイルを削除する
                cFileInfo.Delete();
            }
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
    }
}
