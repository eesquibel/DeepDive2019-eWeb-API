using Avectra.netForum.Common;
using Avectra.netForum.Data;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Web.Http;
using System.Web.Http.Cors;

namespace DeepDive2019.eWeb.API.Controllers
{
#if DEBUG
    [EnableCors(origins: "*", headers: "Content-Type", methods: "GET", SupportsCredentials = true)]
#else
    #warning Change the origins to match your production setup
    [EnableCors(origins: "", headers: "Content-Type", methods: "GET", SupportsCredentials = true)]
#endif
    public class DirectoryController : ApiController
    {
        Guid? CustomerKey;
        FacadeClass Individual;

        public DirectoryController() : base()
        {
            CustomerKey = this.GetCustomerKey();
            Individual = this.GetIndividual();
        }

        // GET api/<controller>
        public IEnumerable<Hashtable> Get()
        {
            // Make sure the user is logged in
            if (CustomerKey == null || Individual == null || string.IsNullOrEmpty(Individual.CurrentKey))
            {
                throw new HttpResponseException(HttpStatusCode.Unauthorized);
            }

            // This could be passed in via a query string (compared against a whitelist), or derived from the object model
            var columns = new string[] { "cst_recno", "ind_first_name", "ind_last_name", "ind_badge_name", "eml_address", "adr_city", "adr_line1", "adr_post_code", "adr_state" };

            // Using a real data model is preferable
            var result = new List<Hashtable>();

            using (FacadeClass facade = FacadeObjectFactory.CreateIndividual()) // Re-use the facade to reduce memory usage
            using (var connection = DataUtils.GetConnection())
            using (var transaction = connection.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted))

            // In a real-world application, you wouldn't dump your whole database
            // Ideally a high-level filter would be applied (i.e. adr_state in this case), with pagination
            // Any more specific filtering (city, zip, radius) could be done client-side
            // Adding a caching mechanism would increase performance
            using (var cmd = new NfDbCommand(@"
SELECT
    ind_cst_key
FROM
    co_customer (NOLOCK)
    JOIN co_individual (NOLOCK) ON ind_cst_key = cst_key AND ind_delete_flag = 0
WHERE
    cst_delete_flag = 0
", connection, transaction))
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.HasRows && reader.Read())
                {
                    facade.CurrentKey = ((Guid)reader.GetValue(0)).ToString();
                    facade.SelectByKey(connection, transaction);

                    // Using a real data model is preferable
                    var record = new Hashtable();

                    foreach (var col in columns)
                    {
                        // Using GetValueNative so booleans and numeric data types come over to JSON as their proper data types
                        record[col] = facade.GetValueNative(col);
                    }

                    result.Add(record);

                    // Clear facade values between loop iterations
                    facade.ClearValues();
                }
            }

            return result;
        }
    }
}