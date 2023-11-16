using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace POCThinClientServices.Classes
{
    public static class ApplicationLogger
    {
        public static string MessageLogLocation
        {
            get
            {                
                return Path.Combine(
                    AssemblyLoadDirectory(),
                    string.Format("POCThinClientServices_{0}.log", DateTime.Now.ToString("yyyy-MM-dd")));
            }
        }
        static public string AssemblyLoadDirectory()
        {
            string codeBase = Assembly.GetEntryAssembly()?.Location;

            if (codeBase == null)
            {
                throw new InvalidOperationException("Invalid assembly directory.");
            }

            UriBuilder uri = new UriBuilder(codeBase);
            string path = Uri.UnescapeDataString(uri.Path);

            // Ensuring the return string is never null, since
            // GetDirectoryName will return null if in the root directory
            return Path.GetDirectoryName(path) ?? "";
        }
        /// <summary>
        /// Activate the logging if set to <see langword="true"/>
        /// </summary>
        public static bool Active { get; set; } = false;

        internal static void LogMessage(string msg)
        {
            if (!Active)
            {
                return;
            }

            try
            {
                File.AppendAllText(MessageLogLocation, msg + Environment.NewLine);
            }
            catch (IOException)
            {
                //Console.WriteLine("Could not log to file");
                //Supress Exception
            }
        }
    }
}
