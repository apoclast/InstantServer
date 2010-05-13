using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace CF.InstantServer
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            startupPath_ = new FileInfo(Application.ExecutablePath).Directory;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            using(Stream stm = typeof(Program).Assembly.GetManifestResourceStream("CF.InstantServer.Icon.ico"))
            {
                mainIcon_ = new Icon(stm);
            }
            trayIcon_ = new NotifyIcon();
            trayIcon_.Icon = mainIcon_;
            trayIcon_.Click += new EventHandler(trayIcon__Click);
            trayIcon_.Visible = true;

            instantServerWindow_ = new InstantServer();
            Application.Run(instantServerWindow_);
            //instantServerWindow_.ShowDialog();
            trayIcon_.Visible = false;

        }

        static void trayIcon__Click(object sender, EventArgs e)
        {
            if(instantServerWindow_.Visible)
                instantServerWindow_.Hide();
            else
                instantServerWindow_.Show();
        }
        internal static Icon mainIcon_;
        private static NotifyIcon trayIcon_;
        private static InstantServer instantServerWindow_;
        private static DirectoryInfo startupPath_;
        public static DirectoryInfo StartupPath
        {
            get { return startupPath_; }
        }
    }

}
