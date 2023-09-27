namespace PowerFile.CommandLine.Verbs;

public interface IVerbHandler
{
    Task<int> HandleAsync(CancellationToken cancellationToken = default);
}

public abstract class VerbHandler<TOptions>(TOptions options) : IVerbHandler
{
    private TOptions Options { get; } = options;

    public Task<int> HandleAsync(CancellationToken cancellationToken = default)
    {
        return HandleAsync(Options, cancellationToken);
    }

    protected abstract Task<int> HandleAsync(TOptions options, CancellationToken cancellationToken = default);
}