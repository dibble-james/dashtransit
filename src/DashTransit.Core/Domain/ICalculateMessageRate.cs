// <copyright file="ICalculateMessageRate.cs" company="James Dibble">
// Copyright (c) James Dibble. All rights reserved.
// </copyright>

namespace DashTransit.Core.Domain;

public interface ICalculateMessageRate
{
    Task<double> MessageRate(TimeSpan context, EndpointId? endpoint);

    Task<TimeSpan> ProcessingRate(TimeSpan context);

    Task<int> FaultCount(TimeSpan context);

    Task<double> FailureRate(TimeSpan context);
}