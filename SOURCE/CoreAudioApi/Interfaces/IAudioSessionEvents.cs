﻿/*
  LICENSE
  -------
  Copyright (C) 2007-2010 Ray Molenkamp

  This source code is provided 'as-is', without any express or implied
  warranty.  In no event will the authors be held liable for any damages
  arising from the use of this source code or the software it produces.

  Permission is granted to anyone to use this source code for any purpose,
  including commercial applications, and to alter it and redistribute it
  freely, subject to the following restrictions:

  1. The origin of this source code must not be misrepresented; you must not
     claim that you wrote the original source code.  If you use this source code
     in a product, an acknowledgment in the product documentation would be
     appreciated but is not required.
  2. Altered source versions must be plainly marked as such, and must not be
     misrepresented as being the original source code.
  3. This notice may not be removed or altered from any source distribution.
*/

using System;
using System.Runtime.InteropServices;
using AudioDeviceCmdlets.CoreAudioApi.Enums;

namespace AudioDeviceCmdlets.CoreAudioApi.Interfaces
{
    [Guid("24918ACC-64B3-37C1-8CA9-74A66E9957A8"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IAudioSessionEvents
    {
        // See https://docs.microsoft.com/en-us/windows/win32/api/audiopolicy/

        [PreserveSig]
        int OnDisplayNameChanged([MarshalAs(UnmanagedType.LPWStr)] string newDisplayName, Guid eventContext);

        [PreserveSig]
        int OnIconPathChanged([MarshalAs(UnmanagedType.LPWStr)] string newIconPath, Guid eventContext);

        [PreserveSig]
        int OnSimpleVolumeChanged(float newVolume, bool newMute, Guid eventContext);

        [PreserveSig]
        int OnChannelVolumeChanged(UInt32 channelCount, IntPtr newChannelVolumeArray, UInt32 changedChannel, Guid eventContext);

        [PreserveSig]
        int OnGroupingParamChanged(Guid newGroupingParam, Guid eventContext);

        [PreserveSig]
        int OnStateChanged(AudioSessionStates newState);

        [PreserveSig]
        int OnSessionDisconnected(AudioSessionDisconnectReasons disconnectReason);
    }
}
