// Copyright 2022 Carl Reinke
//
// This file is part of a program that is licensed under the terms of the GNU
// General Public License version 3 as published by the Free Software
// Foundation.

using System;
using System.Collections.Immutable;
using System.Runtime.CompilerServices;

namespace SKSshAgent
{
    internal static class ReadOnlySpanExtensions
    {
        public static ImmutableArray<T> ToImmutableArray<T>(this ReadOnlySpan<T> span)
        {
            var array = span.ToArray();
            return Unsafe.As<T[], ImmutableArray<T>>(ref array);
        }
    }
}
