using System;
using System.Collections.Generic;
using System.Linq;

namespace WiktionaryNET
{
    /// <summary>
    /// Search for a word in wiktionary.org and retrieve it's definition.
    /// </summary>
    public static class Wiktionary
    {
        /// <summary>
        /// Returns a word definition from wiktionary.org, given the word 
        /// and the language the searched word is in.
        /// </summary>
        /// <param name="word">Single word, without spaces</param>
        /// <param name="language">Language the searched word is in, default is English</param>
        /// <param name="jsonQuery">Will search on wiktionary.org if left empty</param>
        /// <returns>An object containing the word definition</returns>
        public static WordInfo Define(string word, string language = "en", IJsonQuery jsonQuery = null)
        {
            // Return an empty object if word is not valid
            if ((word == null) || (word == string.Empty) || (word.Length < 2))
                return new WordInfo();

            // The library user didn't choose a json source, so use the default, wiktionary.
            if (jsonQuery == null)
                jsonQuery = new WiktionaryJsonQuery();

            var jsonContent = jsonQuery.Download(word, language);

            // Check if succesfull - maybe the word does not exist
            if (jsonContent.Contains("\"missing\":\"\""))
                return new WordInfo();

            // The wiktionary page contents are contained in the "*" field
            // of the Json response string.
            jsonContent = jsonContent.Split(new string[] { "\"*\":" }, StringSplitOptions.RemoveEmptyEntries)[1];

            // Get the word definition
            var wordInfo = new WordInfo() { Word = word };
            var definitionHeaders = WordDefinitionHeader.Get(language);
            foreach (var defHeader in definitionHeaders)
            {
                var definition = wordInfo.Definition.ToList();
                var toAddDefinition = GetDefinition(defHeader, jsonContent);
                
                // Is there an entry for the current header
                if (toAddDefinition.Count() != 0)
                {
                    definition.Add(defHeader.ToUpper());
                    definition.AddRange(toAddDefinition);
                    wordInfo.Definition = definition;
                }
            }
            return wordInfo;
        }
                
        /// <summary>
        /// Remove some characters that we don't want.
        /// </summary>
        static string ClearMarkup(string input)
        {
            return input
                .Replace("[", "")
                .Replace("]", "")
                .Replace("{", "")
                .Replace("}", "")
                .Replace("\'\'\'", "")
                .Replace("\'\'", "")
                .Replace("=", "")
                .Replace(":", "")
                .Replace("*", "")
                .Replace("\\n", "");
        }

        /// <summary>
        /// Returns a clean word definition given the definition header
        /// and a wiktionary response body.
        /// </summary>
        /// <param name="definitionHeader">==Noun==, for example</param>
        /// <param name="pageContent">Json wiktionary response body (the "*" field)</param>
        /// <returns></returns>
        static IEnumerable<string> GetDefinition(string definitionHeader, string pageContent)
        {
            if (!pageContent.Contains(definitionHeader))
                return new List<string>();

            // The end of the header content is another header.
            // This usually starts with the same two characters as the previous header.
            var beginNextHeader = String.Concat(definitionHeader.Take(2));
            // Get the contents following the definition header (i.e. ==Noun==, for English)
            var definition = Utils.GetStringInBetween(definitionHeader, beginNextHeader, pageContent, false, false);

            // ===== Clear markup and other elements which are not of interest.
            // ===== These will be different for different languages.
            // ===== The order of removal is important.
            // ===== The removals will have to strike a balance for all Wiktionary 
            // ===== languages supported by the WiktionaryNET library.
            definition = Utils.ClearAllInstancesBetween("{{", "}}", definition);
            // JPGs for English Wiktionary.
            definition = Utils.ClearAllInstancesBetween("[[Image:", "]]", definition);
            // Quotations from books or other websites begin with | and end in newline (English Wiktionary)
            definition = Utils.ClearAllInstancesBetween("|", "\\n", definition);
            // Other characters which are not of interest.
            definition = Utils.ClearAllInstancesBetween("#*", "\\n", definition);
            definition = ClearMarkup(definition);
                        
            // Get each definition entry and remove the empty entries
            // One word has multiple definitions, each separated by a token.
            // For English Wiktionary the token is '#', but for German 
            // the definitions are enumerated from 1 to n.
            return definition.Split('#').ToList().Where(str => (str != string.Empty) && (str != " "));
        }
    }
}