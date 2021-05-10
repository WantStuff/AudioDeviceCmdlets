/*
  Based on the work done by Francois Gendron <fg@frgn.ca>
  https://github.com/frgnca/AudioDeviceCmdlets
*/

using System;
using System.Management.Automation;
using AudioDeviceCmdlets.CoreAudioApi;
using AudioDeviceCmdlets.CoreAudioApi.Enums;

namespace AudioDeviceCmdlets
{
    public class AudioDevice
    {
        public AudioDevice(MMDevice baseDevice)
        {
            Index = baseDevice.Index;

            Name = baseDevice.FriendlyName;

            Id = baseDevice.Id;

            switch (baseDevice.DataFlow)
            {
                case DataFlows.Render:
                    Type = "Playback";
                    break;
                case DataFlows.Capture:
                    Type = "Recording";
                    break;
            }

            MultimediaDefault = baseDevice.IsMultimediaDefault;

            CommunicationDefault = baseDevice.IsCommunicationsDefault;

            switch (baseDevice.State)
            {
                case DeviceStates.DEVICE_STATE_ACTIVE:
                    State = "Active";
                    break;
                case DeviceStates.DEVICE_STATE_NOTPRESENT:
                    State = "Not Present";
                    break;
                case DeviceStates.DEVICE_STATE_UNPLUGGED:
                    State = "Unplugged";
                    break;
            }

            Mute = baseDevice.AudioEndpointVolume.Mute;

            Volume = (int) Math.Round(baseDevice.AudioEndpointVolume.MasterVolumeLevelScalar * 100, 0);

            Device = baseDevice;
        }

        public int Index;

        public string Id;

        public string Name;

        public string Type;

        public bool MultimediaDefault;

        public bool CommunicationDefault;

        public string State;

        public bool Mute;

        public int Volume;

        [Hidden]
        public MMDevice Device;
    }
}