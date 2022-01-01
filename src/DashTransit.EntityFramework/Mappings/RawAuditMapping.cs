namespace DashTransit.EntityFramework.Mappings;

using System.Collections.Generic;
using Core.Domain;
using DashTransit.EntityFramework.Entities;
using MassTransit.EntityFrameworkCoreIntegration;
using Microsoft.EntityFrameworkCore;

public class RawAuditMapping : IEntityTypeConfiguration<RawAudit>
{
    public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<RawAudit> builder)
    {
        builder.ToTable("__audit");

        builder.HasKey(x => x.AuditRecordId);
        builder.Property(x => x.AuditRecordId)
            .ValueGeneratedOnAdd();

        builder.Property(x => x.ContextType);
        builder.Property(x => x.MessageId).HasConversion<GuidValueOfConverter<MessageId>>();;
        builder.Property(x => x.InitiatorId);
        builder.Property(x => x.ConversationId);
        builder.Property(x => x.CorrelationId);
        builder.Property(x => x.RequestId);
        builder.Property(x => x.SentTime);
        builder.Property(x => x.SourceAddress);
        builder.Property(x => x.DestinationAddress);
        builder.Property(x => x.ResponseAddress);
        builder.Property(x => x.FaultAddress);
        builder.Property(x => x.InputAddress);
        builder.Property(x => x.MessageType);

        builder.Property(x => x.Headers)
            .HasConversion(new JsonValueConverter<Dictionary<string, string>>());
        builder.Property(x => x.Custom)
            .HasConversion(new JsonValueConverter<Dictionary<string, string>>());
        builder.Property(x => x.Message)
            .HasConversion(new JsonValueConverter<object>());
    }
}