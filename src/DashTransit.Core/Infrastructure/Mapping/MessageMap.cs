// <copyright file="MessageMap.cs" company="James Dibble">
// Copyright (c) James Dibble. All rights reserved.
// </copyright>

namespace DashTransit.Core.Infrastructure.Mapping
{
    using DashTransit.Core.Domain;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class MessageMap : IEntityTypeConfiguration<Message>
    {
        public void Configure(EntityTypeBuilder<Message> builder)
        {
            builder.Property(x => x.Id)
                .HasConversion(id => id.Id, id => new MessageId(id));

            builder.Property(x => x.CorrelationId).IsRequired();
            builder.Property(x => x.Timestamp).IsRequired();
            builder.Property(x => x.Content).IsRequired();
            builder.Property(x => x.Type).IsRequired();
        }
    }
}
