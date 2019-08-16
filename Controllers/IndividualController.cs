using Avectra.netForum.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace DeepDive2019.eWeb.API.Controllers
{
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

            var result = new Dictionary<string, object>();

            foreach (DictionaryEntry pair in Individual.GetDataObject(id).GetFields())
            {
                Field field = (Field)pair.Value;

                if (field == null || field.ValueNative == null)
                {
                    continue;
                }

                // Skip hidden fields
                if (!field.CanSelect || field.Properties.Hidden)
                {
                    continue;
                }

                result.Add((string)pair.Key, field.ValueNative);
            }

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

            foreach (var pair in update)
            {
                var token = pair.Value;
                var field = Individual.GetField(pair.Key);

                // Make sure the field exists
                if (field == null)
                {
                    continue;
                }

                // Skip read-only fields
                if (!field.CanUpdate || field.Properties.ReadOnly || field.Properties.ReadOnlyEdit || field.Properties.NotEditable)
                {
                    continue;
                }

                // Make sure its a scalar type
                if (token is JValue jvalue)
                {
                    field.ValueNative = jvalue.Value;
                }
            }

            using (var connection = DataUtils.GetConnection())
            {
                using (var transaction = connection.BeginTransaction())
                {
                    var error = Individual.Update(connection, transaction);

                    if (error.HasError)
                    {
                        transaction.Rollback();

                        throw new HttpResponseException
                        (
                            new HttpResponseMessage(HttpStatusCode.InternalServerError)
                            {
                                Content = new StringContent(JsonConvert.SerializeObject(new
                                {
                                    error.Message,
                                    error.UserMessage,
                                    error.Number
                                }), Encoding.UTF8, "application/json")
                            }
                        );
                    }

                    transaction.Commit();
                }
            }

            return Get(id);
        }
    }
}