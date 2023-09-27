Quickly create complex directory structures and files from templates with easy to write patterns.

## Example
1. Create a new directory 'Features', that contains a 'Users', 'Chat', and 'Orders' directory, each of these need the directories 'Commands' and 'Queries'
This can be done with a simple pattern:
```
Features/(Users,Chat,Orders)/(Commands,Queries)
```
2. The same structure but only 'Users' and 'Chat' should contain 'Commands' and 'Queries':
```
Features/((Users,Chat)/(Commands,Queries),Orders)/
```
This becomes a little more complicated but let's dissect it quickly
```
(Features/),
(
    (
        (Users,Chat),
        (/),
        (Commands, Queries)
    ),
    (
        Orders
    )
),
(/)
```
The internal expression tree would look like this:
```
TEXT GROUP:
     CHILDREN:
         EXPANDABLE GROUP:
             EXPANDERS:
                 TEXT: "Features/"
                 TEXT GROUP:
                     CHILDREN:
                         EXPANDABLE GROUP:
                             EXPANDERS:
                                 TEXT GROUP:
                                     CHILDREN:
                                         EXPANDABLE GROUP:
                                             EXPANDERS:
                                                 TEXT: "Users"
                                         EXPANDABLE GROUP:
                                             EXPANDERS:
                                                 TEXT: " Chat"
                                 TEXT: "/"
                                 TEXT GROUP:
                                     CHILDREN:
                                         EXPANDABLE GROUP:
                                             EXPANDERS:
                                                 TEXT: "Commands"
                                         EXPANDABLE GROUP:
                                             EXPANDERS:
                                                 TEXT: " Queries"
                         EXPANDABLE GROUP:
                             EXPANDERS:
                                 TEXT: " Orders"
                 TEXT: "/"

```