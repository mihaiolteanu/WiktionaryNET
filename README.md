Deprecated
=============

Wiktionary library for .NET.

Retrieves the definition of the given word from wiktionary.org.

Installation
------------
Use the NuGet Package Manager from Visual Studio, or

*PM> Install-Package WiktionaryNET*

Usage
------------
```c#
static void Main(string[] args)
{
    var word = Wiktionary.Define("house", "en");
    foreach(var def in word.Definition)
        Console.WriteLine(def);
}
```
