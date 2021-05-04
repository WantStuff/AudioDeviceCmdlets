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
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using AudioDeviceCmdlets.CoreAudioApi.Interfaces;

namespace AudioDeviceCmdlets.CoreAudioApi
{
    // ReSharper disable once InconsistentNaming
    public class MMDeviceCollection : IEnumerable<MMDevice>
    {
        // See https://docs.microsoft.com/en-us/windows/win32/api/mmdeviceapi/

        private readonly List<MMDevice> _mmDevices;

        internal MMDeviceCollection(IMMDeviceCollection parent)
        {
            uint count;
            Marshal.ThrowExceptionForHR(parent.GetCount(out count));

            _mmDevices = new List<MMDevice>();
            for (uint i = 0; i < count; i++)
            {
                IMMDevice result;
                parent.Item(i, out result);
                _mmDevices.Add(new MMDevice(result, (int)i + 1));
            }
        }

        public MMDevice this[int index] => _mmDevices[index];

        public int Count => _mmDevices.Count;

        public MMDevice Find(Predicate<MMDevice> match)
        {
            return _mmDevices.Find(match);
        }

        public IEnumerator<MMDevice> GetEnumerator()
        {
            return _mmDevices.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
