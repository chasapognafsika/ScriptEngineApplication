using System.Threading.Tasks;
using System.Collections.Generic;
using WebApp;
using NLog;
using System.Net.Http;

namespace ScriptServer
{
    class CommandsServer
    {
        RestClient _restclient = null;
        private static readonly ILogger ErrorLogger = LogManager.GetLogger("fileErrorLogger");

        public CommandsServer(string url)
        {
            _restclient = new RestClient(url);
        }

        public async Task ExecuteScript(List<ScriptEngineParams> commands)
        {
            foreach (var cmd in commands)
            {
                if (cmd is SelectWindowParams) { var c = (SelectWindowParams)(cmd); await SelectWindow(c.window); }
                if (cmd is SetCursorParams) { var c = (SetCursorParams)(cmd); await SetCursor(c.x, c.y); }
                if (cmd is MouseClickParams) { var c = (MouseClickParams)(cmd); await DoMouseClick(); }
                if (cmd is SendKeysParams) { var c = (SendKeysParams)(cmd); await SendKeys(c.str); }
            }
        }

        private async Task SelectWindow(string window)
        {
            try 
            { 
                await _restclient.PostAsync<string,SelectWindowParams>(
                    "/api/ExecuteCommand/SelectWindow", 
                    new SelectWindowParams()
                    {
                        window = window 
                    });
            }
            catch (HttpRequestException ex)
            {
                ErrorLogger.Error(ex.Message);
            }
        }

        private async Task SetCursor(int x, int y)
        {
            try
            {
                await _restclient.PostAsync<string,SetCursorParams>(
                    "/api/ExecuteCommand/SetCursor", 
                    new SetCursorParams() 
                    { 
                        x = x, y = y 
                    });
            }
            catch (HttpRequestException ex)
            {
                ErrorLogger.Error(ex.Message);
            }
        }

        private async Task SendKeys(string str)
        {
            try
            {
                await _restclient.PostAsync<string,SendKeysParams>(
                    "/api/ExecuteCommand/SendKeys", 
                    new SendKeysParams() 
                    { 
                        str = str 
                    });
            }
            catch (HttpRequestException ex)
            {
                ErrorLogger.Error(ex.Message);
            }
        }

        private async Task DoMouseClick()
        {
            try
            {
                await _restclient.PostAsync<string, object>(
                "/api/ExecuteCommand/DoMouseClick", 
                null);
            }
            catch (HttpRequestException ex)
            {
                ErrorLogger.Error(ex.Message);
            }
        }
    }
}
