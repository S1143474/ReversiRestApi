using System;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Primitives;

namespace Reversi.API.Filters
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class QueryStringConstraintAttribute : ActionMethodSelectorAttribute
    {
        public string QueryStringName { get; set; }
        public bool CanPass { get; set; }

        public QueryStringConstraintAttribute(string qname, bool canpass)
        {
            QueryStringName = qname;
            CanPass = canpass;
        }

        public override bool IsValidForRequest(RouteContext routeContext, ActionDescriptor action)
        {
            StringValues value;

            routeContext.HttpContext.Request.Query.TryGetValue(QueryStringName, out value);

            if (QueryStringName == "" && CanPass)
            {
                return true;
            }

            if (CanPass)
            {
                return !StringValues.IsNullOrEmpty(value);
            }

            return StringValues.IsNullOrEmpty(value);
        }
    }
}
