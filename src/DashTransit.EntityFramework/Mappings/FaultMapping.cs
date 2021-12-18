// <copyright file="FaultMapping.cs" company="James Dibble">
// Copyright (c) James Dibble. All rights reserved.
// </copyright>

namespace DashTransit.EntityFramework.Mappings;

using System;
using Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Fault = Entities.Fault;

public class FaultMapping : IEntityTypeConfiguration<Fault>
{
    public void Configure(EntityTypeBuilder<Fault> builder)
    {
        builder.Property(x => x.Id)
            .HasConversion<IntValueOfConverter<FaultId>>()
            .UseIdentityColumn()
            .ValueGeneratedOnAdd()
            .HasValueGenerator<IntValueOfGenerator<FaultId>>();
        builder.Property<Guid>("_rawMessageId").UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("MessageId");
        builder.Ignore(x => x.MessageId);
        builder.Property(x => x.Exception);
        builder.Property(x => x.Produced);
        builder.Property(x => x.ProducedBy).HasConversion<UriValueOfConverter<EndpointId>>();
        builder.HasOne(x => x.Message).WithMany().HasForeignKey("_rawMessageId").HasPrincipalKey(x => x.MessageId);
    }
}