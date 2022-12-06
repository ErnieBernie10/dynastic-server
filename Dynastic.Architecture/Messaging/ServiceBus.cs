using Azure.Messaging.ServiceBus;
using Dynastic.Application.Common.Interfaces;

namespace Dynastic.Infrastructure.Messaging;

public class ServiceBus : IServiceBus
{
    private readonly ServiceBusClient _serviceBusClient;

    private readonly Dictionary<string, ServiceBusSender> _senders = new();

    public ServiceBus(ServiceBusClient serviceBusClient)
    {
        _serviceBusClient = serviceBusClient;
    }
    
    public async Task SendMessage(string queueName, string message)
    {
        var sender = GetOrCreateSender(queueName);

        await sender.SendMessageAsync(new ServiceBusMessage(message));
    }

    public Task<string> ReceiveMessage(string queueName)
    {
        // Implement if it ever becomes necessary. We should try to use azure functions always to receive messages
        throw new NotImplementedException();
    }

    private ServiceBusSender GetOrCreateSender(string queueName)
    {
        if (_senders.TryGetValue(queueName, out var sender))
        {
            return sender;
        }

        sender = _serviceBusClient.CreateSender(queueName);
        
        _senders.Add(queueName, sender);

        return sender;
    }
}