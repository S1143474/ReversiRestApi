using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using Reversi.API.Application.Common.Interfaces;

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
            if (value == null)
            {
                Query += $" WHERE {column} {sqloperator} NULL";
                return this;
            }
            Query += $" WHERE {column} {sqloperator} '{value}'";
            return this;
        }

        public SQLBaseCommand And(string column, string sqloperator, object value)
        {
            if (value == null)
            {
                Query += $" AND {column} {sqloperator} NULL";
                return this;
            }

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
            foreach (var parameter in Parameters)
            {
                Command.Parameters.AddWithValue(parameter.Key, parameter.Value);
            }
            Console.WriteLine(Command.CommandText);

            return this;
        }

        /*public virtual async Task<List<TItem>> Execute<TItem>()*/
        public async Task<int> Execute()
        {
            await Connection.OpenAsync();

            var affectedRows = await Command.ExecuteNonQueryAsync();

            await Command.DisposeAsync();
            await Connection.CloseAsync();

            return affectedRows;
        }

        public virtual async Task<T> Execute<T, U>() where T : ISQLItem<U>
        {
            await Connection.OpenAsync();

            var affectedRows = await Command.ExecuteNonQueryAsync();

            await Command.DisposeAsync();
            await Connection.CloseAsync();

/*            return new List<TItem> { (TItem)Convert.ChangeType(affectedRows, typeof(TItem)) };
*/
            T t = Activator.CreateInstance<T>();

            return t;
        }
    }
}
