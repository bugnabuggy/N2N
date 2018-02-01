using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using N2N.Api.Filters;
using N2N.Core.Services;
using N2N.Infrastructure.Models.DTO;

namespace N2N.Api.Controllers
{
    [Produces("application/json")]
    [Route("postcard")]
    [N2NAutorization]
    public class PostcardController : Controller
    {
        private readonly IPostCardService _postcardService; 

        public PostcardController(IPostCardService postcardService)
        {
            this._postcardService = postcardService;
        }

        // GET: /postcard
        [HttpGet()]
        [AllowAnonymous]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: postcard/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }
        
        // POST: postcard
        [HttpPost]
        public void Post([FromBody]N2NPostcardDTO postcardDTO)
        {
            //_postcardService.Add(postcardDTO);
        }
        
        // PUT: postcard/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]N2NPostcardDTO postcardDTO)
        {
            //_postcardService.Update(postcardDTO);
        }
        
        // DELETE: postcard/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            //_postcardService.Delete(id);
        }
    }
}
