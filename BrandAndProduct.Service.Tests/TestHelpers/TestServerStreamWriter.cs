using System.Threading.Channels;
using Grpc.Core;
using Channel = System.Threading.Channels.Channel;

namespace BrandAndProduct.Service.Tests.TestHelpers;

public class TestServerStreamWriter<T> : IServerStreamWriter<T> where T : class
{
    private readonly Channel<T> _channel;
    private readonly ServerCallContext _serverCallContext;

    public TestServerStreamWriter(ServerCallContext serverCallContext)
    {
        _channel = Channel.CreateUnbounded<T>();

        _serverCallContext = serverCallContext;
    }

    public WriteOptions? WriteOptions { get; set; }

    public Task WriteAsync(T message)
    {
        if (_serverCallContext.CancellationToken.IsCancellationRequested)
        {
            return Task.FromCanceled(_serverCallContext.CancellationToken);
        }

        if (!_channel.Writer.TryWrite(message))
        {
            throw new InvalidOperationException("Unable to write message.");
        }

        return Task.CompletedTask;
    }

    public void Complete()
    {
        _channel.Writer.Complete();
    }

    public IAsyncEnumerable<T> ReadAllAsync()
    {
        return _channel.Reader.ReadAllAsync();
    }

    public async Task<T?> ReadNextAsync()
    {
        if (await _channel.Reader.WaitToReadAsync())
        {
            _channel.Reader.TryRead(out var message);
            return message;
        }
        else
        {
            return null;
        }
    }
}