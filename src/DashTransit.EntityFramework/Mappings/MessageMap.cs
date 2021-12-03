namespace DashTransit.EntityFramework.Mappings;

using DashTransit.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class MessageMap : IEntityTypeConfiguration<Message>
{
    public void Configure(EntityTypeBuilder<Message> builder)
    {
        builder.ToTable("Message");

        builder.Property(x => x.Id).HasConversion<GuidValueOfConverter<MessageId>>().HasColumnName("MessageId");
        builder.Property(x => x.CorrelationId).HasConversion<GuidValueOfConverter<CorrelationId>>();
        builder.Property(x => x.Type).HasConversion<StringValueOfConverter<MessageType>>();
        builder.Property(x => x.Timestamp).IsRequired().HasColumnName("SentTime");
        builder.Property(x => x.Content).IsRequired().HasColumnName("Message");

        builder.HasMany(x => x.Faults).WithOne(x => x.Message);
    }
}