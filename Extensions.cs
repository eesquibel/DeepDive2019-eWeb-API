using Avectra.netForum.Data;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace DeepDive2019.eWeb.API
{
    public static class Extensions
    {
        /// <summary>
        /// Get the eWeb Customer Key from the session
        /// </summary>
        /// <param name="apiController">The API controller used by the extension method.</param>
        /// <returns>Customer Key, or null if session is not started, the session variable is missing, or not a valid Guid</returns>
        public static Guid? GetCustomerKey(this ApiController apiController)
        {
            var Session = HttpContext.Current?.Session;

            if (Session == null)
            {
                return null;
            }

            string customerKey = (string)Session["CustomerKey"];

            if (Guid.TryParse(customerKey, out Guid result))
            {
                return result;
            } else
            {
                return null;
            }
        }

        /// <summary>
        /// Get the eWeb Individual facade object from the session
        /// </summary>
        /// <param name="apiController">The API controller used by the extension method.</param>
        /// <returns>The Individual facade object, or null if the session is not started or the value is uninitialized.</returns>
        public static FacadeClass GetIndividual(this ApiController apiController)
        {
            var Session = HttpContext.Current?.Session;

            if (Session == null)
            {
                return null;
            }

            return (FacadeClass)Session["Individual"];
        }

        /// <summary>
        /// Get the DataObject <em>id</em> from the FacadeClass as a dictionary of native values,
        /// filtering out nulls, hidden, or unreadable fields.
        /// </summary>
        /// <param name="facade">The FacadeClass</param>
        /// <param name="id">The prefix for the DataObject to return</param>
        /// <returns>Dictionary of field names and values from the DataObject</returns>
        public static Dictionary<string, object> GetDataObjectDictionary(this FacadeClass facade, string id)
        {
            var result = new Dictionary<string, object>();

            var data = facade.GetDataObject(id);

            if (data == null)
            {
                return null;
            }

            foreach (DictionaryEntry pair in data.GetFields())
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

        /// <summary>
        /// Copy the values from <em>update</em> to the named fields on the <em>facade</em>,
        /// skipping hidden, readyonly, and otherwise noneditable fields
        /// </summary>
        /// <param name="facade">The FacadeClass to copy the values onto.</param>
        /// <param name="update">The JSON object to read values from.</param>
        public static void Merge(this FacadeClass facade, JObject update)
        {
            foreach (var pair in update)
            {
                var token = pair.Value;
                var field = facade.GetField(pair.Key);

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

        }
    }
}