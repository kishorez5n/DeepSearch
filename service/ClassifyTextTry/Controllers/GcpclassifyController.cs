using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClassifyTextTry.Model;
using ClassifyTextTry.Model.Data;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ClassifyTextTry.Controllers
{
    [Route("api/classify")]
    [Produces("application/json")]
    [ApiController]
    public class GcpclassifyController : ControllerBase
    {
        private CategoryProcessor processor;

        public GcpclassifyController(CategoryProcessor proc)
        {
            processor = proc;
        }

        // GET api/classify
        [HttpGet]
        public ActionResult<string> Get()
        {
            string text = "Google Home enables users to speak voice commands to interact with services through the Home's intelligent personal assistant called Google Assistant. A large number of services, both in-house and third-party, are integrated, allowing users to listen to music, look at videos or photos, or receive news updates entirely by voice.";
            return processor.Get(text);
        }

        // POST api/classify
        [HttpPost]
        public ActionResult<string> Post([FromBody] string toclassify)
        {
            var linkData = JsonConvert.DeserializeObject<LinkData>(toclassify);
            return processor.ClassifyData(linkData);
        }

        // PUT api/classify/5
        [HttpPost("{id}")]
        public ActionResult<string> PostTest(int id, [FromBody] string toclassify)
        {
            var linkData = JsonConvert.DeserializeObject<LinkData>(toclassify);
            return processor.Get(linkData.Text);
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
