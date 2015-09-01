# PeoplecodeDecoder
C# library for decoding peoplecode bytecode into a textual representation.

#Usage
 In order to use this library you must have the binary peoplecode bytecode (stored in PSPCMPROG) as well as a JSON representation of the references as stored in PSPCMNAME.

 References should look like this
```json
[
  {
    "NAMENUM": 1,
    "RECNAME": " ",
    "REFNAME": " ",
    "PACKAGEROOT": " ",
    "QUALIFYPATH": " "
  },
  {
    "NAMENUM": 2,
    "RECNAME": "PACKAGE",
    "REFNAME": "ROWSET",
    "PACKAGEROOT": "Rowset",
    "QUALIFYPATH": "Rowset"
  },
  {
    "NAMENUM": 3,
    "RECNAME": "RECORD",
    "REFNAME": "UM_GA_DIMENSION",
    "PACKAGEROOT": " ",
    "QUALIFYPATH": " "
  },
  {
    "NAMENUM": 4,
    "RECNAME": "PACKAGE",
    "REFNAME": "RECORD",
    "PACKAGEROOT": "Record",
    "QUALIFYPATH": "Record"
  }
]
```

Once both of these items are available, you can invoke the library like this:
```c#
ProgramElement p = Parser.ParsePPC("path\to\binary.pcode", "path\to\references.json");
```
To get the textual representation simply call .Write() on the ProgramElement that is returned and pass it in a StringBuilder with which it should build out the text.

#Expectations
PeopleCodeDecoder aims to provide a decoding library that produces the textual representation in a synactically identical manner. What this means is that the decoded version of the peoplecode should execute exactly as the original, although it may not be character for character exact. Some differences may include stray semicolons. For example

```
If &variable = "Y" Then;
   DoSomething();
End-If;
```

The semicolon at the end of the "then" is not actually needed, and therefore the results of decoding will omit that semicolon.

Another example is a double semicolon

```
&myVariable = "123";;
```

The extra semicolon while perfectly valid serves no purpose and will be omitted from the output of the decoder.



