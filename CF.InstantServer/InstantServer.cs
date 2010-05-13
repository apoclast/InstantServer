using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Net;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
namespace CF.InstantServer
{
    internal partial class InstantServer : Form
    {
        private static object locker_ = new object();
        private string mySQLEXEPathDefault_;
        private string mySQLINIPathDefault_;
        private string mySQLDataPathDefault_;
        private string apacheEXEPathDefault_;
        private string apacheConfigPathDefault_;
        private string apacheInitPathDefault_;
        private string phpDLLPathDefault_;
        private string phpINIPathDefault_;
        private string phpExtensionPathDefault_;
        private string phpPathDefault_;
        private string postgreSQLBinPathDefault_;
        private string postgreSQLDataPathDefault_;
        SortedList<string, TextBox> pathSettingPairs_ = new SortedList<string, TextBox>();


        public InstantServer()
        {
            InitializeComponent();

            apacheEXEPathBox_.Text = apacheEXEPathDefault_ = String.Format(@"{0}\Apps\Apache\bin\httpd.exe", Program.StartupPath);
            apacheConfigPathBox_.Text = apacheConfigPathDefault_ = String.Format(@"{0}\Apps\Apache\conf\httpd.conf", Program.StartupPath);
            apacheInitPathBox_.Text = apacheInitPathDefault_ = String.Format(@"{0}\Apps\Apache\.", Program.StartupPath);

            mySQLEXEPathBox_.Text = mySQLEXEPathDefault_ = String.Format(@"{0}\Apps\mysql\bin\mysqld.exe", Program.StartupPath);
            mySQLINIPathBox_.Text = mySQLINIPathDefault_ = String.Format(@"{0}\Apps\mysql\my.ini", Program.StartupPath);
            mySQLDataPathBox_.Text = mySQLDataPathDefault_ = String.Format(@"{0}\Apps\mysql\data", Program.StartupPath);

            phpDLLPathBox_.Text = phpDLLPathDefault_ = String.Format(@"{0}\Apps\php\php5apache2_2.dll", Program.StartupPath);
            phpINIPathBox_.Text = phpINIPathDefault_ = String.Format(@"{0}\Apps\php\php.ini", Program.StartupPath);
            phpExtensionPathBox_.Text = phpExtensionPathDefault_ = String.Format(@"{0}\Apps\php\ext", Program.StartupPath);
            phpPathBox_.Text = phpPathDefault_ = String.Format(@"{0}\Apps\php", Program.StartupPath);

            postgreSQLDataPathBox_.Text = postgreSQLDataPathDefault_ = String.Format(@"{0}\Apps\postgresql\data", Program.StartupPath);
            postgreSQLBinPathBox_.Text = postgreSQLBinPathDefault_ = String.Format(@"{0}\Apps\postgresql\bin", Program.StartupPath);
            //mySQLINIPathResetLabel_.Text = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";

            Icon = Program.mainIcon_;
            pathSettingPairs_.Add("Apache.EXEPath", apacheEXEPathBox_);
            pathSettingPairs_.Add("Apache.ConfigPath", apacheEXEPathBox_);
            pathSettingPairs_.Add("Apache.InitPath", apacheInitPathBox_);
            pathSettingPairs_.Add("MySQL.EXEPath", mySQLEXEPathBox_);
            pathSettingPairs_.Add("MySQL.INIPath", mySQLINIPathBox_);
            pathSettingPairs_.Add("MySQL.DataPath", mySQLDataPathBox_);
            pathSettingPairs_.Add("PHP.DLLPath", phpDLLPathBox_);
            pathSettingPairs_.Add("PHP.INIPath", phpINIPathBox_);
            pathSettingPairs_.Add("PHP.ExtensionPath", phpExtensionPathBox_);
            pathSettingPairs_.Add("PHP.Path", phpPathBox_);
            pathSettingPairs_.Add("PostgreSQL.DataPath", postgreSQLDataPathBox_);
            pathSettingPairs_.Add("PostgreSQL.BinPath", postgreSQLBinPathBox_);


            if (!Directory.Exists(String.Format(@"{0}\Config", Program.StartupPath)))
                Directory.CreateDirectory(String.Format(@"{0}\Config", Program.StartupPath));
            if (!Directory.Exists(String.Format(@"{0}\Apps", Program.StartupPath)))
                Directory.CreateDirectory(String.Format(@"{0}\Apps", Program.StartupPath));
            loadSetting();
        }

