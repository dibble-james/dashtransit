// <copyright file="WithRandomFailure.cs" company="James Dibble">
// Copyright (c) James Dibble. All rights reserved.
// </copyright>

namespace FaultGenerator
{
    using System;
    using System.Threading.Tasks;

    public static class Failure
    {
        private static readonly Random random = new Random();

        public static Func<Func<Task>, Task> WithFailureRate(double rate) => async (Func<Task> func) =>
        {
            if (Math.Round(random.NextDouble(), 1) < rate)
            {
                throw new Exception("FAIL");
            }

            await func();
        };
    }
}
