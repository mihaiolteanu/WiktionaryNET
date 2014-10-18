The definition of a word, in the response Json from wiktionary, is contained in a header.

In English, for example, the header name is the part of the speech the word belongs to 
(i.e. Verb, Noun, etc.). The same word can belong to multiple parts of speech. For example,
"can" is a Verb or a Noun. These headers are sourounded by `=`, like ==Noun==, for example

In German, on the other hand, the header name containing the word definition is "Bedeutungen", 
which literally translates to "meanings". This is sourounded by `{`, like this: {{Bedeutungen}}.

Other languages will have different header sections for the word meanining sourounded by either
`=`, `{` or other characters.

We need the names of these headings to extract all the definitions for the searched word.
