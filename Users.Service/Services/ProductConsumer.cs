using System.Diagnostics;
using Confluent.Kafka;
using Newtonsoft.Json.Linq;
using Users.Service.Business.ProductBusiness;
using Users.Service.Models.Dto;

namespace Users.Service.Services;

/// <summary>
/// Represents a hosted service for consuming messages related to products.
/// </summary>
public class ProductConsumer : IHostedService, IDisposable
{
    private readonly IConsumer<string, string> _consumer;
    private readonly IProductBusiness _productBusiness;

    /// <summary>
    /// Initializes a new instance of the <see cref="ProductConsumer"/> class.
    /// </summary>
    /// <param name="productBusiness">The business service for handling product-related operations.</param>
    /// <param name="consumer">The Kafka consumer used for message consumption.</param>
    public ProductConsumer(IProductBusiness productBusiness, IConsumer<string, string> consumer)
    {
        _productBusiness = productBusiness;
        _consumer = consumer;
        _consumer.Subscribe("products");
    }

    /// <summary>
    /// Gets the name of the queue for product messages.
    /// </summary>
    protected string QueueName => "products";

    /// <inheritdoc/>
    public void Dispose()
    {
        try
        {
            _consumer.Close();
            _consumer.Dispose();
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
        }
    }

    /// <inheritdoc/>
    public Task StartAsync(CancellationToken cancellationToken)
    {
        Task.Run(() => ConsumeMessages(cancellationToken));
        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    public Task StopAsync(CancellationToken cancellationToken)
    {
        Dispose();
        return Task.CompletedTask;
    }

    private void ConsumeMessages(CancellationToken cancellationToken)
    {
        try
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var consumeResult = _consumer.Consume(cancellationToken);

                    if (consumeResult != null)
                    {
                        var data = JObject.Parse(consumeResult.Message.Value);
                        switch (consumeResult.Message.Key)
                        {
                            case "product.create":
                                _productBusiness.CreateProductAsync(new ProductDto()
                                {
                                    Id = data["id"]!.Value<long>(),
                                    Name = data["name"]!.Value<string>()!,
                                    Rating = data["rating"]!.Value<int>(),
                                });
                                break;
                            case "product.update":
                                _productBusiness.UpdateProductAsync(new ProductDto()
                                {
                                    Id = data["id"]!.Value<long>(),
                                    Name = data["name"]!.Value<string>()!,
                                    Rating = data["rating"]!.Value<int>()
                                });
                                break;
                            case "product.delete":
                                _productBusiness.DeleteProductAsync(data["id"]!.Value<long>());
                                break;
                        }

                        Console.WriteLine($"[BROKER] Consumed message : {consumeResult.Message.Key}");
                        _consumer.Commit(consumeResult);
                    }
                }
                catch (ConsumeException ce)
                {
                    Console.WriteLine($"Error occurred: {ce.Error.Reason}");
                }
            }
        }
        catch (OperationCanceledException oe)
        {
            Console.WriteLine($"Canceled: {oe.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error occurred: {ex.Message}");
        }
    }
}
