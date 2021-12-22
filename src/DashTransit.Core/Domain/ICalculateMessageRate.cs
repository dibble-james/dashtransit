// <copyright file="ICalculateMessageRate.cs" company="James Dibble">
// Copyright (c) James Dibble. All rights reserved.
// </copyright>

namespace DashTransit.Core.Domain;

public interface ICalculateMessageRate
{
    Task<double> MessageRate(TimeSpan context);

    Task<TimeSpan> ProcessingRate(TimeSpan context);
}