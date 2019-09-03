using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeepDive2019.eWeb.API.Models.Decorators
{
    [AttributeUsage(validOn: AttributeTargets.Property | AttributeTargets.Class)]
    public class CreateAttribute : Attribute
    {
    }
}
