using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NATS.Client.Core;
using NATS.Net;

namespace PeoplePortal.Infrastructure.Messaging;

public class EventConsumerService : BackgroundService
{
    private readonly ILogger<EventConsumerService> _logger;
    private readonly NatsConnection _connection;

    public EventConsumerService(ILogger<EventConsumerService> logger, NatsConnection connection)
    {
        _logger = logger;
        _connection = connection;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var jsContext = _connection.CreateJetStreamContext();
        var consumer = await jsContext.CreateOrderedConsumerAsync("peopleportal-events", cancellationToken: stoppingToken);

        await foreach (var msg in consumer.ConsumeAsync<string>(NatsClientDefaultSerializer<string>.Default, cancellationToken: stoppingToken))
        {
            _logger.LogInformation("Received event on {Subject}: {Data}", msg.Subject, msg.Data);
            await msg.AckAsync(cancellationToken: stoppingToken);
        }
    }
}
