// <copyright file="StatisticsRepository.cs" company="James Dibble">
// Copyright (c) James Dibble. All rights reserved.
// </copyright>

namespace DashTransit.EntityFramework.Repositories;

using System;
using System.Linq;
using System.Threading.Tasks;
using Core.Domain;
using Microsoft.EntityFrameworkCore;

public class StatisticsRepository : ICalculateMessageRate
{
    private readonly DashTransitContext database;

    public StatisticsRepository(DashTransitContext database) => this.database = database;

    public async Task<double> MessageRate(TimeSpan context)
    {
        var minDate = DateTime.Now.Add(-context);

        var average = await this.database.Audit
            .Where(Message.IsProducerSpec)
            .Where(x => x.SentTime.HasValue && x.SentTime >= minDate)
            .GroupBy(x => new { x.SentTime.Value.Hour, x.SentTime.Value.Minute, x.SentTime.Value.Second })
            .Select(x => x.Count())
            .DefaultIfEmpty()
            .AverageAsync();

        return average;
    }

    public async Task<TimeSpan> ProcessingRate(TimeSpan context)
    {
        var minDate = DateTime.Now.Add(-context);

        var average = await this.database.Audit
            .Where(x => x.SentTime.HasValue && x.SentTime >= minDate)
            .GroupBy(x => x.MessageId)
            .Select(x => new { Min = x.Min(y => y.SentTime), Max = x.Max(y => y.SentTime) })
            .AverageAsync(x => EF.Functions.DateDiffMillisecond(x.Min, x.Max));

        return TimeSpan.FromMilliseconds(average.GetValueOrDefault());
    }
}