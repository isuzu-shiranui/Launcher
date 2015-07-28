using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Compression;
using Ionic.Zip;
using Ionic.Zlib;

namespace Launcher
{
    public partial class ClientFileDownload
    {
        private LauncherMessageBox msg = null;

        ZipExtract zipExtarct = new ZipExtract();

        static string fileName;

        //WebClientフィールド
        WebClient downloadClient = null;

        public void clientFileDownload()
        {
            //ダウンロードしたファイルの保存先
            fileName = Properties.Settings.Default.InstallFolder + @"\temp\ModPack.zip";
            //ダウンロード基のURL
            Uri u = new Uri("http://kurenai-mc.ddo.jp/frontia/config.zip");

            //WebClientの作成
            if (downloadClient == null)
            {
                downloadClient = new System.Net.WebClient();
                //イベントハンドラの作成
                downloadClient.DownloadProgressChanged += new System.Net.DownloadProgressChangedEventHandler(downloadClient_DownloadProgressChanged);
                downloadClient.DownloadFileCompleted += new System.ComponentModel.AsyncCompletedEventHandler(downloadClient_DownloadFileCompleted);
            }
            MainForm.MainFormInstance.Label6Text = "ClientFile Downloading";
            //非同期ダウンロードを開始する
            downloadClient.DownloadFileAsync(u, fileName);

            
        }

        private void downloadClient_DownloadProgressChanged(object sender, System.Net.DownloadProgressChangedEventArgs e)
        {
            MainForm.MainFormInstance.Label4Text = e.ProgressPercentage.ToString() + "%";
            MainForm.MainFormInstance.progressBer1MaxValue = (int)e.TotalBytesToReceive;
            MainForm.MainFormInstance.progressBarValue = (int)e.BytesReceived;
            ConsoleForm.ConsoleFormInstance.richTextBox1AppendText = "Downloading: " + (int)e.BytesReceived + "/" + (int)e.TotalBytesToReceive + " (" + e.ProgressPercentage.ToString() + "%)";
        }

        private void downloadClient_DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                ShowMessagebox("エラー:" + e.Error.Message);
                MainForm.MainFormInstance.Label6Text = "エラー";
                ConsoleForm.ConsoleFormInstance.richTextBox1AppendText = "エラー:" + e.Error.Message;
            }
            else
            {
                ConsoleForm.ConsoleFormInstance.richTextBox1AppendText = "Download Completed";
                zipExtarct.Extract(fileName, Properties.Settings.Default.InstallFolder + @"\temp\ModPack");
            }
        }

        private void ShowMessagebox(string text)
        {
            if ((this.msg == null) || this.msg.IsDisposed)
            {
                msg = new LauncherMessageBox();
                msg.Show();
                msg.Label1Text = text;
            }
            else
            {
                msg.Close();
                msg = new LauncherMessageBox();
                msg.Show();
                msg.Label1Text = text;
            }
        }
    }
}
