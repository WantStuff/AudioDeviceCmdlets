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
using AudioDeviceCmdlets.CoreAudioApi.Structs;

namespace AudioDeviceCmdlets.CoreAudioApi
{
    public class MultiMediaDevice
    {
        private readonly IMultiMediaDevice _realDevice;
        private PropertyStore _propertyStore;
        private AudioMeterInformation _audioMeterInformation;
        private AudioEndpointVolume _audioEndpointVolume;
        private AudioSessionManager _audioSessionManager;
        private static Guid _audioMeterInformationGuid = typeof(IAudioMeterInformation).GUID;
        private static Guid _audioEndpointVolumeGuid = typeof(IAudioEndpointVolume).GUID;
        private static Guid _audioSessionManagerGuid = typeof(IAudioSessionManager2).GUID;


        internal MultiMediaDevice(IMultiMediaDevice realDevice)
        {
            _realDevice = realDevice;
        }


        public AudioSessionManager AudioSessionManager
        {
            get
            {
                if (_audioSessionManager == null)
                    GetAudioSessionManager();

                return _audioSessionManager;
            }
        }

        public AudioMeterInformation AudioMeterInformation
        {
            get
            {
                if (_audioMeterInformation == null)
                    GetAudioMeterInformation();

                return _audioMeterInformation;
            }
        }

        public AudioEndpointVolume AudioEndpointVolume
        {
            get
            {
                if (_audioEndpointVolume == null)
                    GetAudioEndpointVolume();

                return _audioEndpointVolume;
            }
        }

        public PropertyStore Properties
        {
            get
            {
                if (_propertyStore == null)
                    GetPropertyInformation();
                return _propertyStore;
            }
        }

        public string FriendlyName
        {
            get
            {
                if (_propertyStore == null)
                    GetPropertyInformation();
                if (_propertyStore.Contains(PKEY.PKEY_DeviceInterface_FriendlyName))
                {
                   return (string)_propertyStore[PKEY.PKEY_DeviceInterface_FriendlyName].Value;
                }
                else
                    return "Unknown";
            }
        }


        public string ID
        {
            get
            {
                string Result;
                Marshal.ThrowExceptionForHR(_realDevice.GetId(out Result));
                return Result;
            }
        }

        public DataFlows DataFlow
        {
            get
            {
                DataFlows Result;
                IMultiMediaEndpoint ep =  _realDevice as IMultiMediaEndpoint;
                ep.GetDataFlow(out Result);
                return Result;
            }
        }

        public DeviceStates State
        {
            get
            {
                DeviceStates Result;
                Marshal.ThrowExceptionForHR(_realDevice.GetState(out Result));
                return Result;

            }
        }


        private void GetPropertyInformation()
        {
            IPropertyStore propstore;
            Marshal.ThrowExceptionForHR(_realDevice.OpenPropertyStore(StgmAccesses.STGM_READ, out propstore));
            _propertyStore = new PropertyStore(propstore);
        }

        private void GetAudioSessionManager()
        {
            object result;
            Marshal.ThrowExceptionForHR(_realDevice.Activate(ref _audioSessionManagerGuid, CLSCTX.ALL, IntPtr.Zero, out result));
            _audioSessionManager = new AudioSessionManager(result as IAudioSessionManager2);
        }

        private void GetAudioMeterInformation()
        {
            object result;
            Marshal.ThrowExceptionForHR(_realDevice.Activate(ref _audioMeterInformationGuid, CLSCTX.ALL, IntPtr.Zero, out result));
            _audioMeterInformation = new AudioMeterInformation(result as IAudioMeterInformation);
        }

        private void GetAudioEndpointVolume()
        {
            object result;
            Marshal.ThrowExceptionForHR(_realDevice.Activate(ref _audioEndpointVolumeGuid, CLSCTX.ALL, IntPtr.Zero, out result));
            _audioEndpointVolume = new AudioEndpointVolume(result as IAudioEndpointVolume);
        }
    }
}
