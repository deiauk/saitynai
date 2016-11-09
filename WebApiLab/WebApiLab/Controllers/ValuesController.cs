using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApiLab.Models;

namespace WebApiLab.Controllers
{
    //[Authorize]
    public class ValuesController : ApiController
    {
        [Route("api/{Values}/{id}/{fromVal}/{toVal}")]
        public IEnumerable<MyData> Get(string id, int fromVal, int toVal)
        {
            MetaParser myParser = new MetaParser { Source = id, FromValue = fromVal, ToValue = toVal };
            myParser.Parse();
            return myParser.myDataList;
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
