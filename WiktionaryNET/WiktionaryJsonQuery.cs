using System;
using System.Net;

namespace WiktionaryNET
{
    /// <summary>
    /// Get the response from Wiktionary in Json format
    /// </summary>
    class WiktionaryJsonQuery : IJsonQuery
    {
        // Wiktionary base API - must add the language and the query to it.
        string baseUrl = @"wiktionary.org/w/api.php";

        /// <summary>
        /// Build a Wiktionary Json query based on a given word and language.
        /// </summary>
        /// <param name="word">The word to search for in the Json query</param>
        /// <param name="language">Language of Wiktionary to search in</param>
        /// <returns>Json response</returns>
        public string Download(string word, string language)
        {
            baseUrl = language + "." + baseUrl;

            // Build the url used to get the word definition from wiktionary.
            AddQuery("format=json");
            AddQuery("action=query");
            AddQuery("prop=revisions");
            AddQuery("rvprop=content");
            AddQuery("titles=" + word);

            // Download the Json response string
            using (var webClient = new WebClient())
                return webClient.DownloadString(baseUrl);
        }

        /// <summary>
        /// Append query parameters to the final url.
        /// </summary>
        void AddQuery(string query)
        {
            var url = new UriBuilder(baseUrl);
            var isQuery = url.Query != "" ? true : false;
            // Is there already a query.
            if (isQuery)
            {
                // Append the query to the original url
                baseUrl = url.Uri + "&" + query;
                url = new UriBuilder(baseUrl);
            }
            else
                url.Query = query;

            baseUrl = url.Uri.ToString();
        }
    }
}
