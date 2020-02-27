using NLog;
using System;
using System.Windows.Forms;

namespace ScriptServer
{
    static class Program
    {
        const string url = "http://localhost:9000";

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            using (Microsoft.Owin.Hosting.WebApp.Start<Startup>(url))
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new ScriptLoader());
            }
        }
    }
}
