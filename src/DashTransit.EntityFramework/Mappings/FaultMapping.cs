// <copyright file="FaultMapping.cs" company="James Dibble">
// Copyright (c) James Dibble. All rights reserved.
// </copyright>

namespace DashTransit.EntityFramework.Mappings;

using System;
using System.Collections.Generic;
using Core.Domain;
using Entities;
using MassTransit;
using MassTransit.EntityFrameworkCoreIntegration;
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
        builder.Property(x => x.MessageId).HasConversion<GuidValueOfConverter<MessageId>>();
        builder.Property(x => x.Exceptions).HasConversion(new JsonValueConverter<IEnumerable<ExceptionInfo>>());
        builder.Property(x => x.Produced);
        builder.Property(x => x.ProducedBy).HasConversion<UriValueOfConverter<EndpointId>>();
    }
}