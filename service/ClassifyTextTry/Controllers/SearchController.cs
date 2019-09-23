using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClassifyTextTry.Model;
using Microsoft.AspNetCore.Mvc;


namespace ClassifyTextTry.Controllers
{
    [Route("api/search")]
    [Produces("application/json")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        public ActionResult<string> Get()
        {
            string text = "Google Home enables users to speak voice commands to interact with services through the Home's intelligent personal assistant called Google Assistant. A large number of services, both in-house and third-party, are integrated, allowing users to listen to music, look at videos or photos, or receive news updates entirely by voice.";
            return CategoryProcessor.Get(text);
        }

        // POST api/values
        [HttpPost]
        public ActionResult<string> Post([FromBody] string toclassify)
        {
            return CategoryProcessor.Get(toclassify);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
