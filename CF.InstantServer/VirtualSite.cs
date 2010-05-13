using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
namespace CF.InstantServer
{
    [Serializable]
    internal class VirtualSite
    {
        private List<IPPair> ipPairs_ = new List<IPPair>();
        public List<IPPair> IPPairs
        {
            get { return ipPairs_; }
        }
        private DirectoryInfo documentRoot_;

        public DirectoryInfo DocumentRoot
        {
            get { return documentRoot_; }
            set { documentRoot_ = value; }
        }

        private string serverName_;

        public string ServerName
        {
            get { return serverName_; }
            set
            {
                if(value == null || !hostNameRegex_.IsMatch(value))
                    throw new ArgumentException(String.Format("{0} is not a valid host name!", value));
                serverName_ = value;

            }
        }
        public void AppendConfigSegment(StringBuilder bd, IEnumerable<IPAddress> all_ips)
        {
            string escape_root = documentRoot_.FullName.Replace('\\', '/');
            //bd.AppendFormat(@"DocumentRoot ""{0}""", escape_root);
            bd.AppendFormat(@"<Directory ""{0}"">", escape_root);
            bd.AppendLine();
            bd.AppendLine(@"Options Indexes FollowSymLinks");
            bd.AppendLine(@"AllowOverride All");
            bd.AppendLine(@"Order allow,deny");
            bd.AppendLine(@"Allow from all");
            bd.AppendLine(@"</Directory>");
            bd.Append(@"<VirtualHost ");
            bool found = false;
            for(int idx = 0; idx < ipPairs_.Count; idx++)
            {

                if(ipPairs_[idx].IPAddress == null)
                {
                    foreach(IPAddress ip in all_ips)
                    {
                        if(found)
                            bd.Append(' ');
                        bd.Append(ip);
                        bd.Append(":");
                        bd.Append(ipPairs_[idx].Port);
                        found = true;
                    }
                }
                else
                {
                    if(found)
                        bd.Append(' ');
                    bd.Append(ipPairs_[idx]);
                    found = true;
                }

            }
            bd.AppendLine(@">");
            //bd.AppendLine(@"ServerAdmin webmaster@dummy-host.localhost");
            bd.AppendFormat(@"DocumentRoot ""{0}""", escape_root);
            bd.AppendLine();
            bd.AppendFormat(@"ServerName {0}", serverName_);
            bd.AppendLine();
            bd.AppendLine(@"</VirtualHost>");
            //ServerAlias www.dummy-host.localhost
            //ErrorLog "logs/dummy-host.localhost-error.log"
            //CustomLog "logs/dummy-host.localhost-access.log" common
        }
        public override string ToString()
        {
            StringBuilder bd = new StringBuilder();
            bd.Append(serverName_);
            bd.Append("  ");
            bd.Append(documentRoot_.FullName);
            bd.Append("  ");
            for(int idx = 0; idx < ipPairs_.Count; idx++)
            {
                if(idx > 0)
                    bd.Append('/');
                bd.Append(ipPairs_[idx]);

            }
            return bd.ToString();

        }
        public static bool IsValidName(string name)
        {
            if(name == null)
                return false;
            return hostNameRegex_.IsMatch(name);
        }
        internal static Regex hostNameRegex_ = new Regex(@"^\*$|^(?:(?:[0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])\.){3}(?:[0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])$|^(?:(?:[a-zA-Z0-9]|[a-zA-Z0-9][a-zA-Z0-9\-]*[a-zA-Z0-9])\.)*(?:[A-Za-z]|[A-Za-z][A-Za-z0-9\-]*[A-Za-z0-9])$", RegexOptions.Compiled);


    }
    //(?:(?:[a-zA-Z0-9]|[a-zA-Z0-9][a-zA-Z0-9\-]*[a-zA-Z0-9])\.)*(?:[A-Za-z]|[A-Za-z][A-Za-z0-9\-]*[A-Za-z0-9])
    [Serializable]
    internal class IPPair
    {
        public IPPair(IPAddress address, ushort port)
        {
            IPAddress = address;
            Port = port;
        }
        private IPAddress ipAddress_ = null;
        public IPAddress IPAddress
        {
            get { return ipAddress_; }
            set
            {
                ipAddress_ = value;
            }
        }
        public ushort Port;
        public override string ToString()
        {
            if(ipAddress_ == null)
                return "*:" + Port.ToString();
            return ipAddress_.ToString() + ":" + Port.ToString();
        }
        private static Regex isHostRegex_ = new Regex(@"^\*$|^(?:(?:[0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])\.){3}(?:[0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])$", RegexOptions.Compiled);

    }
}
