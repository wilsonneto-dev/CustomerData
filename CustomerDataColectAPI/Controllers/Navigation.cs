using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using CustomerDataColectAPI.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace CustomerDataColectAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NavigationController : ControllerBase
    {
        
        // GET api/test
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "Thats All Working Fine! :)" };
        }
        

        // Save customer navigation data (IP, Page, Params, Broser)
        // POST api/navigation
        [EnableCors]
        [HttpPost]
        public ActionResult<string> Post([FromBody] CustomerNavigation customerNavigation)
        {
            try
            {
                // Fill IP and Date
                customerNavigation.IP = this.Request.HttpContext.Connection.RemoteIpAddress.ToString();
                customerNavigation.Date = DateTime.Now;

                // serialize in JSON
                String jsonPack = JsonConvert.SerializeObject(customerNavigation);

                // post to queue
                QueuePost(jsonPack);

                // return OK
                return new JsonResult("Send to queue successfully.");

            }
            catch (Exception ex)
            {
                return new JsonResult("Error sending to queue: " + ex.Message);
            }

        }

        private void QueuePost(string jsonCustomerNavigation)
        {
            // Post the customer data to Queue / RabbitMQ Server
            var factory = new ConnectionFactory()
            {
                HostName = "whale-01.rmq.cloudamqp.com",
                UserName = "yyvswksf",
                VirtualHost = "yyvswksf",
                Password = "aw1hZCbj52fN4U--M3yX9NBSjAMx7xLS"
            };

            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(
                        queue: "Customer",
                        durable: true,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null
                    );

                    var body = Encoding.UTF8.GetBytes(jsonCustomerNavigation);

                    channel.BasicPublish(
                        exchange: "",
                        routingKey: "Customer",
                        basicProperties: null,
                        body: body
                    );
                }
            }
        }

    }
}
