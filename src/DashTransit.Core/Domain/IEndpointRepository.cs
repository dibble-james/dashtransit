// <copyright file="IEndpointRepository.cs" company="James Dibble">
// Copyright (c) James Dibble. All rights reserved.
// </copyright>

namespace DashTransit.Core.Domain;

public interface IEndpointRepository
{
    Task<IEnumerable<EndpointId>> GetAll(CancellationToken token);
}