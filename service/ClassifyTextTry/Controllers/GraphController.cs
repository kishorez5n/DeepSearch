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

    [Route("api/graph")]
    [Produces("application/json")]
    [ApiController]
    public class GraphController : ControllerBase
    {
        private GraphQuery graphQuery;

        public GraphController(GraphQuery query)
        {
            graphQuery = query;
        }

        // GET api/values
        [HttpGet]
        public ActionResult<string> Get()
        {
            return graphQuery.GetCategories();
        }

        // GET api/values
        [Route("category")]
        [HttpGet]
        public ActionResult<string> GetSubCategory(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return graphQuery.GetCategories();
            }
            else
            {
                return graphQuery.GetSubCategories(name);
            }
        }

        // GET api/values
        [Route("link")]
        [HttpGet]
        public ActionResult<string> GetCategoryLinks(string name)
        {
            return graphQuery.GetCategoryLinks(name);
        }

        // POST api/values
        [HttpPost]
        public ActionResult<string> Post([FromBody] string toclassify)
        {
            var linkData = JsonConvert.DeserializeObject<LinkData>(toclassify);
            return graphQuery.GetCategories();
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
