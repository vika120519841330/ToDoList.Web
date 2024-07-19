using System;

public class Repository<T> : IRepository<T>
{
    private readonly IDbContextFactory<SpeditionContext> contextFactory;
    private string message;

    public Repository(IDbContextFactory<SpeditionContext> contextFactory) => this.contextFactory = contextFactory;

    public string Message => message ?? string.Empty;

    private SpeditionContext context;

    private SpeditionContext Context => context == null || string.IsNullOrEmpty(context?.ContextId.InstanceId.ToString()) ? context = contextFactory.CreateDbContext() : context;

    private SpeditionContext contextAsync;

    private SpeditionContext ContextAsync
    {
        get
        {
            if (contextAsync == null || string.IsNullOrEmpty(contextAsync?.ContextId.InstanceId.ToString()))
            {
                var f = async () => await contextFactory?.CreateDbContextAsync();
                return contextAsync = f.Invoke().GetAwaiter().GetResult();
            }
            else
            {
                return contextAsync;
            }
        }
    }

    private DbSet<T> DbSet => Context?.Set<T>();

    private DbSet<T> DbSetAsync => ContextAsync?.Set<T>();


}
