using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Reversi.API.Application.Common.Interfaces;

namespace Reversi.API.Application
{
    public class ItemList<TItem> : ISQLItem<TItem>
    {
        public override void Add(List<Dictionary<string, object>> items)
        {
            foreach (var item in items)
            {
                var newItem = Activator.CreateInstance(typeof(TItem));

                foreach (KeyValuePair<string, object> KV in item)
                {
                    foreach (var prop in newItem.GetType().GetProperties())
                    {
                        if (prop.Name.Equals(KV.Key))
                        {
                            prop.SetValue(newItem, KV.Value);
                        }
                    }
                }

                if (newItem != null)
                    Items.Add((TItem)newItem);
            }
        }
    }
}
