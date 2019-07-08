using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Couchbase;
using Couchbase.Authentication;
using Couchbase.N1QL;
using CustomerDataQueryAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace CustomerDataQueryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NavigationQueryController : ControllerBase
    {

        // GET
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get(String pageTitle = "", String ip = "")
        {
            var searchResult = CustomerNavigationDB.Search(pageTitle, ip);
            return new JsonResult(searchResult);
        }

    }
}
