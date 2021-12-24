// <copyright file="ResendMessage.cs" company="James Dibble">
// Copyright (c) James Dibble. All rights reserved.
// </copyright>

namespace DashTransit.Core.Application.Commands;

using System.IO;
using System.Net.Mime;
using System.Runtime.Serialization;
using System.Text;
using MassTransit;
using MassTransit.Metadata;
using MassTransit.Serialization;
using Newtonsoft.Json;

public record ResendMessage(AuditId Message, EndpointId To) : IRequest
{
    public class Handler : IRequestHandler<ResendMessage>
    {
        private readonly IBus bus;
        private readonly IReadRepositoryBase<IRawAuditData> messages;

        public Handler(IBus bus, IReadRepositoryBase<IRawAuditData> messages)
        {
            this.bus = bus;
            this.messages = messages;
        }

        public async Task<Unit> Handle(ResendMessage request, CancellationToken cancellationToken)
        {
            var message = await this.messages.GetByIdAsync(request.Message.Value);

            if (message is not null)
            {
                await (await this.bus.GetSendEndpoint(request.To.Value))
                    .Send(message.Message,
                        context => context.Serializer = new SerialiserWrapper(message.MessageType, context.Serializer),
                        cancellationToken);
            }

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

                var envelope = new JsonMessageEnvelope(context, context.Message, new[] {messageType});

                using var writer = new StreamWriter(stream, new UTF8Encoding(false, true), 1024, true);
                using var jsonWriter = new JsonTextWriter(writer) {Formatting = Formatting.Indented};

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