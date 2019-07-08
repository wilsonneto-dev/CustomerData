using Couchbase;
using Couchbase.Authentication;
using Couchbase.N1QL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerDataQueryAPI.Services
{
    public class CustomerNavigationDB
    {
        public static object Search(String pageTitle = "", String ip = "")
        {
            // connect server
            using (var cluster = new Cluster())
            {
                // autentication parameters
                var authenticator = new PasswordAuthenticator("user", "user123");
                cluster.Authenticate(authenticator);

                // open bucket
                using (var bucket = cluster.OpenBucket("CustomerNavigations"))
                {
                    // request with N1QL

                    // where clausules based on received parameters
                    String whereClausules = "";
                    if (!String.IsNullOrEmpty(pageTitle))
                        whereClausules = "pageTitle = $pageTitle";
                    if (!String.IsNullOrEmpty(ip))
                        whereClausules += " ip = $ip";

                    // add "where" if have where clausules
                    if (whereClausules.Length > 0)
                        whereClausules = " WHERE " + whereClausules;

                    // prepare query
                    var queryRequest = new QueryRequest()
                        .Statement("SELECT * FROM CustomerNavigations " + whereClausules + " ORDER BY id LIMIT 100");

                    // add parameters values
                    if (!String.IsNullOrEmpty(pageTitle))
                        queryRequest.AddNamedParameter("$pageTitle", pageTitle);
                    if (!String.IsNullOrEmpty(ip))
                        queryRequest.AddNamedParameter("$ip", ip);

                    var result = bucket.Query<dynamic>(queryRequest);
                    return result.Rows;

                }
            }
        }
    }
}
