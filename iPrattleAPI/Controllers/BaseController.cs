using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace iPrattleAPI.Controllers
{
    public abstract class BaseController : Controller
    {
        protected readonly IConfiguration _config;

        public BaseController(IConfiguration config)
        {
            _config = config;
            Log.Information("BaseController Constructor");
        }
    }
}