        private void loadSetting()
        {
            BinaryFormatter formater = new BinaryFormatter();
            if (File.Exists(String.Format(@"{0}\Config\sites.bin", Program.StartupPath)))
            {
                using (Stream stm = new FileStream(String.Format(@"{0}\Config\sites.bin", Program.StartupPath), FileMode.Open))
                {
                    object sites = null;
                    try
                    {
                        sites = formater.Deserialize(stm);
                    }
                    catch { }
                    if (sites is VirtualSite[])
                    {
                        apacheVirtualSiteEditor_.SetVirtualSites((VirtualSite[])sites);
                    }
                }
            }
            if (File.Exists(String.Format(@"{0}\Config\path_setting.bin", Program.StartupPath)))
            {
                using (Stream stm = new FileStream(String.Format(@"{0}\Config\path_setting.bin", Program.StartupPath), FileMode.Open))
                {
                    object setting = null;
                    try
                    {
                        setting = formater.Deserialize(stm);
                    }
                    catch { }
                    if (setting is SortedList<string, string>)
                    {
                        SortedList<string, string> setting_v = (SortedList<string, string>)setting;
                        foreach (KeyValuePair<string, TextBox> p in pathSettingPairs_)
                            if (setting_v.ContainsKey(p.Key) && (File.Exists(setting_v[p.Key]) || Directory.Exists(setting_v[p.Key])))
                                p.Value.Text = setting_v[p.Key];
                    }
                }
            }
        }
        private void saveSetting()
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (Stream stm = new FileStream(String.Format(@"{0}\Config\sites.bin", Program.StartupPath), FileMode.Create))
            {
                formatter.Serialize(stm, apacheVirtualSiteEditor_.GetVirtualSites());
            }


            using (Stream stm = new FileStream(String.Format(@"{0}\Config\path_setting.bin", Program.StartupPath), FileMode.Create))
            {
                object setting = null;
                try
                {
                    setting = formatter.Deserialize(stm);
                }
                catch { }
                if (setting is SortedList<string, string>)
                {
                    SortedList<string, string> setting_v = (SortedList<string, string>)setting;
                    foreach (KeyValuePair<string, TextBox> p in pathSettingPairs_)
                    {
                        if (setting_v.ContainsKey(p.Key))
                            setting_v[p.Key] = p.Value.Text;
                        else
                            setting_v.Add(p.Key, p.Value.Text);
                    }
                }
            }
        }

