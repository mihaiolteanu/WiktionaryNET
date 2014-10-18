using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WiktionaryNET
{
    public class WordInfo
    {
        public string Word { get; set; }

        /// <summary>
        /// The dictionary definition of the Word.
        /// </summary>
        public IEnumerable<string> Definition { get; set; }

        public IEnumerable<string> Synonyms { get; set; }

        /// <summary>
        /// Verb, Noun, Adjective, etc.
        /// </summary>
        public string WordType { get; set; }

        public string Etymology { get; set; }

        public string AlternativeForms { get; set; }

        public string Translation { get; set; }

        /// <summary>
        /// Build an empty object.
        /// </summary>
        public WordInfo()
        {
            Word = string.Empty;
            Definition = new List<string>();
            Synonyms = new List<string>();
            WordType = string.Empty;
            Etymology = string.Empty;
            AlternativeForms = string.Empty;
            Translation = string.Empty;
        }
    }
}