using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Ionic.Zip;
using Ionic.Zlib;

namespace Launcher
{
    public partial class ZipExtract
    {
        public void Extract(string FilePath, string ExtractPath)
        {
            MemoryStream ms = new MemoryStream();
            StreamWriter sw = new StreamWriter(ms);

            ReadOptions options = new ReadOptions();
            options.StatusMessageWriter = sw;
            ZipFile zf = ZipFile.Read(FilePath, options);

            zf.ExtractAll(ExtractPath);

            ms.Seek(0, 0);
            StreamReader sr = new StreamReader(ms);
            string msg = sr.ReadToEnd();
            ConsoleForm._ConsoleFormInstance.richTextBox1AppendText += msg;

            ConsoleForm._ConsoleFormInstance.richTextBox1AppendText += "Extract Completed";
        }
    }
}
