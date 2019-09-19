using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SharpPacker.Base.Models;

namespace SharpPacker.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BoxPackerController : ControllerBase
    {
        // GET api/BoxPacker/Pack
        [HttpGet]
        public ActionResult<string> Get([FromBody]BoxPackerRequest value)
        {
            throw new InvalidOperationException();
        }

        [HttpGet("{strategy}")]
        public ActionResult<string> Get(string strategy, [FromBody]BoxPackerRequest request)
        {
            return request.ToString();
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
            throw new InvalidOperationException();
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
            throw new InvalidOperationException();
        }
    }
}
