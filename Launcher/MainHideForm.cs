﻿using System;
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
    public partial class MainHideForm : Form
    {
        public MainHideForm()
        {
            InitializeComponent();
        }

        private void MainHideForm_Load(object sender, EventArgs e)
        {
            this.Hide();

            ConsoleForm cf = new ConsoleForm();
            cf.Show();

            if (!Properties.Settings.Default.FirstRun)
            {
                //初回起動フォーム表示
                FirstRunForm frf = new FirstRunForm();
                frf.Show();
            }
            else
            {
                //メインフォーム表示
                SelectServer ss = new SelectServer();
                ss.Show();
            }
        }
    }
}
