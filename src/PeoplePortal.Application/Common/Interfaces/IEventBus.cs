namespace PeoplePortal.Application.Common.Interfaces;

public interface IEventBus
{
    Task PublishAsync<T>(string subject, T message, CancellationToken cancellationToken = default);
}
