// <copyright file="ResendMessage.cs" company="James Dibble">
// Copyright (c) James Dibble. All rights reserved.
// </copyright>

namespace DashTransit.Core.Application.Commands;

using System.IO;
using System.Net.Mime;
using System.Runtime.Serialization;
using System.Text;
using MassTransit;
using MassTransit.Serialization;
using Newtonsoft.Json;

public record SendMessage(EndpointId To, string MessageType, IEnumerable<KeyValuePair<string?, string?>> Headers, string Message) : IRequest
{
    public class Handler : IRequestHandler<SendMessage>
    {
        private readonly IBus bus;

        public Handler(IBus bus) => this.bus = bus;

        public async Task<Unit> Handle(SendMessage request, CancellationToken cancellationToken)
        {
            var messageObj = JsonConvert.DeserializeObject(request.Message);

            await (await this.bus.GetSendEndpoint(request.To.Value))
                .Send(messageObj,
                    context =>
                    {
                        request.Headers.Where(h => !string.IsNullOrEmpty(h.Key)).ToList().ForEach(h => context.Headers.Set(h.Key, h.Value));
                        context.Serializer = new SerialiserWrapper(request.MessageType, context.Serializer);
                    },
                    cancellationToken);

            return Unit.Value;
        }
    }

    public class SerialiserWrapper : IMessageSerializer
    {
        private readonly string messageType;
        private readonly IMessageSerializer inner;

        public SerialiserWrapper(string messageType, IMessageSerializer inner)
        {
            this.messageType = messageType;
            this.inner = inner;
        }

        public void Serialize<T>(Stream stream, SendContext<T> context) where T : class
        {
            try
            {
                context.ContentType = this.inner.ContentType;

                var messageTypeParts = this.messageType.Split('.');
                var messageType = $"urn:message:{string.Join(".", messageTypeParts[..^1])}:{messageTypeParts.Last()}";

                var envelope = new JsonMessageEnvelope(context, context.Message, new[] { messageType });

                using var writer = new StreamWriter(stream, new UTF8Encoding(false, true), 1024, true);
                using var jsonWriter = new JsonTextWriter(writer) { Formatting = Formatting.Indented };

                JsonMessageSerializer.Serializer.Serialize(jsonWriter, envelope, typeof(MessageEnvelope));

                jsonWriter.Flush();
                writer.Flush();
            }
            catch (SerializationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new SerializationException("Failed to serialize message", ex);
            }
        }

        public ContentType ContentType => this.inner.ContentType;
    }
}