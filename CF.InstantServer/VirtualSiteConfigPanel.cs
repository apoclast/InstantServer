using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace CF.InstantServer
{
    internal partial class VirtualSiteConfigPanel : UserControl
    {
        public VirtualSiteConfigPanel()
        {
            InitializeComponent();
        }
        public VirtualSite[] GetVirtualSites()
        {
            VirtualSite[] sites = new VirtualSite[virtualSites_.Count];
            virtualSites_.CopyTo(sites, 0);
            return sites;
        }
        public void SetVirtualSites(VirtualSite[] sites)
        {
            virtualSites_.Clear();
            virtualSiteListBox_.Items.Clear();
            for (int idx = 0; idx < sites.Length; idx++)
            {
                virtualSites_.Add(sites[idx]);
                virtualSiteListBox_.Items.Add(sites[idx]);
            }

        }
        private List<VirtualSite> virtualSites_ = new List<VirtualSite>();
        private void doAddSiteButton_Click(object sender, EventArgs e)
        {
            VirtualSiteEditDialog edit_dialog = new VirtualSiteEditDialog(new VirtualSite());
            if (edit_dialog.ShowDialog() == DialogResult.OK)
            {
                
                    virtualSites_.Add(edit_dialog.VirtualSite);
                
                virtualSiteListBox_.Items.Add(edit_dialog.VirtualSite);
            }
        }
        public void AppendConfigSegment(StringBuilder bd)
        {
            SortedList<ushort, bool> watch_port = new SortedList<ushort, bool>();
            SortedList<string, IPAddress> watch_ip = new SortedList<string, IPAddress>();
            SortedList<string, IPPair> watch_scope = new SortedList<string, IPPair>();
            SortedList<string, bool> added_scope = new SortedList<string, bool>();
            foreach (VirtualSite site in virtualSites_)
            {
                foreach (IPPair p in site.IPPairs)
                {
                    if (!watch_port.ContainsKey(p.Port))
                        watch_port.Add(p.Port, true);
                    if (p.IPAddress != null && !watch_ip.ContainsKey(p.IPAddress.ToString()))
                        watch_ip.Add(p.IPAddress.ToString(), p.IPAddress);
                    if (!watch_scope.ContainsKey(p.ToString()))
                        watch_scope.Add(p.ToString(), p);
                }
            }
            foreach (ushort port in watch_port.Keys)
            {
                bd.AppendFormat(@"Listen {0}", port);
                bd.AppendLine();
            }
            if (watch_ip.Count == 0)
                watch_ip.Add("127.0.0.1", new IPAddress(new byte[] { 127, 0, 0, 1 }));

            foreach (KeyValuePair<string, IPPair> pair in watch_scope)
            {
                IPPair p = pair.Value;
                if (p.IPAddress == null)
                {
                    foreach (IPAddress add in watch_ip.Values)
                    {
                        if (!added_scope.ContainsKey(String.Format("{0}:{1}", add, p.Port)))
                        {
                            bd.AppendFormat(@"NameVirtualHost {0}:{1}", add, p.Port);
                            bd.AppendLine();
                            added_scope.Add(String.Format("{0}:{1}", add, p.Port), true);
                        }
                    }
                }
                else
                {
                    if (!added_scope.ContainsKey(String.Format("{0}:{1}", p.IPAddress, p.Port)))
                    {
                        bd.AppendFormat(@"NameVirtualHost {0}:{1}", p.IPAddress, p.Port);
                        bd.AppendLine();
                        String.Format("{0}:{1}", p.IPAddress, p.Port);
                    }
                }
            }
            foreach (VirtualSite site in virtualSites_)
            {
                site.AppendConfigSegment(bd, watch_ip.Values);
            }
        }

        private void virtualSiteListBox_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (virtualSiteListBox_.SelectedItem is VirtualSite)
            {
                VirtualSite site = (VirtualSite)virtualSiteListBox_.SelectedItem;
                if (new VirtualSiteEditDialog(site).ShowDialog() == DialogResult.OK)
                {
                    virtualSiteListBox_.Refresh();
                }
            }
        }

        private void doRemoveSiteButton__Click(object sender, EventArgs e)
        {
            if (virtualSiteListBox_.SelectedItem is VirtualSite)
            {
                virtualSites_.Remove((VirtualSite)virtualSiteListBox_.SelectedItem);
                virtualSiteListBox_.Items.Remove(virtualSiteListBox_.SelectedItem);
                
            }
        }
    }
}
