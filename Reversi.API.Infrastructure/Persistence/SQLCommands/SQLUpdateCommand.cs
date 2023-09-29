using System;
using System.Collections.Generic;
using System.Text;

namespace Reversi.API.Infrastructure.Persistence.SQLCommands
{
    public class SQLUpdateCommand : SQLBaseCommand
    {
        public SQLUpdateCommand Update(string tableName)
        {
            Query = $"UPDATE \"{tableName}\"";
            return this;
        }

        public SQLUpdateCommand Set(string[] columns, object[] values)
        {
            if (columns.Length != values.Length)
                throw new Exception("The amount of columns and given values do not match.");

            Query += " SET ";

            for (int i = 0; i < columns.Length; i++)
            {
                Parameters.Add($"@{columns[i]}", values[i]);
                Query += $"{columns[i]} = @{columns[i]}, ";
            }

            var trimEnd = Query.TrimEnd(' ', ',');

            Query = $"{trimEnd}";
            return this;
        }
    }
}
