using Azure.Data.Tables;
using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureServices((context, services) =>
    {
        string storageConnection =
            context.Configuration["AzureWebJobsStorage"]
            ?? throw new InvalidOperationException(
                "AzureWebJobsStorage is missing.");

        services.AddSingleton(
            new TableServiceClient(storageConnection));

        services.AddSingleton(
            new QueueClient(
                storageConnection,
                "irrigation-commands",
                new QueueClientOptions
                {
                    MessageEncoding = QueueMessageEncoding.Base64
                }));
    })
    .Build();

host.Run();