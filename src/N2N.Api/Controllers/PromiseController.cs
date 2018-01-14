using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using N2N.Api.Filters;
using N2N.Data.Repositories;

namespace N2N.Api.Controllers
{
    [Produces("application/json")]
    [Route("Promise")]
    public class PromiseController : Controller
    {

        [N2NAutorization]
        public JsonResult Get()
        {

            return Json(new object());
        }

    }
}
