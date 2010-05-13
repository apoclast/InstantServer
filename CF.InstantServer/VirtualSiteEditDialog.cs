using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace CF.InstantServer
{
    internal partial class VirtualSiteEditDialog : Form
    {
        public VirtualSiteEditDialog(VirtualSite site)
        {
            InitializeComponent();
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
            virtualSite_ = site;
            if (virtualSite_.DocumentRoot != null)
                documentRootBox_.Text = virtualSite_.DocumentRoot.FullName;
            if (!String.IsNullOrEmpty(virtualSite_.ServerName))
                serverNameBox_.Text = virtualSite_.ServerName;
            StringBuilder bd = new StringBuilder();
            foreach (IPPair p in virtualSite_.IPPairs)
                bd.AppendLine(p.ToString());
            addressListBox_.Text = bd.ToString();
        }

    
        private bool getVirtualSite()
        {
            try
            {
                if (!Directory.Exists(documentRootBox_.Text))
                    return false;
            }
            catch { return false; }

            if (!VirtualSite.IsValidName(serverNameBox_.Text))
                return false;

            MatchCollection mths = parseIPPairRegex_.Matches(addressListBox_.Text);
            List<IPPair> addresses = new List<IPPair>();
            for (int idx = 0; idx < mths.Count; idx++)
            {
                if (!mths[idx].Success)
                    continue;
                IPAddress add;
                ushort port;
                if (!ushort.TryParse(mths[idx].Groups[2].Value, out port))
                    continue;
                if (mths[idx].Groups[1].Value == "*")
                    addresses.Add(new IPPair(null, port));
                else if (IPAddress.TryParse(mths[idx].Groups[1].Value, out add))
                    addresses.Add(new IPPair(add, port));
            }
            if (addresses.Count == 0)
                return false;


            virtualSite_.DocumentRoot = new DirectoryInfo(documentRootBox_.Text);
            virtualSite_.ServerName = serverNameBox_.Text.ToLower();
            virtualSite_.IPPairs.Clear();
            virtualSite_.IPPairs.AddRange(addresses);
            return true;

        }
        private static Regex parseIPPairRegex_ = new Regex(@"^(\*|(?:(?:[0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])\.){3}(?:[0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5]))\:(\d+)\r?\n?$", RegexOptions.Compiled | RegexOptions.Multiline);
        public VirtualSite VirtualSite
        {
            get { return virtualSite_; }
        }
        private VirtualSite virtualSite_;
        private void okButton_Click(object sender, EventArgs e)
        {
            if (getVirtualSite())
            {
                DialogResult = System.Windows.Forms.DialogResult.OK;
                Close();
            }
            else
            {
                validateMessageLabel_.Text = "Some information is incorrect. Please check your setting";
            }
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
            Close();
        }

        private void doSelectDocumentRootButton_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog opener = new FolderBrowserDialog();
            opener.SelectedPath = Program.StartupPath.FullName;
            opener.RootFolder = Environment.SpecialFolder.MyComputer;
            opener.ShowNewFolderButton = true;
            if(opener.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                documentRootBox_.Text = opener.SelectedPath;
            }
        }




       
    }
}
