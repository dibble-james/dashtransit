namespace DashTransit.Core.Application.Queries;

using Ardalis.Specification;
using DashTransit.Core.Domain;

public class SearchMessages : Specification<Message>
{
    public SearchMessages()
    {
        Query.Skip(0).Take(100);
    }
}