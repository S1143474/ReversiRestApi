using System;
using System.Collections.Generic;
using System.Text;

namespace Reversi.API.Application.Common.Interfaces
{
    public abstract class ISQLItem<T>
    {
        public List<T> Items = new List<T>();
        public virtual void Add(List<Dictionary<string, object>> items) {}

        public List<T> Get()
        {
            return Items;
        }
    }
}
