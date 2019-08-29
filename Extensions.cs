﻿using Avectra.netForum.Data;
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

        public static FacadeClass GetIndividual(this ApiController apiController)
        {
            var Session = HttpContext.Current?.Session;

            if (Session == null)
            {
                return null;
            }

            return (FacadeClass)Session["Individual"];
        }

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