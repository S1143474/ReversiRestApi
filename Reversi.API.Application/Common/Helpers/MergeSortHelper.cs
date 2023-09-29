using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Reversi.API.Application.Common.Interfaces;

namespace Reversi.API.Application.Common.Helpers
{
    public class MergeSortHelper<T> : ISortHelper<T>
    {
        public IQueryable<T> ApplySort(IQueryable<T> entities, string orderByQueryString)
        {
            // When Orderby fails in its speed maby try to make a mergesort variant.
            throw new NotImplementedException();
            /*
            if (!entities.Any())
                return entities;

            if (string.IsNullOrWhiteSpace(orderByQueryString))
                return entities;

            var orderParams = orderByQueryString.Trim().Split(',');
            var propertyInfos = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var orderQueryBuilder = new StringBuilder();

            foreach (var param in orderParams)
            {
                if (string.IsNullOrWhiteSpace(param))
                    continue;

                var propertyFromQueryName = param.Split(" ")[0];
                var objectProperty = propertyInfos.FirstOrDefault(pi =>
                    pi.Name.Equals(propertyFromQueryName, StringComparison.InvariantCultureIgnoreCase));

                if (objectProperty == null)
                    continue;

                var sortingOrder = param.EndsWith(" desc") ? "descending" : "ascending";
                orderQueryBuilder.Append($"{objectProperty.Name.ToString()} {sortingOrder}, ");
            }*/
        }

    }
}
