using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using CustomerDataColectAPI.Configuration;
using CustomerDataColectAPI.Models;
using CustomerDataColectAPI.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace CustomerDataColectAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NavigationController : ControllerBase
    {
        
        private AppSettings AppSettings { get; set; }

        // GET api/test
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "Thats All Working Fine! :)" };
        }
        
        // Save customer navigation data (IP, Page, Params, Broser)
        // POST api/navigation
        [HttpPost]
        public ActionResult<string> Post([FromBody] CustomerNavigation customerNavigation)
        {
            try
            {
                // Fill IP and Date
                customerNavigation.IP = this.Request.HttpContext.Connection.RemoteIpAddress.ToString();
                customerNavigation.Date = DateTime.Now;
                // _context.

                // serialize in JSON
                String jsonPack = JsonConvert.SerializeObject(customerNavigation);

                // post to queue
                Queue queue = new Queue();
                queue.QueuePostJson(jsonPack, "Customer");

                // return OK
                return new JsonResult("Send to queue successfully.");

            }
            catch (Exception ex)
            {
                return new JsonResult("Error sending to queue: " + ex.Message);
            }

        }

    }
}
