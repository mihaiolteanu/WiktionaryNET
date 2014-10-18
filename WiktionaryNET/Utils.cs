using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WiktionaryNET
{
    public static class Utils
    {
        /// <summary>
        /// Parse a string and return the contents between two tokens.
        /// Taken from:
        /// http://timtrott.co.uk/string-in-between/
        /// </summary>
        public static string GetStringInBetween(string strBegin, string strEnd, string strSource, bool includeBegin, bool includeEnd)
        {
            string[] result = { string.Empty, string.Empty };
            int iIndexOfBegin = strSource.IndexOf(strBegin);

            if (iIndexOfBegin != -1)
            {
                // include the Begin string if desired 
                if (includeBegin)
                    iIndexOfBegin -= strBegin.Length;

                strSource = strSource.Substring(iIndexOfBegin + strBegin.Length);

                int iEnd = strSource.IndexOf(strEnd);
                if (iEnd != -1)
                {
                    // include the End string if desired 
                    if (includeEnd)
                        iEnd += strEnd.Length;
                    result[0] = strSource.Substring(0, iEnd);
                    // advance beyond this segment 
                    if (iEnd + strEnd.Length < strSource.Length)
                        result[1] = strSource.Substring(iEnd + strEnd.Length);
                }
            }
            else
                // stay where we are 
                result[1] = strSource;
            return result[0];
        }

        /// <summary>
        /// Returns the original string without the characters specified
        /// and without the string between those characters as well
        /// </summary>
        public static string ClearAllInstancesBetween(string start, string end, string source)
        {
            while (true)
            {
                var instance = Utils.GetStringInBetween(start, end, source, true, true);
                if (string.IsNullOrEmpty(instance)) break;
                source = source.Replace(instance, string.Empty);
            }
            return source;
        }
    }
}
