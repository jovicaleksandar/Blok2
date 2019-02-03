using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Contracts;

namespace ServiceApp
{
    public class Logger
    {
        public void WriteToEventLog(string LogName, string SourceName, int clientID , string opis)
        {
            string user = WindowsIdentity.GetCurrent().User.ToString();

            if (!EventLog.SourceExists(SourceName))
            {
                EventLog.CreateEventSource(SourceName, LogName);
            }
            EventLog newLog = new EventLog(LogName, Environment.MachineName, SourceName);
            newLog.WriteEntry(opis, EventLogEntryType.Information, clientID);
        }
    }
}
