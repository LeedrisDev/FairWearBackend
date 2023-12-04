using System.Diagnostics;
using System.Text;
using BrandAndProduct.Service.Business.IntegrationEventBusiness;
using Confluent.Kafka;

namespace BrandAndProduct.Service.Services;

public class IntegrationEventSenderService : BackgroundService, IIntegrationEventSenderService
{
    private readonly IIntegrationEventBusiness _integrationEventBusiness;
    private readonly IProducer<string, string> _producer;
    private readonly IServiceScopeFactory _scopeFactory;
    private CancellationTokenSource _wakeupCancelationTokenSource = new CancellationTokenSource();

    public IntegrationEventSenderService(IServiceScopeFactory scopeFactory,
        IIntegrationEventBusiness integrationEventBusiness, IProducer<string, string> producer)
    {
        _scopeFactory = scopeFactory;
        _integrationEventBusiness = integrationEventBusiness;
        _producer = producer;
    }

    public void StartPublishingOutstandingIntegrationEvents()
    {
        _wakeupCancelationTokenSource.Cancel();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await PublishOutstandingIntegrationEvents(stoppingToken);
        }
    }

    private async Task PublishOutstandingIntegrationEvents(CancellationToken stoppingToken)
    {
        try
        {
            var config = new ProducerConfig
            {
                BootstrapServers = "host1:9092",
            };

            while (!stoppingToken.IsCancellationRequested)
            {
                var response = await _integrationEventBusiness.GetAllIntegrationEventsAsync();
                var events = response.Object.OrderBy(o => o.Id).ToList();
                Debug.WriteLine("[PUBLISH] HERE");
                foreach (var e in events)
                {
                    var body = Encoding.UTF8.GetBytes(e.Data);
                    _producer.Produce("products", new Message<string, string> { Key = e.Event, Value = e.Data },
                        deliveryReport =>
                        {
                            if (deliveryReport.Error is not null)
                            {
                                Console.WriteLine($"Failed to deliver message: {deliveryReport.Error.Reason}");
                            }
                            else
                            {
                                Console.WriteLine($"Published: {e.Event} {e.Data}");
                                _integrationEventBusiness.DeleteIntegrationEventAsync(e.Id);
                            }
                        }
                    );
                }

                using var linkedCts =
                    CancellationTokenSource.CreateLinkedTokenSource(_wakeupCancelationTokenSource.Token,
                        stoppingToken);

                try
                {
                    await Task.Delay(Timeout.Infinite, linkedCts.Token);
                }
                catch (OperationCanceledException)
                {
                    if (_wakeupCancelationTokenSource.Token.IsCancellationRequested)
                    {
                        Debug.WriteLine("Publish requested");
                        var tmp = _wakeupCancelationTokenSource;
                        _wakeupCancelationTokenSource = new CancellationTokenSource();
                        tmp.Dispose();
                    }
                    else if (stoppingToken.IsCancellationRequested)
                    {
                        Debug.WriteLine("Shutting down.");
                    }
                }
            }
        }
        catch (Exception e)
        {
            Debug.WriteLine(e.ToString());
        }
    }
}