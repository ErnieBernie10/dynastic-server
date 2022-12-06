namespace Dynastic.Application.Common.Interfaces;

public interface IServiceBus
{
    Task SendMessage(string queueName, string message);
    Task<string> ReceiveMessage(string queueName);
}

