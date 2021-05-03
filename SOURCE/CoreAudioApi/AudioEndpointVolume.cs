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
using AudioDeviceCmdlets.CoreAudioApi.Delegates;
using AudioDeviceCmdlets.CoreAudioApi.Enums;
using AudioDeviceCmdlets.CoreAudioApi.Interfaces;

namespace AudioDeviceCmdlets.CoreAudioApi
{

    public class AudioEndpointVolume : IDisposable
    {
        private readonly IAudioEndpointVolume _audioEndPointVolume;
        private readonly AudioEndpointVolumeChannels _channels;
        private readonly AudioEndpointVolumeStepInformation _stepInformation;
        private readonly AudioEndPointVolumeVolumeRange _volumeRange;
        private readonly EndpointHardwareSupports _hardwareSupport;
        private AudioEndpointVolumeCallback _callBack;
        public  event AudioEndpointVolumeNotificationDelegate OnVolumeNotification;

        internal AudioEndpointVolume(IAudioEndpointVolume realEndpointVolume)
        {
            uint hardwareSupp;

            _audioEndPointVolume = realEndpointVolume;
            _channels = new AudioEndpointVolumeChannels(_audioEndPointVolume);
            _stepInformation = new AudioEndpointVolumeStepInformation(_audioEndPointVolume);
            Marshal.ThrowExceptionForHR(_audioEndPointVolume.QueryHardwareSupport(out hardwareSupp));
            _hardwareSupport = (EndpointHardwareSupports)hardwareSupp;
            _volumeRange = new AudioEndPointVolumeVolumeRange(_audioEndPointVolume);
            _callBack = new AudioEndpointVolumeCallback(this);
            Marshal.ThrowExceptionForHR(_audioEndPointVolume.RegisterControlChangeNotify(_callBack));
        }

        public AudioEndPointVolumeVolumeRange VolumeRange
        {
            get
            {
                return _volumeRange;
            }
        }
        public EndpointHardwareSupports HardwareSupport
        {
            get
            {
                return _hardwareSupport;
            }
        }
        public AudioEndpointVolumeStepInformation StepInformation
        {
            get
            {
                return _stepInformation;
            }
        }
        public AudioEndpointVolumeChannels Channels
        {
            get
            {
                return _channels;
            }
        }
        public float MasterVolumeLevel
        {
            get
            {
                float result;
                Marshal.ThrowExceptionForHR(_audioEndPointVolume.GetMasterVolumeLevel(out result));
                return result;
            }
            set
            {
                Marshal.ThrowExceptionForHR(_audioEndPointVolume.SetMasterVolumeLevel(value, Guid.Empty));
            }
        }
        public float MasterVolumeLevelScalar
        {
            get
            {
                float result;
                Marshal.ThrowExceptionForHR(_audioEndPointVolume.GetMasterVolumeLevelScalar(out result));
                return result;
            }
            set
            {
                Marshal.ThrowExceptionForHR(_audioEndPointVolume.SetMasterVolumeLevelScalar(value, Guid.Empty));
            }
        }
        public bool Mute
        {
            get
            {
                bool result;
                Marshal.ThrowExceptionForHR(_audioEndPointVolume.GetMute(out result));
                return result;
            }
            set
            {
                Marshal.ThrowExceptionForHR(_audioEndPointVolume.SetMute(value, Guid.Empty));
            }
        }
        public void VolumeStepUp()
        {
            Marshal.ThrowExceptionForHR(_audioEndPointVolume.VolumeStepUp(Guid.Empty));
        }
        public void VolumeStepDown()
        {
            Marshal.ThrowExceptionForHR(_audioEndPointVolume.VolumeStepDown(Guid.Empty));
        }
        internal void FireNotification(AudioVolumeNotificationData NotificationData)
        {
            AudioEndpointVolumeNotificationDelegate del = OnVolumeNotification;
            if (del != null)
            {
                del(NotificationData);
            }
        }


        #region IDisposable Members

        public void Dispose()
        {
            if (_callBack != null)
            {
                Marshal.ThrowExceptionForHR(_audioEndPointVolume.UnregisterControlChangeNotify( _callBack ));
                _callBack = null;
            }
        }

        ~AudioEndpointVolume()
        {
            Dispose();
        }

        #endregion
       
    }
}
