﻿using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace WiktionaryNET
{
    /// <summary>
    /// Holds the header for the section containg the word definition for a particular language.
    /// A single word can be understood both as a verb AND a noun, for example.
    /// The ==Verb== header will contain the word definition on wiktionary, for example.
    /// </summary>
    public static class WordDefinitionHeader
    {
        // Avoid reloading the same file again and again.
        static Dictionary<string, IEnumerable<string>> list = new Dictionary<string,IEnumerable<string>>();

        /// <summary>
        /// Returns the headers for sections (in wiktionary) 
        /// containing the word definition for the selected language.
        /// </summary>
        /// <param name="language">"en" for English or "de" for German, for example</param>
        /// <returns></returns>
        public static IEnumerable<string> Get(string language)
        {
            // Language already present.
            if (list.ContainsKey(language))
                return list[language];

            // Don't know what headers to search for
            if (!File.Exists(DefinitionsPath(language)))
                return new List<string>();

            // Search in files.
            var definitionParts = File.ReadAllLines(DefinitionsPath(language)).ToList();

            list.Add(language, definitionParts);

            return definitionParts;
        }

        static string DefinitionsPath(string language)
        {
            return "../../../word_definition_headers/" + language + ".txt";
        }
    }
}
