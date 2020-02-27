using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace ScriptServer.Controllers
{
    [Route("api/[controller]")]
    public class ServeClientController : ApiController
    {
        private readonly List<string> _clients = new List<string>();

        [HttpPost]
        public IHttpActionResult Register([FromBody]RequestModel req)
        {
            _clients.Add(req.url);
            Action<string> notifier = ScriptLoader.SendNotifications;
            notifier.BeginInvoke(req.url, null, null);
            return Ok(req.url);
        }

        public static async Task DoExecuteScript(string url, List<ScriptEngineParams> commands)
        {
            var srv = new CommandsServer(url);
            await srv.ExecuteScript(commands);
        }
    }

    public class RequestModel
    {
        public string url { get; set; }
    }

}
