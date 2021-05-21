// <copyright file="FaultMap.cs" company="James Dibble">
// Copyright (c) James Dibble. All rights reserved.
// </copyright>

namespace DashTransit.Core.Infrastructure.Mapping
{
    using DashTransit.Core.Domain;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class FaultMap : IEntityTypeConfiguration<Fault>
    {
        public void Configure(EntityTypeBuilder<Fault> builder)
        {
            builder.ToTable("Faults");

            builder.Property(x => x.Exception).IsRequired();
            builder.Property(x => x.ExceptionType).IsRequired();
            builder.Property(x => x.Source).IsRequired();
            builder.Property(x => x.StackTrace).IsRequired();
        }
    }
}
