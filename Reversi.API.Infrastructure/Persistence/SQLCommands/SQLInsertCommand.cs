using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Reversi.API.Application.Common.Mappings;

namespace Reversi.API.Infrastructure.Persistence.SQLCommands
{
    public class SQLInsertCommand : SQLBaseCommand
    {
        private const int _MAX_STRING_LENGTH = 16;
        private int AmountColumns { get; set; }

        public SQLInsertCommand Insert(params string[] columns)
        {
            Query = "INSERT {0} ";
            AmountColumns = columns.Length;

            if (columns.Length == 0) return this;

            Query += "(";
            foreach (var column in columns)
            {
                Query += $"{column}, ";
            }
            Query = Query.TrimEnd(' ', ',');

            Query += ") ";

            return this;
        }

        public SQLInsertCommand Into(string tableName)
        {
            Query = string.Format(Query, $"INTO \"{tableName}\"");
            return this;
        }

        public SQLInsertCommand Values(params object[] values)
        {
            if (values.Length != AmountColumns && AmountColumns != 0)
                throw new Exception("The amount of given columns does not match the amount of given values.");

            /* throw new ColumnAndValuesNotEqualExceptions("The amount of given columns does not match the amount of given values.");
            */
            Query += " VALUES(";
            char[] delimiterChars = { ' ', '-', '=' };

            foreach (var value in values)
            {
                var scalar = value.ToString().Split(delimiterChars)[0];
                var limitedScalar = scalar.LimitLength(_MAX_STRING_LENGTH);

                Query += $"@{limitedScalar}, ";
                Parameters.Add($"@{limitedScalar}", value);
            }
            Query = Query.TrimEnd(' ', ',');
            Query += ");";
            /*Console.WriteLine(Query);*/
            return this;
        }
    }
}
