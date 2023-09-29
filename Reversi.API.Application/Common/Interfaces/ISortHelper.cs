using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Reversi.API.Application.Common.Interfaces
{
    public interface ISortHelper<T>
    {
        IQueryable<T> ApplySort(IQueryable<T> entities, string orderByQueryString);
    }
}
