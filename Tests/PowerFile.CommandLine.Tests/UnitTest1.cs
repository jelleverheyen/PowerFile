using PowerFile.Core.Parsing;
using PowerFile.Core.Scanning;
using PowerFile.Core.Visitors;

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
    
    private static Scanner CreateScanner(string pattern)
    {
        return new Scanner(pattern);
    }
}