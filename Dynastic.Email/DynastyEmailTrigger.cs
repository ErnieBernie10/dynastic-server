using Dynastic.Domain.Common.Messaging;
using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using SendGrid.Helpers.Mail;
using System.Text.Json;

namespace Dynastic.Email;

public static class DynastyEmailTrigger
{
    [FunctionName("DynastyEmailTrigger")]
    public static async Task RunAsync(
        [ServiceBusTrigger("dynasty-email", Connection = "ServiceBusConnectionString")] string myQueueItem,
        [SendGrid(ApiKey = "SendGridApiKey")] IAsyncCollector<SendGridMessage> messageCollector,
        ILogger log)
    {
        var emailObject = JsonSerializer.Deserialize<InviteToDynastyMessage>(myQueueItem,
            new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
        
        var message = new SendGridMessage();
        message.AddTo(emailObject.To);
        message.AddContent("text/html", emailObject.Content);
        message.SetFrom(new EmailAddress("no-reply@dynastic.be"));
        message.SetSubject(emailObject.Subject);

        await messageCollector.AddAsync(message);

    }
}