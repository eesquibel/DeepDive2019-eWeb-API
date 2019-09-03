using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeepDive2019.eWeb.API.Models.Decorators
{
    [AttributeUsage(validOn: AttributeTargets.Class)]
    public class DeleteAttribute : Attribute
    {
    }
}
