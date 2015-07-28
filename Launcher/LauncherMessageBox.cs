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
    public partial class LauncherMessageBox : Form
    {
        public LauncherMessageBox()
        {
            InitializeComponent();
        }

        //LauncherMessageBoxオブジェクトを保持するためのフィールド
        private static LauncherMessageBox _MessageBoxInstance;

        //LauncherMessageBoxオブジェクトを取得、設定するためのプロパティ
        public static LauncherMessageBox MessageBoxInstance
        {
            get
            {
                return _MessageBoxInstance; 
            }
            set
            {
                _MessageBoxInstance = value;
            }
        }

        public string Label1Text
        {
            get
            {
                return label1.Text;
            }
            set
            {
                label1.Text = value;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
