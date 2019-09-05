using System;

namespace DeepDive2019.eWeb.API.Models.Decorators
{
    [AttributeUsage(validOn: AttributeTargets.Property | AttributeTargets.Class)]
    public class ReadAttribute : Attribute
    {
    }
}
