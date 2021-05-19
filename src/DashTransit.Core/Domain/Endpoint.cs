// <copyright file="Endpoint.cs" company="James Dibble">
// Copyright (c) James Dibble. All rights reserved.
// </copyright>

namespace DashTransit.Core.Domain
{
    using System;
    using DashTransit.Core.Domain.Common;

    public record EndpointId : IntIdentity<EndpointId>
    {
        public EndpointId(int Id)
            : base(Id)
        {
        }
    }

    public class Endpoint : Entity<EndpointId>
    {
        private readonly Uri uri;

        public Endpoint(EndpointId id)
            : base(id)
        {
        }

        public Uri Uri => this.uri;
    }
}
