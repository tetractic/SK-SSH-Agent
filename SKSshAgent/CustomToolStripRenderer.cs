// Copyright 2022 Carl Reinke
//
// This file is part of a program that is licensed under the terms of the GNU
// General Public License Version 3 as published by the Free Software
// Foundation.

using System.Windows.Forms;

namespace SKSshAgent;

internal sealed class CustomToolStripRenderer : ToolStripProfessionalRenderer
{
    public CustomToolStripRenderer()
        : base(new CustomColorTable())
    {
    }

    protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
    {
        if (e.ToolStrip is StatusStrip)
            return;

        base.OnRenderToolStripBorder(e);
    }

    private class CustomColorTable : ProfessionalColorTable
    {
        public CustomColorTable()
        {
            UseSystemColors = true;
        }
    }
}
