namespace PowerFile.CommandLine.Verbs.Templates;

public class TemplatesVerbHandler(TemplatesVerbOptions options) : VerbHandler<TemplatesVerbOptions>(options)
{
    protected override Task<int> HandleAsync(TemplatesVerbOptions options, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}