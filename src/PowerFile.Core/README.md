## PowerFile.Core
This class library contains the core language of PowerFile.

It contains tools for scanning and parsing patterns to expressions, as well as an interpreter for computing a pattern's output.

This library does not handle resource creation, rather it solely deals with parsing a pattern like `Features/(Chat,Users)` into:
```
Features/Chat
Features/Users
```
For resource creation, check out the `PowerFile.Core.Templating` project in this solution.

## Scanning
The first step of turning a PowerFile pattern into a usable directory and file structure is by scanning the pattern.
We walk through each of the characters and transform them into a list of `Tokens`.

Each token has a type, such as `TokenType.LeftParenthesis`, a `Lexeme` which is the piece of text this `Token` was created from, and a `Literal`, which could be a String, Int32, etc.

Scanning essentially prepares the pattern for parsing as well as checking for unexpected characters.

## Parsing
Once we scanned the pattern, we can start parsing it. We do this by evaluating the `Tokens` and turning the pattern into a `PowerFileExpression`.

The `PowerFileExpression`s are the building blocks of a pattern.

There are multiple types of expressions:
1. Text Expression
2. Group Expression
   - Text Group Expression
   - Expandable Group Expression
   - Range Expression

### Text Expression
The Text Expression is the simplest one, it simply contains some `Text` that's returned during the evaluation

### Group Expressions
There are 3 different types of Group Expressions, all of which inherit from the base class `PowerFileExpression`.

Each group expression has a list of child Expressions, and each group handles their children differently.
#### Text Group Expression
Text Groups are intended for simply putting all the results of its children into a list during evaluation.

E.g. `Features/(Users/,Chat/)` -> `(Users,Chat)` is a Text Group, during evaluation it will simply return `[Users/, Chat/]`.

#### Expandable Group Expression
Expandable Groups are similar to Text Groups, however instead of simply returning all the children's results, they are all combined together.

Take the following pattern: `Features/Tests/(Unit,Integration)/`
This becomes the following tree:
```
TEXT GROUP:
      CHILDREN:
          EXPANDABLE GROUP:
              EXPANDERS:
                  TEXT: "Features/Tests/"
                  TEXT GROUP:
                      CHILDREN:
                          EXPANDABLE GROUP:
                              EXPANDERS:
                                  TEXT: "Unit"
                          EXPANDABLE GROUP:
                              EXPANDERS:
                                  TEXT: "Integration"
                  TEXT: "/"

```
When evaluating a pattern, the root of it will always be a `TextGroup`, inside of which an `ExpandableGroup`.
We can see that the group has 2 'Expanders':
1. Returns `[ "Features/Tests/" ]`
2. Returns `[ "Unit", "Integration" ]`
3. Returns `[ "/" ]`

When evaluating, the expression, we save the result(s) of the first child/expander `Features/Tests`.
Then, we evaluate the second expander which returns `[ "Unit", "Integration" ]`.
For all the result(s) of the second expander, add the result(s) of the first expander to it.

So we end up with:
```
Features/Tests/Unit
Features/Tests/Integration
```

Now we do the same with the last expander `[ "/" ]` and the final result becomes:
```
Features/Tests/Unit/
Features/Tests/Integration/
```

#### Range Group
The Range Group is a `TextGroup` under the hood, it can be created with 2 `char`s or 2 `int`s.
It is expressed with two square brackets, and members separated with a comma (`,`): `[0,999]`

Example: `Tests/Test_[0,10]` ->
```
Tests/Test_0
Tests/Test_1
Tests/Test_2
Tests/Test_3
...
...
Tests/Test_10
```

## Visitors
The [Visitor pattern](https://refactoring.guru/design-patterns/visitor) is used to evaluate the expression tree.
It makes it very easy to traverse it. `IPowerFileVisitor<TResult>` is the interface to represent a Visitor.

In essence, a Visitor implementation will call the base method `PowerFileExpression.Accept(IPowerFileVisitor)` on the root expression, 
generally this will be a `TextGroup` so the group would call `IPowerFileVisitor.VisitTextGroup(this)` on the visitor that was passed to `Accept`.

When the text group is evaluated, it simply calls `Accept` on all its children.