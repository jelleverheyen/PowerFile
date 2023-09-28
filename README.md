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
PowerFile has support for templates, it'll automatically try to match a template to the files you try to create, if none are found it will create an empty file.

### Defining a template
Inside the templates directory for your OS, create a new file and add FrontMatter to define its metadata.
Let's take a C# interface as an example, it can have any file name you like
```csharp
---
name: C# Interface
Description: Interface template for C#
prefix: I
keywords: null
suffix: .cs
tags: csharp interface
---
namespace $NAMESPACE$
        
public interface $FILE_NAME$
{
    $
}    
```

When you run `powerfile reload`, it will index all the templates in the `config/templates` folder, based on the frontmatter that's defined inside the file.

In this case the template will match any file name that starts with `I` and ends with `.cs`.

Let's give it a try
```
> powerfile create "Abstractions/(IParser.cs,ITemplateStore.cs)" 
```

This will create a directory `Abstractions` with 2 files, IParser.cs and ITemplateStore.cs with the interface template we defined above

### Conflicts
It's possible that multiple templates are found for your file name.
PowerFile tries to match the best template based on how many fields (Prefixes, Suffixes, and Keywords) are matched, as well as the length of the match.
PowerFile will use the template it deems to be the most appropriate.

E.g. if we take our interface template from above, and now add a new template for a C# class.
We'll set the suffix to `.cs` for this template. This means that both the `interface` and `class` templates will be matched initially.

This is where the template engine will compute a score.
The interface template matches multiple fields (gives the most score) and also matches more text than the class template, so the `interface` template will be used.



### Directories
**Templates**
- **Windows**:
  - `%APPDATA%/PowerFile/Config/templates`
- **MacOS**:
  - `~/Library/Preferences/templates`
- **Linux**:
  - Environment Variable `XDG_CONFIG_HOME/PowerFile/templates`
  - `~/.config`

**Index**
- **Windows**:
    - `%LOCALAPPDATA%/PowerFile/Cache/templates.index`
- **MacOS**:
    - `~/Library/Caches/PowerFile/templates.index`
- **Linux**:
    - Environment Variable `$XDC_CACHE_HOME/PowerFile/templates.index`
    - `~/.cache/PowerFile/templates.index`

