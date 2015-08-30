# PeoplecodeDecoder
C# library for decoding peoplecode bytecode into a textual representation.

#Usage
 In order to use this library you must have the binary peoplecode bytecode (stored in PSPCMPROG) as well as a JSON representation of the references as stored in PCPCMNAME.

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
To get the textual representation simply call .ToString() on the ProgramElement that is returned.

#Parse Options
There are a variety of parsing options that you can opt into using

```c#
            ParseOptions opts = new ParseOptions();
            opts.AlphabetizeMethodDeclarations = true;
            opts.MatchMethodDeclarationOrder = true;
            opts.PairGetSets = true;
```

ParseOptions can be passed as the 3rd parameter to Parser.ParsePPC().

##AlphabetizeMethodDeclarations
This option will cause the method declarations inside class/end-class; to be alphabetized. Sorting takes place in the public/protected/private sections seperately.

##MatchMethodDeclarationOrder
This option will cause the implementations of methods to come in the same order they are declared. This is especially useful if you have AlphabetizeMethodDeclartions enabled as well.

##PairGetSets
This option will cause all get/set pairs to be put together. This setting will emit all get/set pairs, then any leftover gets and finally any leftover sets.
