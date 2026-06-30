using NATS.Client.Core;
using NATS.Client.JetStream;
using NATS.Client.JetStream.Models;
using NATS.Net;
using PeoplePortal.Application.Common.Interfaces;

namespace PeoplePortal.Infrastructure.Messaging;

public class NatsEventBus : IEventBus, IAsyncDisposable
{
    private readonly NatsConnection _connection;
    private readonly INatsJSContext _jsContext;

    public NatsEventBus(string url)
    {
        _connection = new NatsConnection(new NatsOpts { Url = url });
        _jsContext = _connection.CreateJetStreamContext();
    }

    public async Task PublishAsync<T>(string subject, T message, CancellationToken cancellationToken = default)
    {
        await _jsContext.CreateOrUpdateStreamAsync(new StreamConfig
        {
            Name = "peopleportal-events",
            Subjects = ["hr.>", "employee.>"]
        }, cancellationToken);

        await _jsContext.PublishAsync(subject, message, serializer: NatsClientDefaultSerializer<T>.Default, cancellationToken: cancellationToken);
    }

    public async ValueTask DisposeAsync()
    {
        await _connection.DisposeAsync();
    }
}
