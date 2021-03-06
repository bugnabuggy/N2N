﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace N2N.Api.Controllers
{
    [Produces("application/json")]
    public class N2NController : Controller
    {
        [HttpGet]
        public string Index(string name)
        {
            name = name?? "World!";
            return $"Hello {name}";
        }
    }
}
