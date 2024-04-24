namespace TH.Services;

public interface IService<in TContext, TResult>
{
    Task<TResult> ExecuteAsync(TContext context, CancellationToken cancellationToken);
}
