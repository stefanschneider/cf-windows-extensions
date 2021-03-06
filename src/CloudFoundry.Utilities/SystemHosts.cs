﻿namespace CloudFoundry.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Text;

    /// <summary>
    /// Utility for modifying the windows system hosts file.
    /// </summary>
    public static class SystemHosts
    {
        /// <summary>
        /// Gets the hosts file path.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "It will be used.")]
        private static string HostsFilePath
        {
            get
            {
                return Path.Combine(Environment.SystemDirectory, @"drivers\etc\hosts");
            }
        }

        /// <summary>
        /// Adds the specified unique hostname.
        /// </summary>
        /// <param name="hostName">The hostname.</param>
        /// <param name="ipAddress">The ip address.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "It will be used.")]
        public static void Add(string hostName, string ipAddress)
        {
            if (hostName == null)
            {
                throw new ArgumentNullException("hostName");
            }

            if (ipAddress == null)
            {
                throw new ArgumentNullException("ipAddress");
            }

            if (hostName.Length > 64)
            {
                throw new ArgumentException("Invalid hostName. It has more then 64 characters.", "hostName");
            }

            IPAddress ip;
            if (!IPAddress.TryParse(ipAddress, out ip))
            {
                throw new ArgumentException("Invalid IP address format", "ipAddress");
            }

            //// List<string> hosts = File.ReadAllLines(HostsFilePath).ToList();
            //// int hostNameIndex = hosts.FindIndex(s => s.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ElementAtOrDefault(1) == hostName);
            //// if (hostNameIndex != -1)
            //// {
            ////   throw new ArgumentException("Hostname already exists", "hostName");
            //// }
            File.AppendAllText(HostsFilePath, "\n" + ipAddress + " " + hostName + " # timestamp: " + DateTime.Now.ToString(CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// Tries the remove the specified host name.
        /// </summary>
        /// <param name="hostName">The hostname.</param>
        /// <returns>True if removal was successful.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "No reason to propagate the exception."), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "It will be used.")]
        public static bool TryRemove(string hostName)
        {
            if (hostName == null)
            {
                throw new ArgumentNullException("hostName");
            }

            List<string> hosts = File.ReadAllLines(HostsFilePath).ToList();

            bool hostRemoved = false;

            for (int i = 0; i < hosts.Count; i++)
            {
                try
                {
                    string[] hostLine = hosts[i].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    if (hostLine[1] == hostName)
                    {
                        hosts.RemoveAt(i);
                        hostRemoved = true;
                        i--;
                    }
                }
                catch
                {
                }
            }

            if (hostRemoved)
            {
                File.WriteAllLines(HostsFilePath, hosts);
            }

            return hostRemoved;
        }

        /// <summary>
        /// Check is the host is present in etc\hosts.
        /// </summary>
        /// <param name="hostName">The hostname.</param>
        /// <returns>
        /// True if the host is present.
        /// </returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "No reason to propagate the exception."), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "It will be used.")]
        public static bool Exists(string hostName)
        {
            if (hostName == null)
            {
                throw new ArgumentNullException("hostName");
            }

            List<string> hosts = File.ReadAllLines(HostsFilePath).ToList();

            bool exists = false;

            for (int i = 0; i < hosts.Count; i++)
            {
                try
                {
                    string[] hostLine = hosts[i].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    if (hostLine[1] == hostName)
                    {
                        exists = true;
                    }
                }
                catch
                {
                }
            }

            return exists;
        }
    }
}
