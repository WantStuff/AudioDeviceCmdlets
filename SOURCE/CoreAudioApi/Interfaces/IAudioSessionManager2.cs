/*
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

namespace AudioDeviceCmdlets.CoreAudioApi.Interfaces
{
    [Guid("77AA99A0-1BD6-484F-8BC7-2C654C9A9B6F"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    interface IAudioSessionManager2 
    {
        // See https://docs.microsoft.com/en-us/windows/win32/api/audiopolicy/

        [PreserveSig]
        int GetAudioSessionControl(ref Guid audioSessionGuid, UInt32 streamFlags,  IntPtr sessionControl );
        [PreserveSig]
        int GetSimpleAudioVolume(ref Guid audioSessionGuid, UInt32 streamFlags, IntPtr  /*ISimpleAudioVolume*/ simpleAudioVolume);
        [PreserveSig]
        int GetSessionEnumerator(out IAudioSessionEnumerator sessionEnum);
        [PreserveSig]
        int RegisterSessionNotification( IntPtr audioSessionNotification );
        [PreserveSig]
        int UnregisterSessionNotification( IntPtr audioSessionNotification );
        [PreserveSig]
        int RegisterDuckNotification( string sessionID, IntPtr audioVolumeDuckNotification);
        [PreserveSig]
        int UnregisterDuckNotification(IntPtr audioVolumeDuckNotification);
    };
}
