using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public override async Task<List<Dictionary<string, object>>> Execute()
        /*public override async Task<List<TItem>> Execute<TItem>()*/

        {
            await Connection.OpenAsync();

            Reader = await Command.ExecuteReaderAsync();
            var result = new List<Dictionary<string, object>>();

            while (Reader.Read())
            {
                var item = Enumerable.Range(0, Reader.FieldCount)
                    .ToDictionary(Reader.GetName, Reader.GetValue);

                result.Add(item);
            }

            await Reader.CloseAsync();
            await Command.DisposeAsync();
            await Connection.CloseAsync();

            return result;
        }
    }
}
