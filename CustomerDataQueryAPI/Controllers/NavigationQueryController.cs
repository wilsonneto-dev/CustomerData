using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Couchbase;
using Couchbase.Authentication;
using Couchbase.N1QL;
using CustomerDataQueryAPI.Configuration;
using CustomerDataQueryAPI.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace CustomerDataQueryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NavigationQueryController : ControllerBase
    {
        private AppSettings AppSettings { get; set; }

        public NavigationQueryController(IOptions<AppSettings> appSettings)
        {
            AppSettings = appSettings.Value;
        }

        // GET
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get(String pageTitle = "", String ip = "")
        {
            // execute the search
            CustomerNavigationDB customerNavigationDB = new CustomerNavigationDB(AppSettings);
            var searchResult = customerNavigationDB.Search(pageTitle, ip);
            return new JsonResult(searchResult);
        }

    }
}
