// Copyright 2022 Carl Reinke
//
// This file is part of a program that is licensed under the terms of the GNU
// General Public License version 3 as published by the Free Software
// Foundation.

using System;
using System.Collections.Immutable;
using System.Linq;

namespace SKSshAgent
{
    internal static class ImmutableArrayExtensions
    {
        public static bool SequenceEqual<T>(this ImmutableArray<T> @this, ImmutableArray<T> other)
        {
            return @this.AsSpan().SequenceEqual(other.AsSpan());
        }

        public static T[] ToArray<T>(this ImmutableArray<T> @this) => @this.AsSpan().ToArray();
    }
}
