// <copyright file="Identity.cs" company="James Dibble">
// Copyright (c) James Dibble. All rights reserved.
// </copyright>

namespace DashTransit.Core.Domain.Common
{
    using System;

    public abstract record StringIdentity<T>(string Id)
        where T : StringIdentity<T>
    {
        public static implicit operator string(StringIdentity<T> id)
            => id.Id;

        public static implicit operator T(StringIdentity<T> id)
            => id as T ?? throw new InvalidCastException();

        public static implicit operator StringIdentity<T>(string id)
            => (T)Activator.CreateInstance(typeof(T), id)!;
    }

    public abstract record GuidIdentity<T>(Guid Id)
        where T : GuidIdentity<T>
    {
        public static implicit operator Guid(GuidIdentity<T> id)
            => id.Id;

        public static implicit operator GuidIdentity<T>(Guid id)
            => (T)Activator.CreateInstance(typeof(T), id)!;
    }

    public abstract record IntIdentity<T>(int Id)
        where T : IntIdentity<T>
    {
        public static implicit operator int(IntIdentity<T> id)
            => id.Id;

        public static implicit operator IntIdentity<T>(int id)
            => (T)Activator.CreateInstance(typeof(T), id)!;
    }
}