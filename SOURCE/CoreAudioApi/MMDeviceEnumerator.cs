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
using AudioDeviceCmdlets.CoreAudioApi.Enums;
using AudioDeviceCmdlets.CoreAudioApi.Interfaces;

namespace AudioDeviceCmdlets.CoreAudioApi
{
    //Marked as internal, since on its own its no good

    //Small wrapper class
    // ReSharper disable once InconsistentNaming
    public class MMDeviceEnumerator
    {
        // See https://docs.microsoft.com/en-us/windows/win32/api/mmdeviceapi/

        private readonly IMMDeviceEnumerator _realEnumerator = new MMDeviceEnumeratorCom() as IMMDeviceEnumerator;

        public MMDeviceCollection EnumerateAudioEndPoints(DataFlows dataFlow, DeviceStates dwStateMask)
        {
            IMMDeviceCollection result;
            Marshal.ThrowExceptionForHR(_realEnumerator.EnumAudioEndpoints(dataFlow,dwStateMask,out result));
            return new MMDeviceCollection(result);
        }

        public MMDevice GetDefaultAudioEndpoint(DataFlows dataFlow, DeviceRoles role)
        {
            IMMDevice _Device = null;
            Marshal.ThrowExceptionForHR(((IMMDeviceEnumerator)_realEnumerator).GetDefaultAudioEndpoint(dataFlow, role, out _Device));
            return new MMDevice(_Device, -1, false, false);
        }

        public MMDevice GetDevice(string ID)
        {
            IMMDevice _Device = null;
            Marshal.ThrowExceptionForHR(((IMMDeviceEnumerator)_realEnumerator).GetDevice(ID, out _Device));
            return new MMDevice(_Device, -1, false, false);
        }

        public MMDeviceEnumerator()
        {
            if (System.Environment.OSVersion.Version.Major < 6)
            {
                throw new NotSupportedException("This functionality is only supported on Windows Vista or newer.");
            }
        }
    }
}
