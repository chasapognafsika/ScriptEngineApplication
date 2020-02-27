using System.Net;
using System.Net.Http;
using System.Web.Http;
using ScriptClient.Services;

namespace ScriptClient.Controllers
{
    public class ExecuteCommandController : ApiController
    {
        ExecuteCommandEngine eng = new ExecuteCommandEngine();

        [HttpPost]
        public IHttpActionResult SelectWindow([FromBody]SelectWindowParams req)
        {
            if (eng.SelectWindow(req.window))
                return Ok();
            else
                return NotFound();
        }

        [HttpPost]
        public IHttpActionResult SetCursor([FromBody]SetCursorParams req)
        {
            if (eng.SetCursor(req.x, req.y))
                return Ok();
            else
                return NotFound();
        }

        [HttpPost]
        public IHttpActionResult DoMouseClick()
        {
            if (eng.DoMouseClick())
                return Ok();
            else
                return NotFound();
        }

        [HttpPost]
        public IHttpActionResult SendKeys([FromBody]SendKeysParams req)
        {
            if (eng.SendKeys(req.str))
                return Ok();
            else 
                return NotFound();
        }

    }
}