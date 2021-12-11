namespace DashTransit.Core.Domain;

public abstract record Actor(EndpointId Endpoint)
{
    public static Actor FromRawAuditData(IRawAuditData audit)
    {
        return audit.ContextType.ToLowerInvariant() switch
        {
            "send" => new Sender(EndpointId.From(new Uri(audit.SourceAddress))),
            "publish" => new Publisher(EndpointId.From(new Uri(audit.SourceAddress))),
            "consume" => new Consumer(EndpointId.From(new Uri(audit.DestinationAddress))),
            _ => throw new InvalidOperationException("Unknown context type for Actor"),
        };
    }
}

public record Sender(EndpointId Endpoint) : Actor(Endpoint);

public record Publisher(EndpointId Endpoint) : Actor(Endpoint);

public record Consumer(EndpointId Endpoint) : Actor(Endpoint);