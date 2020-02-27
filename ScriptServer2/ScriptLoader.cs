using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using NLog;
using ScriptServer.Controllers;

namespace ScriptServer
{
    public partial class ScriptLoader : Form
    {
        static ScriptLoader _this;
        List<List<ScriptEngineParams>> _scriptsList = new List<List<ScriptEngineParams>>();
        private static readonly ILogger ErrorLogger = LogManager.GetLogger("fileErrorLogger");

        public ScriptLoader()
        {
            _this = this;
            InitializeComponent();
        }

        public static void SendNotifications(string url)
        {
            // Running on the worker thread
            _this.lblClients.Invoke((MethodInvoker)delegate {
                // Running on the UI thread
                _this.lblClients.Items.Add(url);
            });
            // Back on the worker thread
        }

        private void btnRunScript_Click(object sender, EventArgs e)
        {
            string error = "";
            if (lblClients.SelectedIndex == -1)
                error = "Please select a registered client";           
            if (lblScripts.SelectedIndex == -1)
                error += "\n Please select an action script";
            if (error != "")
            { 
                MessageBox.Show(error,"ERROR", MessageBoxButtons.OK);
            }
            else
            {
                string url = lblClients.SelectedItem.ToString();
                int idx = lblScripts.SelectedIndex;
                var response = ServeClientController.DoExecuteScript(url, _scriptsList[idx]);
            }
        }

        private void btnLoadScript_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            var res = dlg.ShowDialog();
            if (res == DialogResult.OK)
            {
                string file = dlg.FileName;
                try
                {
                    string script = File.ReadAllText(file);
                    var parser = new ScriptParser(script);
                    var cmdList = parser._parms;
                    _scriptsList.Add(cmdList);
                    lblScripts.Items.Add(file);
                }
                catch(Exception ex)
                {
                    ErrorLogger.Error(ex);
                    MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK);
                }
            }
        }
    }
}
