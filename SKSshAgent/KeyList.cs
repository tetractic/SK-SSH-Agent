﻿// Copyright 2022 Carl Reinke
//
// This file is part of a program that is licensed under the terms of the GNU
// General Public License version 3 as published by the Free Software
// Foundation.

using SKSshAgent.Ssh;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;

namespace SKSshAgent
{
    internal sealed class KeyList
    {
        public static readonly KeyList Instance = new();

        private ImmutableArray<(SshKey Key, string Comment)> _items = ImmutableArray<(SshKey Key, string Comment)>.Empty;

        private KeyList()
        {
        }

        public int KeyCount => _items.Length;

        public ImmutableArray<(SshKey Key, string Comment)> GetAllKeys() => _items;

        public event ChangedEventHandler? Changed;

        public bool AddKey(SshKey key, string comment)
        {
            for (var items = _items; ;)
            {
                foreach (var item in items)
                    if (item.Key.Equals(key, publicOnly: true))
                        return false;

                var newItems = items.Add((key, comment));

                var oldItems = ImmutableInterlocked.InterlockedCompareExchange(ref _items, newItems, items);
                if (oldItems == items)
                {
                    Changed?.Invoke(this);
                    return true;
                }

                items = oldItems;
            }
        }

        public bool RemoveKey(SshKey key)
        {
            for (var items = _items; ;)
            {
                ImmutableArray<(SshKey Key, string Comment)> newItems;
                for (int i = 0; ; ++i)
                {
                    if (i == items.Length)
                        return false;

                    if (items[i].Key.Equals(key, publicOnly: true))
                    {
                        newItems = items.RemoveAt(i);
                        break;
                    }
                }

                var oldItems = ImmutableInterlocked.InterlockedCompareExchange(ref _items, newItems, items);
                if (oldItems == items)
                {
                    Changed?.Invoke(this);
                    return true;
                }

                items = oldItems;
            }
        }

        public bool TryGetKey(SshKey key, [MaybeNullWhen(false)] out SshKey actualKey, [MaybeNullWhen(false)] out string comment)
        {
            foreach (var item in _items)
            {
                if (item.Key.Equals(key, publicOnly: true))
                {
                    actualKey = item.Key;
                    comment = item.Comment;
                    return true;
                }
            }

            actualKey = default;
            comment = default;
            return false;
        }

        public delegate void ChangedEventHandler(KeyList keyList);
    }
}