namespace PowerFile.CommandLine.Verbs;

public interface IVerbHandler<TOptions>
{
    Task<int> HandleAsync(TOptions options, CancellationToken cancellationToken = default);
}