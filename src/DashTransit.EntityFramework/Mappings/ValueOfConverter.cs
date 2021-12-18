namespace DashTransit.EntityFramework.Mappings;

using System;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ValueOf;

public class GuidValueOfConverter<T> : ValueConverter<T, Guid>
    where T : ValueOf<Guid, T>, new()
{
    public GuidValueOfConverter()
        : base(Out, In)
    {
    }

    private static Expression<Func<T, Guid>> Out => value => value.Value;

    private static Expression<Func<Guid, T>> In => value => ValueOf<Guid, T>.From(value);
}

public class StringValueOfConverter<T> : ValueConverter<T, string>
    where T : ValueOf<string, T>, new()
{
    public StringValueOfConverter()
        : base(Out, In)
    {
    }

    private static Expression<Func<T, string>> Out => value => value.Value;

    private static Expression<Func<string, T>> In => value => ValueOf<string, T>.From(value);
}

public class UriValueOfConverter<T> : ValueConverter<T, string>
    where T : ValueOf<Uri, T>, new()
{
    public UriValueOfConverter()
        : base(Out, In)
    {
    }

    private static Expression<Func<T, string>> Out => value => value.Value.ToString();

    private static Expression<Func<string, T>> In => value => ValueOf<Uri, T>.From(new Uri(value));
}

public class IntValueOfConverter<T> : ValueConverter<T, int>
    where T : ValueOf<int, T>, new()
{
    public IntValueOfConverter()
        : base(Out, In)
    {
    }

    private static Expression<Func<T, int>> Out => value => value.Value;

    private static Expression<Func<int, T>> In => value => ValueOf<int, T>.From(value);
}