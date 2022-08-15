# csharp-data-oriented
Exploring data oriented and FP principles from c#

## Seq

Convert any object into a list of lists, graph

```
Seq: (arg: T) -> Seq
```

Example:

```csharp
var person = new {
    Name = new {
        Given = new[] { "Glen", "Ruben" },
        Family = "Rodriguez" },
    DoB = new DateTime(2011, 10, 3),
    Addresses = new[] { 
        new { 
            Lines = new[] { "184 #42301, esq 423" },
            PostalCode = "18100",
            Period = new { Start = new DateTime(2019, 04, 17) } } } };

var seqPerson = Seq(person);
```

```json
[["Name", [
     ["Given", [
         ["0", ["Glen"]], 
         ["1", ["Ruben"]]]], 
     ["Family", ["Rodriguez"]]]], 
 ["DoB", ["2011-10-03T00:00:00"]], 
 ["Addresses", [
     ["0", [
         ["Lines", [
             ["0", ["184 #42301, esq 423"]]]], 
         ["PostalCode", [ 
             "18100"]], 
         ["Period", [
             ["Start", ["2019-04-17T00:00:00"]]]]]]]]]
```

## Get

```
Get: <T>(seq: Seq, path: string[]) -> T
```

Resolves a part of the graph given a path

Example:

```csharp
var personSeq = Seq(person);

personSeq.Get<string>(
    path: new[] { "Name", "Family" }); // => "Rodriguez"

personSeq.Get<string>(
    path: new[] { "Addresses", "0", "Lines", "0" }); // => "184 #42301, esq 423"
```
