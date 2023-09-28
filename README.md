# PowerFile
Quickly create complex directory structures and files from templates with easy to write patterns.

## Documentation
- [Core Library](src/PowerFile.Core/README.md)
- [Templates](src/PowerFile.Core.Templating/README.md)

## Examples
Create a new directory 'Features', that contains a 'Users', 'Chat', and 'Orders' directory, each of these need the directories 'Commands' and 'Queries'.

This can be done with a simple pattern:
```
Features/(Orders, Users,Chat)/(Commands,Queries)/

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
Features/(Orders, (Users,Chat)/(Commands,Queries))/

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
3. A directory `Tests`, inside of which there are 10 numbered folders:
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
