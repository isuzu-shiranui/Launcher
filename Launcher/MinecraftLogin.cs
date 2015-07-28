using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Web.Script.Serialization;
using System.Diagnostics;

namespace Launcher
{
    class MinecraftLogin
    {
        private LauncherMessageBox msg = null;
        string ACCESS_TOKEN;
        string UserName;
        public string GetAccessToken()
        {
            return ACCESS_TOKEN;
        }
        public void ObtainAccessToken(string username, string password)
        {
            UserName = username;
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://authserver.mojang.com/authenticate");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = "{\"agent\":{\"name\":\"Minecraft\",\"version\":1},\"username\":\"" + username + "\",\"password\":\"" + password + "\",\"clientToken\":\"6c9d237d-8fbf-44ef-b46b-0b8a854bf391\"}";

                streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Close();

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    ACCESS_TOKEN = result;
                }
            }
        }

        public void StartMinecraft()
        {
            string directory = "C:\\Users\\Can\\AppData\\Roaming\\.minecraft";
            string java = @"C:\windows\system32\javaw.exe";
            string javaLocation = "C:\\Program Files\\Java\\jre7\\bin\\javaw.exe";
            string RAM = "1G";
            //string username = "namehere";
            string token = "--session token:" + ACCESS_TOKEN;
            string version = "1.6.2";
            string launch = "-Xmx" + RAM + " -Djava.library.path={0}\\versions\\1.6.2\\1.6.2-natives-7453523379463 -cp {0}\\libraries\\net\\sf\\jopt-simple\\jopt-simple\\4.5\\jopt-simple-4.5.jar;{0}\\libraries\\com\\paulscode\\codecjorbis\\20101023\\codecjorbis-20101023.jar;{0}\\libraries\\com\\paulscode\\codecwav\\20101023\\codecwav-20101023.jar;{0}\\libraries\\com\\paulscode\\libraryjavasound\\20101123\\libraryjavasound-20101123.jar;{0}\\libraries\\com\\paulscode\\librarylwjglopenal\\20100824\\librarylwjglopenal-20100824.jar;{0}\\libraries\\com\\paulscode\\soundsystem\\20120107\\soundsystem-20120107.jar;{0}\\libraries\\argo\\argo\\2.25_fixed\\argo-2.25_fixed.jar;{0}\\libraries\\org\\bouncycastle\\bcprov-jdk15on\\1.47\\bcprov-jdk15on-1.47.jar;{0}\\libraries\\com\\google\\guava\\guava\\14.0\\guava-14.0.jar;{0}\\libraries\\org\\apache\\commons\\commons-lang3\\3.1\\commons-lang3-3.1.jar;{0}\\libraries\\commons-io\\commons-io\\2.4\\commons-io-2.4.jar;{0}\\libraries\\net\\java\\jinput\\jinput\\2.0.5\\jinput-2.0.5.jar;{0}\\libraries\\net\\java\\jutils\\jutils\\1.0.0\\jutils-1.0.0.jar;{0}\\libraries\\com\\google\\code\\gson\\gson\\2.2.2\\gson-2.2.2.jar;{0}\\libraries\\org\\lwjgl\\lwjgl\\lwjgl\\2.9.0\\lwjgl-2.9.0.jar;{0}\\libraries\\org\\lwjgl\\lwjgl\\lwjgl_util\\2.9.0\\lwjgl_util-2.9.0.jar;{0}\\versions\\1.6.2\\1.6.2.jar net.minecraft.client.main.Main --username " + UserName + " " + token + " --version " + version + " --gameDir {0} --assetsDir {0}\\assets";
            //launch = String.Format(launch, directory);
            string text = launch;
            // WriteAllText creates a file, writes the specified string to the file, 
            // and then closes the file.
            Directory.SetCurrentDirectory(@"C:\windows\system32\");
            Process.Start("javaw.exe",launch);
        }

        private string GetJavaInstallationPath()
        {
            string environmentPath = @"G:\Program Files (x86)\Minecraft\runtime\jre-x64\1.8.0_25\bin\java.exe";
            if (!string.IsNullOrEmpty(environmentPath))
            {
                return environmentPath;
            }

            try
            {
                string javaKey = "SOFTWARE\\JavaSoft\\Java Runtime Environment\\";
                using (Microsoft.Win32.RegistryKey rk = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(javaKey))
                {
                    string currentVersion = rk.GetValue("CurrentVersion").ToString();
                    using (Microsoft.Win32.RegistryKey key = rk.OpenSubKey(currentVersion))
                    {
                        return key.GetValue("JavaHome").ToString();
                    }
                }
            }
            catch (NullReferenceException)
            {
                try
                {
                    string javaKey = "Wow6432Node\\SOFTWARE\\JavaSoft\\Java Runtime Environment\\";
                    using (Microsoft.Win32.RegistryKey rk = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(javaKey))
                    {
                        string currentVersion = rk.GetValue("CurrentVersion").ToString();
                        using (Microsoft.Win32.RegistryKey key = rk.OpenSubKey(currentVersion))
                        {
                            return key.GetValue("JavaHome").ToString();
                        }
                    }
                }
                catch (NullReferenceException)
                {
                    ShowMessagebox("NullReferenceException: Javaをインストールしてください。");
                    return null;
                }
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
