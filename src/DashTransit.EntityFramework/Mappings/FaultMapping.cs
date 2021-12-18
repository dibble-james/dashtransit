// <copyright file="FaultMapping.cs" company="James Dibble">
// Copyright (c) James Dibble. All rights reserved.
// </copyright>

namespace DashTransit.EntityFramework.Mappings;

using Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class FaultMapping : IEntityTypeConfiguration<Fault>
{
    public void Configure(EntityTypeBuilder<Fault> builder)
    {
        builder.Property(x => x.Id).HasConversion<IntValueOfConverter<FaultId>>();
        builder.Property(x => x.MessageId).HasConversion<GuidValueOfConverter<MessageId>>();
        builder.Property(x => x.Exception);
    }
}