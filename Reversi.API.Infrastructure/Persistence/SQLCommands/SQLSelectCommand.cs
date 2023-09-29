using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reversi.API.Application;
using Reversi.API.Application.Common.Interfaces;

namespace Reversi.API.Infrastructure.Persistence.SQLCommands
{
    public class SQLSelectCommand : SQLBaseCommand
    {
        public SqlDataReader Reader { get; set; }

        public SQLSelectCommand Select()
        {
            Query = "SELECT *";
            return this;
        }

        public SQLSelectCommand From(string tableName)
        {
            Query += $" FROM \"{tableName}\"";
            return this;
        }

        public override async Task<T> Execute<T, U>()
        /*public override async Task<List<TItem>> Execute<TItem>()*/
        {   
            await Connection.OpenAsync();

            Reader = await Command.ExecuteReaderAsync();
            var result = new List<Dictionary<string, object>>();

            while (Reader.Read())
            {
                var item = Enumerable.Range(0, Reader.FieldCount)
                    .ToDictionary(Reader.GetName, Reader.GetValue);

                var keys = new List<string>(item.Keys);

                foreach (var key in keys.Where(key => item[key].Equals(DBNull.Value)))
                {
                    item[key] = null;
                }
               

                result.Add(item);
            }

            T t = Activator.CreateInstance<T>();
            t.Add(result);

            await Reader.CloseAsync();
            await Command.DisposeAsync();
            await Connection.CloseAsync();

            return t;
        }
    }
}
