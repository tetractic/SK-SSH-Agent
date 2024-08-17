// Copyright 2022 Carl Reinke
//
// This file is part of a program that is licensed under the terms of the GNU
// General Public License Version 3 as published by the Free Software
// Foundation.

using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace SKSshAgent;

internal static class Program
{
    [STAThread]
    private static int Main(string[] args)
    {
        Mutex? mutex;
        try
        {
            mutex = new Mutex(initiallyOwned: false, $"SKSshAgent.35552eb1-eca6-4c53-b36c-4d12b90a6bb0");
        }
        catch (Exception ex)
            when (ex is UnauthorizedAccessException ||
                  ex is WaitHandleCannotBeOpenedException ||
                  ex is IOException)
        {
            mutex = null;
        }
        using (mutex)
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            ToolStripManager.Renderer = new CustomToolStripRenderer();

            if (mutex != null && !mutex.WaitOne(TimeSpan.Zero))
            {
                _ = MessageBox.Show("SK SSH Agent is already running.", "SK SSH Agent", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return -1;
            }

            var mainForm = new KeyListForm();
            mainForm.Show();

            foreach (string filePath in args)
                mainForm.LoadKeyFromFile(filePath);

            if (KeyList.Instance.KeyCount == 0)
                mainForm.AllowVisible = true;

            Application.Run(mainForm);

            return 0;
        }
    }
}
