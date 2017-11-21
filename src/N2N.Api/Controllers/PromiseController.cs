using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using N2N.Data.Repositories;

namespace N2N.Api.Controllers
{
    [Produces("application/json")]
    [Route("Promise")]
    public class PromiseController : Controller
    {
        private TestClass _test;

        public PromiseController(TestClass test)
        {
            this._test = test;
        }

        // GET: /Promise
        [HttpGet]
        public string Get()
        {
            return this._test.Test();
        }
    }
}
