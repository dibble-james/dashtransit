// <copyright file="EndpointMap.cs" company="James Dibble">
// Copyright (c) James Dibble. All rights reserved.
// </copyright>

namespace DashTransit.Core.Infrastructure.Mapping
{
    using System;
    using DashTransit.Core.Domain;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class EndpointMap : IEntityTypeConfiguration<Endpoint>
    {
        public void Configure(EntityTypeBuilder<Endpoint> builder)
        {
            builder.Property(x => x.Id)
                .HasConversion(id => id.Id, id => new EndpointId(id));

            builder.Property(x => x.Uri)
                .HasConversion(uri => uri.AbsoluteUri, uri => new Uri(uri))
                .IsRequired();
        }
    }
}
