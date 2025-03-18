using System.Text.Json;
using Confluent.Kafka;
using CQRS.Core.Consumers;
using CQRS.Core.Events;
using Microsoft.Extensions.Options;
using Post.Query.Infrastructure.Converters;
using Post.Query.Infrastructure.Handlers;

namespace Post.Query.Infrastructure.Consumers;

public class EventConsumer : IEventConsumer
{
    private readonly ConsumerConfig _consumerConfig;
    private readonly IEventHandler _eventHandler;

    public EventConsumer(IOptions<ConsumerConfig> consumerConfig, IEventHandler eventHandler)
    {
        _consumerConfig = consumerConfig.Value;
        _eventHandler = eventHandler;
    }

    public void Consume(string topic)
    {
        using var consumer = new ConsumerBuilder<string, string>(_consumerConfig)
            .SetKeyDeserializer(Deserializers.Utf8)
            .SetValueDeserializer(Deserializers.Utf8)
            .Build();
        consumer.Subscribe(topic);
        while(true)
        {
            var consumerResult = consumer.Consume();
            if(consumerResult?.Message == null) continue;
            var options = new JsonSerializerOptions();
            options.Converters.Add(new EventJsonConverter());
            var @event  = JsonSerializer.Deserialize<BaseEvent>(consumerResult.Message.Value, options);
            if (@event != null)
            {
                var handlerMethod = _eventHandler.GetType().GetMethod("On", [@event.GetType()] );
                if (handlerMethod == null)
                {
                    throw new ArgumentNullException(nameof(handlerMethod), "Could not find event handler method");
                }
                handlerMethod.Invoke(_eventHandler, [@event]);
                consumer.Commit(consumerResult);
            }
            else 
            {
                throw new ArgumentNullException(nameof(@event), "Could not deserialize the event");
            }

        }
        throw new NotImplementedException();
    }
}