        private void appendLog(string txt)
        {
            if (String.IsNullOrEmpty(txt))
                return;
            outputBox_.AppendText("[");
            outputBox_.AppendText(DateTime.Now.ToString("dd MMM HH:mm:ssffff"));
            outputBox_.AppendText("]");
            outputBox_.AppendText(txt);
            outputBox_.AppendText(Environment.NewLine);
        }
        private void outputDoClearButton_Click(object sender, EventArgs e)
        {
            outputBox_.Text = "";
        }
        #region Apache
        private Process apacheProcess_;
        private void apacheDoStartButton_Click(object sender, EventArgs e)
        {
            apacheStart();
        }
        private void apacheProcess_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            appendLog(apacheProcess_.StandardOutput.ReadToEnd());
        }
        private void apacheProcess_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            appendLog(apacheProcess_.StandardError.ReadToEnd());
        }
        private void apacheDoStopButton_Click(object sender, EventArgs e)
        {
            //apacheProcess_.CloseMainWindow();
            ProcessUtility.KillTree(apacheProcess_.Id);
        }
        private void apacheServerNameBox_Enter(object sender, EventArgs e)
        {
            apacheServerNameGroup_.ForeColor = SystemColors.ControlText;
        }
        private void apacheProcess_Exit(object state)
        {
            Process process = state as Process;
            process.WaitForExit();
            Invoke(new SetFormCB(
                () =>
                {
                    appendLog(process.StandardError.ReadToEnd());
                    appendLog(process.StandardOutput.ReadToEnd());
                    appendLog("Apache Stopped");
                    apacheDoStartButton_.Enabled = true;
                    apacheDoStopButton_.Enabled = false;
                }));
        }
        private void apacheVirtualSiteEditor_Enter(object sender, EventArgs e)
        {
            apacheVirtualSiteGroup_.ForeColor = SystemColors.ControlText;
        }
        #region Apache Path
        private void apacheEXEPathSelectLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            selectApacheEXE();
        }
        private void apacheEXEPathBox_Click(object sender, EventArgs e)
        {
            selectApacheEXE();
        }
        private void apacheConfigPathSelectLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            selectApachConfig();
        }
        private void apacheConfigPathBox_Click(object sender, EventArgs e)
        {
            selectApachConfig();
        }
        private void apacheInitPathSelectLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            selectApachInitFolder();
        }
        private void apacheEXEPathResetLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            apacheEXEPathBox_.Text = apacheEXEPathDefault_;
        }
        private void apacheConfigPathResetLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            apacheConfigPathBox_.Text = apacheConfigPathDefault_;
        }
        private void apacheInitPathResetLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            apacheInitPathBox_.Text = apacheInitPathDefault_;
        }
        private void apacheInitPathBox_Click(object sender, EventArgs e)
        {
            selectApachInitFolder();
        }
        private void selectApacheEXE()
        {
            OpenFileDialog opener = new OpenFileDialog();
            opener.CheckFileExists = true;
            opener.Multiselect = false;
            opener.Filter = "Apache Execution|httpd.exe";
            opener.ValidateNames = true;
            if (opener.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                apacheEXEPathBox_.Text = opener.FileName;
            }
        }
        private void selectApachConfig()
        {
            OpenFileDialog opener = new OpenFileDialog();
            opener.CheckFileExists = true;
            opener.Multiselect = false;
            opener.Filter = "Apache Option File|*.conf|Any File|*.*";
            opener.ValidateNames = true;
            if (opener.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                apacheConfigPathBox_.Text = opener.FileName;
            }
        }
        private void selectApachInitFolder()
        {
            FolderBrowserDialog opener = new FolderBrowserDialog();
            opener.SelectedPath = apacheInitPathBox_.Text;
            opener.RootFolder = Environment.SpecialFolder.MyComputer;
            opener.ShowNewFolderButton = false;
            if (opener.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                apacheInitPathBox_.Text = opener.SelectedPath;
            }
        }
        #endregion
        private void apacheStart()
        {
            if (!validateApacheConfig())
                return;

            buildApacheConfigFile();

            StringBuilder param_builder = new StringBuilder();
            param_builder.AppendFormat(@" -w -f ""{0}\Config\apache.conf""", Program.StartupPath);
            param_builder.AppendFormat(@" -d ""{0}""", apacheInitPathBox_.Text);

            ProcessStartInfo start_info = new ProcessStartInfo();
            start_info.FileName = apacheEXEPathBox_.Text;
            start_info.Arguments = param_builder.ToString();
            start_info.UseShellExecute = false;
            start_info.CreateNoWindow = true;
            start_info.RedirectStandardError = true;
            start_info.RedirectStandardOutput = true;
            start_info.WindowStyle = ProcessWindowStyle.Hidden;
            if (phpEnableBox_.Checked)
            {
                start_info.EnvironmentVariables["PATH"] += ";" + phpPathBox_.Text;
            }
            lock (locker_)
            {
                apacheProcess_ = new Process();
                apacheProcess_.StartInfo = start_info;
                apacheProcess_.ErrorDataReceived += new DataReceivedEventHandler(apacheProcess_ErrorDataReceived);
                apacheProcess_.OutputDataReceived += new DataReceivedEventHandler(apacheProcess_OutputDataReceived);
                apacheProcess_.Start();
                ThreadPool.QueueUserWorkItem(apacheProcess_Exit, apacheProcess_);
                apacheDoStartButton_.Enabled = false;
                apacheDoStopButton_.Enabled = true;
                appendLog("Apache Started");
            }
        }
        private void apacheStop()
        {
            apacheDoStopButton_.Enabled = false;
            ThreadPool.QueueUserWorkItem(delegate(object state)
            {
                if (!apacheProcess_.HasExited)
                    ProcessUtility.KillTree(apacheProcess_.Id);
                apacheProcess_ = null;
            }, null);


        }
        private void buildApacheConfigFile()
        {
            StringBuilder bd = new StringBuilder();
            //bd.AppendLine(@"Listen 8080");
            bd.AppendLine(@"ServerAdmin webmaster@apoclast.org");
            bd.AppendFormat(@"ServerName {0}", apacheServerNameBox_.Text);
            bd.AppendLine();
            //bd.AppendLine(@"DocumentRoot ""D:/http/sitea""");
            //bd.AppendLine(@"<Directory ""D:/http/sitea"">");
            //bd.AppendLine(@"Options Indexes FollowSymLinks");
            //bd.AppendLine(@"AllowOverride None");
            //bd.AppendLine(@"Order allow,deny");
            //bd.AppendLine(@"Allow from all");
            //bd.AppendLine(@"</Directory>");
            apacheVirtualSiteEditor_.AppendConfigSegment(bd);
            bd.AppendFormat(@"Include ""{0}""", apacheConfigPathBox_.Text.Replace('\\', '/'));
            bd.AppendLine();
            if (phpEnableBox_.Checked)
            {
                appendPHPConfigSegment(bd);
            }
            File.WriteAllText(String.Format(@"{0}\Config\{1}", Program.StartupPath.FullName, "apache.conf"), bd.ToString());
        }
        private bool validateApacheConfig()
        {
            bool valid = true;
            if (!Regex.IsMatch(apacheServerNameBox_.Text, @"^(?:(?:[0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])\.){3}(?:[0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])$|^(?:(?:[a-zA-Z0-9]|[a-zA-Z0-9][a-zA-Z0-9\-]*[a-zA-Z0-9])\.)*(?:[A-Za-z]|[A-Za-z][A-Za-z0-9\-]*[A-Za-z0-9])\:(\d+)$", RegexOptions.Singleline))
            {
                apacheServerNameBox_.SelectionStart = 0;
                apacheServerNameBox_.SelectionLength = apacheServerNameBox_.Text.Length;
                apacheServerNameGroup_.ForeColor = Color.Red;
                valid = false;
            }
            if (apacheVirtualSiteEditor_.GetVirtualSites().Length == 0)
            {
                apacheVirtualSiteGroup_.ForeColor = Color.Red;
                valid = false;
            }
            if (!valid)
                tabControl_.SelectedTab = apachePage_;
            return valid;
        }
        #endregion
        #region MySQL
        private IPAddress mySQLIP_;
        private Process mySQLProcess_;
        private void mySQLDoStartButton_Click(object sender, EventArgs e)
        {
            mySQLStart();
        }
        private void mySQLProcess_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            appendLog(mySQLProcess_.StandardOutput.ReadToEnd());
        }
        private void mySQLProcess_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            appendLog(mySQLProcess_.StandardError.ReadToEnd());
        }//" MySQL";
        #region MySQL Path
        private void mySQLProcess_Exit(object state)
        {
            Process process = state as Process;
            process.WaitForExit();
            Invoke(new SetFormCB(delegate()
            {
                appendLog(process.StandardError.ReadToEnd());
                appendLog(process.StandardOutput.ReadToEnd());
                appendLog("MySQL Stopped\r\n");
                mySQLDoStartButton_.Enabled = true;
                mySQLDoStopButton_.Enabled = false;
            }));
        }
        private void mySQLEXEPathBox_Click(object sender, EventArgs e)
        {
            selectMySQLEXE();
        }
        private void mySQLINIPathBox_Click(object sender, EventArgs e)
        {
            selectMySQLINI();
        }
        private void mySQLDataPathBox_Click(object sender, EventArgs e)
        {
            selectMySQLDataPath();
        }
        private void mySQLINIPathSelectLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            selectMySQLINI();
        }
        private void mySQLEXEPathSelectLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            selectMySQLEXE();
        }
        private void mySQLDataPathSelectLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            selectMySQLDataPath();
        }
        private void mySQLEXEPathResetLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            mySQLEXEPathBox_.Text = mySQLEXEPathDefault_;
        }
        private void mySQLINIPathResetLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            mySQLINIPathBox_.Text = mySQLINIPathDefault_;
        }
        private void mySQLDataPathResetLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            mySQLDataPathBox_.Text = mySQLDataPathDefault_;
        }
        private void selectMySQLINI()
        {
            OpenFileDialog opener = new OpenFileDialog();
            opener.CheckFileExists = true;
            opener.Multiselect = false;
            opener.Filter = "MySQL Configuration File|*.ini|Any File|*.*";
            opener.ValidateNames = true;
            if (opener.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                mySQLINIPathBox_.Text = opener.FileName;
            }
        }
        private void selectMySQLEXE()
        {
            OpenFileDialog opener = new OpenFileDialog();
            opener.CheckFileExists = true;
            opener.Multiselect = false;
            opener.Filter = "MySQL Execution|mysqld.exe|MySQL Debug Execution|mysqld-debug.exe";
            opener.ValidateNames = true;
            if (opener.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                mySQLEXEPathBox_.Text = opener.FileName;
            }
        }
        private void selectMySQLDataPath()
        {
            FolderBrowserDialog opener = new FolderBrowserDialog();
            opener.SelectedPath = mySQLDataPathBox_.Text;
            opener.RootFolder = Environment.SpecialFolder.MyComputer;
            opener.ShowNewFolderButton = false;
            if (opener.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                mySQLDataPathBox_.Text = opener.SelectedPath;
            }
        }
        #endregion

        private void mySQLIPBox_TextChanged(object sender, EventArgs e)
        {
            mySQLIPBindBox_.ForeColor = SystemColors.WindowText;
        }
        private void mySQLIPBindBox_CheckedChanged(object sender, EventArgs e)
        {
            mySQLIPBox_.Enabled = mySQLIPBindBox_.Checked;
        }
        private void mySQLDoStopButton_Click(object sender, EventArgs e)
        {
            mySQLStop();
        }
        private void mySQLStart()
        {
            if (!validateMySQLConfig())
            {
                tabControl_.SelectedTab = mySQLPage_;
                return;
            }

            StringBuilder param_builder = new StringBuilder();
            param_builder.AppendFormat(@" --defaults-file=""{0}""", mySQLINIPathBox_.Text);
            if (mySQLIPBindBox_.Checked)
                param_builder.AppendFormat(@" --bind-address={0}", mySQLIP_);
            param_builder.AppendFormat(@" --port={0}", mySQLPortNumberBox_.Value);
            param_builder.AppendFormat(@" --basedir=""{0}""", new FileInfo(mySQLEXEPathBox_.Text).Directory.Parent.FullName);
            param_builder.AppendFormat(@" --console=TRUE");


            ProcessStartInfo start_info = new ProcessStartInfo();
            start_info.FileName = mySQLEXEPathBox_.Text;
            start_info.Arguments = param_builder.ToString();

            start_info.UseShellExecute = false;
            start_info.CreateNoWindow = true;
            start_info.RedirectStandardOutput = true;
            start_info.RedirectStandardError = true;
            start_info.WindowStyle = ProcessWindowStyle.Hidden;



            lock (locker_)
            {
                mySQLProcess_ = new Process();
                mySQLProcess_.ErrorDataReceived += new DataReceivedEventHandler(mySQLProcess_ErrorDataReceived);
                mySQLProcess_.OutputDataReceived += new DataReceivedEventHandler(mySQLProcess_OutputDataReceived);
                mySQLProcess_.StartInfo = start_info;
                mySQLProcess_.Start();
                ThreadPool.QueueUserWorkItem(mySQLProcess_Exit, mySQLProcess_);
                mySQLDoStartButton_.Enabled = false;
                mySQLDoStopButton_.Enabled = true;
                appendLog("MySQL Started");
            }
        }
        private void mySQLStop()
        {
            mySQLDoStopButton_.Enabled = false;

            ThreadPool.QueueUserWorkItem(delegate(object state)
            {
                if (!mySQLProcess_.HasExited)
                    ProcessUtility.KillTree(mySQLProcess_.Id);
                mySQLProcess_ = null;
            }, null);

        }
        private bool validateMySQLConfig()
        {
            if (mySQLIPBindBox_.Checked && !IPAddress.TryParse(mySQLIPBox_.Text, out mySQLIP_))
            {
                mySQLIPBindBox_.ForeColor = Color.Red;
                return false;
            }

            return true;
        }
        #endregion
        #region PHP
        private void appendPHPConfigSegment(StringBuilder bd)
        {
            bd.AppendFormat(@"LoadModule php5_module ""{0}""", phpDLLPathBox_.Text.Replace('\\', '/'));
            bd.AppendLine();
            bd.AppendLine(@"AddType application/x-httpd-php .php ");
            bd.AppendLine(@"AddType application/x-httpd-php .phtml ");
            bd.AppendFormat(@"PHPIniDir ""{0}/Config""", Program.StartupPath.FullName.Replace('\\', '/'));
            bd.AppendLine();

            bd.AppendFormat(@"<Directory ""{0}"">", phpPathBox_.Text.Replace('\\', '/'));
            bd.AppendLine();
            bd.AppendLine(@"Options Indexes FollowSymLinks MultiViews");
            bd.AppendLine(@"AllowOverride None");
            bd.AppendLine(@"</Directory>");

            using (StreamWriter writer = new StreamWriter(String.Format(@"{0}\Config\php.ini", Program.StartupPath.FullName), false))
            {
                writer.WriteLine(File.ReadAllText(phpINIPathBox_.Text));
                writer.WriteLine(@"extension_dir = ""{0}""", phpExtensionPathBox_.Text);
            }
        }
        #region PHP Path
        private void phpEnableBox_CheckedChanged(object sender, EventArgs e)
        {
            phpPathGroup_.Enabled = phpEnableBox_.Checked;
        }
        private void phpDLLPathResetLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            phpDLLPathBox_.Text = phpDLLPathDefault_;
        }
        private void phpINIPathResetLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            phpINIPathBox_.Text = phpINIPathDefault_;
        }
        private void phpExtensionPathResetLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            phpExtensionPathBox_.Text = phpExtensionPathDefault_;
        }
        private void phpPathResetLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            phpPathBox_.Text = phpPathDefault_;
        }
        private void phpDLLPathSelectLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            selectPHPDLL();
        }
        private void phpINIPathSelectLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            selectPHPINI();
        }
        private void phpExtensionPathSelectLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            selectPHPExtensionFolder();
        }
        private void phpPathSelectLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            selectPHPPathFolder();
        }
        private void phpDLLPathBox_Click(object sender, EventArgs e)
        {
            selectPHPDLL();
        }
        private void phpINIPathBox_Click(object sender, EventArgs e)
        {
            selectPHPINI();
        }
        private void phpExtensionPathBox_Click(object sender, EventArgs e)
        {
            selectPHPExtensionFolder();
        }
        private void phpPathBox_Click(object sender, EventArgs e)
        {
            selectPHPPathFolder();
        }
        private void selectPHPDLL()
        {
            OpenFileDialog opener = new OpenFileDialog();
            opener.CheckFileExists = true;
            opener.Multiselect = false;
            opener.Filter = "PHP Library|*.dll";
            opener.ValidateNames = true;
            if (opener.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                phpDLLPathBox_.Text = opener.FileName;
            }
        }
        private void selectPHPINI()
        {
            OpenFileDialog opener = new OpenFileDialog();
            opener.CheckFileExists = true;
            opener.Multiselect = false;
            opener.Filter = "PHP Configuration|php.ini";
            opener.ValidateNames = true;
            if (opener.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                phpINIPathBox_.Text = opener.FileName;
            }
        }
        private void selectPHPExtensionFolder()
        {
            FolderBrowserDialog opener = new FolderBrowserDialog();
            opener.SelectedPath = phpExtensionPathBox_.Text;
            opener.RootFolder = Environment.SpecialFolder.MyComputer;
            opener.ShowNewFolderButton = false;
            if (opener.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                phpExtensionPathBox_.Text = opener.SelectedPath;
            }
        }
        private void selectPHPPathFolder()
        {
            FolderBrowserDialog opener = new FolderBrowserDialog();
            opener.SelectedPath = phpPathBox_.Text;
            opener.RootFolder = Environment.SpecialFolder.MyComputer;
            opener.ShowNewFolderButton = false;
            if (opener.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                phpPathBox_.Text = opener.SelectedPath;
            }
        }
        #endregion
        #endregion
        #region PostgreSQL
        private Process postgreSQLProcess_;
        private List<IPAddress> postgreSQLIPList_ = new List<IPAddress>();
        private void postgreSQLDoStartButton_Click(object sender, EventArgs e)
        {
            postgreSQLStart();
        }
        private void postgreSQLProcess_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            appendLog(postgreSQLProcess_.StandardError.ReadToEnd());
        }
        private void postgreSQLProcess_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            appendLog(postgreSQLProcess_.StandardOutput.ReadToEnd());
        }
        private void postgreSQL_Exit(object state)
        {
            Process process = state as Process;
            process.WaitForExit();
            Invoke(new SetFormCB(delegate()
            {
                appendLog(process.StandardError.ReadToEnd());
                appendLog(process.StandardOutput.ReadToEnd());
                appendLog("PostgreSQL Stopped");
                postgreSQLDoStartButton_.Enabled = true;
                postgreSQLDoStopButton_.Enabled = false;
            }));
        }
        private void postgreSQLDoStopButton_Click(object sender, EventArgs e)
        {
            postgreSQLStop();
        }
        private void postgresqlIPBox_Enter(object sender, EventArgs e)
        {
            postgreSQLIPLabel_.ForeColor = SystemColors.WindowText;
        }
        #region PostgreSQL Path
        private void postgreSQLBinPathSelectLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            selectPostgreSQLBinPath();
        }
        private void postgreSQLBinPathBox_Click(object sender, EventArgs e)
        {
            selectPostgreSQLBinPath();
        }
        private void postgreSQLDataPathSelectLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            selectPostgreSQLDataPath();
        }
        private void postgreSQLDataPathBox_Click(object sender, EventArgs e)
        {
            selectPostgreSQLDataPath();
        }
        private void postgreSQLBinPathResetLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            postgreSQLBinPathBox_.Text = postgreSQLBinPathDefault_;
        }
        private void postgreSQLDataPathResetLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            postgreSQLDataPathBox_.Text = postgreSQLDataPathDefault_;
        }
        private void selectPostgreSQLBinPath()
        {
            FolderBrowserDialog opener = new FolderBrowserDialog();
            opener.SelectedPath = postgreSQLBinPathBox_.Text;
            opener.RootFolder = Environment.SpecialFolder.MyComputer;
            opener.ShowNewFolderButton = false;
            if (opener.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                postgreSQLBinPathBox_.Text = opener.SelectedPath;
            }
        }
        private void selectPostgreSQLDataPath()
        {
            FolderBrowserDialog opener = new FolderBrowserDialog();
            opener.SelectedPath = postgreSQLDataPathBox_.Text;
            opener.RootFolder = Environment.SpecialFolder.MyComputer;
            opener.ShowNewFolderButton = false;
            if (opener.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                postgreSQLDataPathBox_.Text = opener.SelectedPath;
            }
        }
        #endregion
        private void postgreSQLStart()
        {
            if (!validatePostgreSQLConfig())
            {
                tabControl_.SelectedTab = postgreSQLPage_;
                return;
            }
            buildPostgreSQLConfigFile();
            StringBuilder param_builder = new StringBuilder();
            param_builder.AppendFormat(@" -D ""{0}""", postgreSQLDataPathBox_.Text);
            param_builder.AppendFormat(@" --config_file=""{0}\Config\postgresql.conf""", Program.StartupPath);
            param_builder.AppendFormat(@" -p {0}", postgreSQLPortNumberBox_.Value);
            param_builder.Append(@" -h *");
            //param_builder.AppendFormat(@" -d ""{0}""", apacheInitPathBox_.Text);

            ProcessStartInfo start_info = new ProcessStartInfo();
            start_info.FileName = String.Format(@"{0}\postgres.exe", postgreSQLBinPathBox_.Text);
            start_info.Arguments = param_builder.ToString();
            start_info.UseShellExecute = false;
            start_info.CreateNoWindow = true;
            start_info.RedirectStandardError = true;
            start_info.RedirectStandardOutput = true;
            start_info.WindowStyle = ProcessWindowStyle.Hidden;

            lock (locker_)
            {

                postgreSQLProcess_ = new Process();
                postgreSQLProcess_.StartInfo = start_info;
                postgreSQLProcess_.OutputDataReceived += new DataReceivedEventHandler(postgreSQLProcess_OutputDataReceived);
                postgreSQLProcess_.ErrorDataReceived += new DataReceivedEventHandler(postgreSQLProcess_ErrorDataReceived);
                postgreSQLProcess_.Start();
                ThreadPool.QueueUserWorkItem(postgreSQL_Exit, postgreSQLProcess_);
                postgreSQLDoStartButton_.Enabled = false;
                postgreSQLDoStopButton_.Enabled = true;
                appendLog("PostgreSQL Started");
            }
        }
        private void postgreSQLStop()
        {
            //See
            //http://www.postgresql.org/docs/8.4/static/server-shutdown.html
            //0x1C : SIGQUIT -> Immediate Shutdown
            //0x03 : SIGINT -> Fast Shutdown
            postgreSQLDoStopButton_.Enabled = false;
            ThreadPool.QueueUserWorkItem(delegate(object state)
            {
                ProcessStartInfo start_info = new ProcessStartInfo();
                start_info.FileName = String.Format(@"{0}\pg_ctl.exe", new DirectoryInfo(postgreSQLBinPathBox_.Text).FullName);
                start_info.Arguments = String.Format(@"stop -m immediate -D ""{0}""", postgreSQLDataPathBox_.Text);
                start_info.UseShellExecute = false;
                start_info.CreateNoWindow = true;
                Process process = new Process();
                process.StartInfo = start_info;
                process.Start();
                process.WaitForExit();
                postgreSQLProcess_ = null;
            }, null);
        }
        private void buildPostgreSQLConfigFile()
        {
            using (StreamWriter writer = new StreamWriter(String.Format(@"{0}\Config\pg_hba.conf", Program.StartupPath), false))
            {
                foreach (IPAddress add in postgreSQLIPList_)
                    writer.WriteLine("host\tall\tall\t{0}/{1}\ttrust", add.ToString(), add.GetAddressBytes().Length);
            }
            using (StreamWriter writer = new StreamWriter(String.Format(@"{0}\Config\postgresql.conf", Program.StartupPath), false))
            {
                writer.WriteLine(File.ReadAllText(String.Format(@"{0}\postgresql.conf", postgreSQLDataPathBox_.Text)));
                writer.WriteLine(@"hba_file='{0}/Config/pg_hba.conf'", Program.StartupPath.FullName.Replace('\\', '/'));
            }
        }
        private bool validatePostgreSQLConfig()
        {
            string[] lines = postgreSQLIPBox_.Text.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            postgreSQLIPList_.Clear();
            IPAddress add;
            SortedList<string, IPAddress> added_adds = new SortedList<string, IPAddress>();
            foreach (string line in lines)
            {
                if (IPAddress.TryParse(line, out add) && !added_adds.ContainsKey(add.ToString()))
                {
                    postgreSQLIPList_.Add(add);
                    added_adds.Add(add.ToString(), add);
                }
            }
            if (postgreSQLIPList_.Count == 0)
            {
                postgreSQLIPLabel_.ForeColor = Color.Red;
                return false;
            }
            return true;
        }
        #endregion

        private void WAMPRoller_FormClosed(object sender, FormClosedEventArgs e)
        {
            saveSetting();
        }
        private void InstantServer_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (apacheProcess_ != null)
            {
                try { apacheStop(); }
                catch { }
            }
            if (mySQLProcess_ != null)
            {
                try { mySQLStop(); }
                catch { }
            }
            if (postgreSQLProcess_ != null)
            {
                try { postgreSQLStop(); }
                catch { }
            }

        }
        private void InstantServer_SizeChanged(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                Hide();
            }
            else
            {
                Show();
            }
        }

        private void homePageLink_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start(homePageLink_.Text);
            }
            catch { }
        }


    }
    delegate void SetFormCB();
}
