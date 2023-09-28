# PowerFile
Quickly create complex directory structures and files from templates with easy to write patterns.

## Technical documentation
- [Core Library](src/PowerFile.Core/README.md)
- [Templates](src/PowerFile.Core.Templating/README.md)

## Patterns
You can write patterns to specify the directories and files you want to create.
Let's take a simple example:
`Features/(Orders,Chat)/`

This will become:
```
Features/Orders/
Features/Chat/
```

Resulting in the following directory structure:
```
- Features
    - Orders
    - Chat
```
As you can see the parenthesis `( )` act as a 'multiplier' to what came before it.
We call this a Group.

The comma `,` indicates a separate expression, this can also be found in the root pattern
```
Features/,Tests/ or (Features/,Tests/)

- Features/
- Tests/
```
Or slightly more complex:
```
Features/(Orders,Users)/,Tests/(Orders,Users)/

- Features
    - Orders
    - Users
- Tests
    - Orders
    - Users
```
This pattern could also be rewritten asj:
```
(Features,Test)/(Orders,Users)/
```
The directory separator can also be put in a group`/`
```
(Features/,Test/)(Orders/,Users/)
```
All three patterns above have the same result.

Let's try to create some files now.
```
Features/(Chat,Users)/(Commands,Queries)/README.md

- Features
    - Chat
        - Commands
            - README.md
        - Queries
            - README.md
    - Users
        - Commands
            - README.md
        - Queries
            - README.md
```

With a simple **50 character** pattern, we manage to create 7 directories and 4 files.

### More pattern examples
#### Expansion
Create a new directory 'Features', that contains a 'Users', 'Chat', and 'Orders' directory, each of these need the directories 'Commands' and 'Queries'.

This can be done with a simple pattern:
```
Features/(Orders,Users,Chat)/(Commands,Queries)/

> Features/Orders/Commands/
> Features/Orders/Queries/
> Features/Users/Commands/
> Features/Users/Queries/
> Features/Chat/Commands/
> Features/Chat/Queries/
```
---
The same structure but only 'Users' and 'Chat' should contain 'Commands' and 'Queries':
```
Features/(Orders,(Users,Chat)/(Commands,Queries))/

> Features/Orders
> Features/Orders
> Features/Users/Commands/
> Features/Users/Queries/
> Features/Chat/Commands/
> Features/Chat/Queries/
```
The pattern can be dissected a bit to make it a little more clear
```
(
    (Features/),
    (
        (
           (Orders)
        ),
        (
            (Users,Chat),
            (/),
            (Commands, Queries)
        ),
        (
            (/)
        )
    ),
    (/)
)
```
---
#### Ranges
A directory `Tests`, inside of which there are 10 numbered folders:
```
Tests/Test_[1,10]

> Tests/Test_1
> Tests/Test_2
> Tests/Test_3
> Tests/Test_4
> ...
> Tests/Test_10
```
---
## Templates
[WIP]