using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace Reversi.API.Infrastructure.Persistence.SQLCommands
{
    public class SQLBaseCommand
    {
        private const string _CONNECTION_STRING = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ReversiDbRestApi;Integrated Security=True;";

        protected SqlConnection Connection { get; set; }
        protected SqlCommand Command { get; set; }

        public Dictionary<string, object> Parameters = new Dictionary<string, object>();

        public string Query { get; set; }

        public SQLBaseCommand Where(string column, string sqloperator, object value)
        {
            Query += $" WHERE {column} {sqloperator} {value}";
            return this;
        }

        public SQLBaseCommand And(string column, string sqloperator, object value)
        {
            Query += $" AND {column} {sqloperator} '{value}'";
            return this;
        }

        public SQLBaseCommand Or(string column, string sqloperator, object value)
        {
            Query += $" OR {column} {sqloperator} '{value}'";
            return this;
        }

        public SQLBaseCommand Build()
        {
            Connection = new SqlConnection(_CONNECTION_STRING);

            Command = new SqlCommand(Query, Connection);
            Console.WriteLine(Command.CommandText);
            foreach (var parameter in Parameters)
            {
                Command.Parameters.AddWithValue(parameter.Key, parameter.Value);
            }
            return this;
        }

        /*public virtual async Task<List<TItem>> Execute<TItem>()*/
        public virtual async Task<List<Dictionary<string, object>>> Execute()
        {
            await Connection.OpenAsync();

            var affectedRows = await Command.ExecuteNonQueryAsync();

            await Command.DisposeAsync();
            await Connection.CloseAsync();

/*            return new List<TItem> { (TItem)Convert.ChangeType(affectedRows, typeof(TItem)) };
*/
            return new List<Dictionary<string, object>>();
        }
    }
}
