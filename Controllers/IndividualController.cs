using Avectra.netForum.Data;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Http;
using System.Web.Http.Cors;

namespace DeepDive2019.eWeb.API.Controllers
{
#if DEBUG
    [EnableCors(origins: "*", headers: "Content-Type", methods: "GET,PUT", SupportsCredentials = true)]
#else
    #warning Change the origins to match your production setup
    [EnableCors(origins: "", headers: "Content-Type", methods: "GET,PUT", SupportsCredentials = true)]
#endif
    public class IndividualController : ApiController
    {
        Guid? CustomerKey;
        FacadeClass Individual;

        public IndividualController() : base()
        {
            CustomerKey = this.GetCustomerKey();
            Individual = this.GetIndividual();
        }

        // GET api/<controller>
        public IDictionary<string, object> Get()
        {
            return Get("ind");
        }

        // GET api/<controller>/5
        public IDictionary<string, object> Get(string id)
        {
            if (CustomerKey == null)
            {
                throw new HttpResponseException(HttpStatusCode.Unauthorized);
            }

            if (Individual == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            Dictionary<string, object> result = Individual.GetDataObjectDictionary(id);

            return result;
        }

        // PUT api/<controller>/<id>
        public IDictionary<string, object> Put(string id, [FromBody]JObject update)
        {
            if (CustomerKey == null)
            {
                throw new HttpResponseException(HttpStatusCode.Unauthorized);
            }

            if (Individual == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            Individual.Merge(update);

            using (var connection = DataUtils.GetConnection())
            {
                using (var transaction = connection.BeginTransaction())
                {
                    var error = Individual.Update(connection, transaction);

                    if (error.HasError)
                    {
                        transaction.Rollback();

                        throw new ApiResponseException(
                            status: HttpStatusCode.InternalServerError,
                            message: error.Message,
                            code: error.Number
                        );
                    }

                    Individual.ProcessRoundTripEvents(connection, transaction);

                    transaction.Commit();
                }
            }

            Individual.AfterSave();

            return Get(id);
        }
    }
}