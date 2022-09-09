// Copyright 2022 Carl Reinke
//
// This file is part of a program that is licensed under the terms of the GNU
// General Public License Version 3 as published by the Free Software
// Foundation.

using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SKSshAgent
{
    internal static class ControlExtensions
    {
        public static async Task InvokeAsync(this Control control, Action action)
        {
            _ = await Task.Factory.FromAsync(
                control.BeginInvoke(action),
                asyncResult => control.EndInvoke(asyncResult)).ConfigureAwait(false);
        }

        public static async Task<T> InvokeAsync<T>(this Control control, Func<T> func)
        {
            return await Task.Factory.FromAsync(
                control.BeginInvoke(func),
                asyncResult => (T)control.EndInvoke(asyncResult)).ConfigureAwait(false);
        }

        public static async Task<T> InvokeAsync<T>(this Control control, Func<Task<T>> func)
        {
            var task = await Task.Factory.FromAsync(
                control.BeginInvoke(func),
                asyncResult => (Task<T>)control.EndInvoke(asyncResult)).ConfigureAwait(false);
            return await task.ConfigureAwait(false);
        }
    }
}
