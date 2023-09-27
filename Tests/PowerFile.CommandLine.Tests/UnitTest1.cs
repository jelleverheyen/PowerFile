using PowerFile.Core.Parsing;
using PowerFile.Core.Visitors;
using PowerFileScanner = PowerFile.Core.Scanning.Scanner;

namespace PowerFile.CommandLine.Tests;

public class UnitTest1
{
    [Fact]
    public void Test1()
    {
        // Features/(Queries/(Get,GetAll)/,Commands/(Create,Update,Delete)/
        var scanner = CreateScanner("Features/(Test,Potato)/[0,9]");
        var expression = new PowerFileParser(scanner.Scan()).Parse();
        var printer = new PowerFileExpressionPrinter().Print(expression);

        int i = 0;
    }
    
    private static PowerFileScanner CreateScanner(string pattern)
    {
        return new PowerFileScanner(pattern);
    }
}