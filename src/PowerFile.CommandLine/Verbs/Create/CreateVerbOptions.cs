using CommandLine;

namespace PowerFile.CommandLine.Verbs.Create;

[Verb("create", isDefault: true, HelpText = "Creates resources from the given pattern")]
public class CreateVerbOptions
{
    [Value(0, Required = true, HelpText = "The pattern to be evaluated")]
    public string Pattern { get; set; } = string.Empty;
    
    [Option('l', "limit", Required = false, HelpText = "Set the limit of resources to be created, aborts if resource count exceeds the limit")]
    public int Limit { get; set; }
    
    [Option('d', "debug", Required = false, HelpText = "Pretty prints the internal tree")]
    public bool Debug { get; set; }
    
    [Option("tags", Required = false, Separator = ',', HelpText = "List of comma-separated values that will filter the templates by the given tags")]
    public IEnumerable<string> Tags { get; set; }
}