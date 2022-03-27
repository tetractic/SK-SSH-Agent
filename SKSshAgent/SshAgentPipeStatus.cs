// Copyright 2022 Carl Reinke
//
// This file is part of a program that is licensed under the terms of the GNU
// General Public License version 3 as published by the Free Software
// Foundation.

namespace SKSshAgent
{
    internal enum SshAgentPipeStatus
    {
        NotStarted = 0,
        Starting,
        RunningIdle,
        RunningBusy,
        Stopped,
        AlreadyInUse,
    }
}
