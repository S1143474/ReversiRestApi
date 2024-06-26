﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Reversi.API.Application.Common.Mappings
{
    public static class StringExtensions
    {
        /// <summary>
        /// Method that limits the length of text to a defined length.
        /// </summary>
        /// <param name="source">The source text.</param>
        /// <param name="maxLength">The maximum limit of the string to return.</param>
        public static string LimitLength(this string source, int maxLength)
        {
            if (source.Length <= maxLength || maxLength < 0)
            {
                return source;
            }

            return source.Substring(0, maxLength);
        }
    }
}
