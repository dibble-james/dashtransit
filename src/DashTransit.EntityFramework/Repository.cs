namespace DashTransit.EntityFramework;

using Ardalis.Specification.EntityFrameworkCore;

public class Repository<T> : RepositoryBase<T>
    where T : class
{
    public Repository(DashTransitContext context)
        : base(context)
    {       
    }
}