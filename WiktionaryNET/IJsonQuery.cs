namespace WiktionaryNET
{
    /// <summary>
    /// Returns a json string containing the word info
    /// </summary>
    public interface IJsonQuery
    {
        string Download(string word, string language);
    }
}
