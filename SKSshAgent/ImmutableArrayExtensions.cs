// Copyright 2022 Carl Reinke
//
// This file is part of a program that is licensed under the terms of the GNU
// General Public License Version 3 as published by the Free Software
// Foundation.

using System;
using System.Collections.Immutable;
using System.Linq;
using System.Runtime.CompilerServices;

namespace SKSshAgent
{
    internal static class ImmutableArrayExtensions
    {
        public static bool SequenceEqual<T>(this ImmutableArray<T> @this, ImmutableArray<T> other)
        {
            return @this.AsSpan().SequenceEqual(other.AsSpan());
        }

        public static T[] ToArray<T>(this ImmutableArray<T> @this) => @this.AsSpan().ToArray();

        public static ImmutableArray<T> ToImmutableArray<T>(this Memory<T> memory) => ToImmutableArray((ReadOnlyMemory<T>)memory);

        public static ImmutableArray<T> ToImmutableArray<T>(this ReadOnlyMemory<T> memory)
        {
            var array = memory.ToArray();
            return Unsafe.As<T[], ImmutableArray<T>>(ref array);
        }

        public static ImmutableArray<T> ToImmutableArray<T>(this Span<T> span) => ToImmutableArray((ReadOnlySpan<T>)span);

        public static ImmutableArray<T> ToImmutableArray<T>(this ReadOnlySpan<T> span)
        {
            var array = span.ToArray();
            return Unsafe.As<T[], ImmutableArray<T>>(ref array);
        }
    }
}
