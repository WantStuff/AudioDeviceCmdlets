/*
  Copyright (c) 2016-2018 Francois Gendron <fg@frgn.ca>
  MIT License

  AudioDeviceCmdlets.cs
  AudioDeviceCmdlets is a suite of PowerShell Cmdlets to control audio devices on Windows
  https://github.com/frgnca/AudioDeviceCmdlets
*/

// To interact with MMDevice

// To act as a PowerShell Cmdlet

using System;
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

            Default = baseDevice.IsMultimediaDefault;

            CommunicationsDefault = baseDevice.IsCommunicationsDefault;

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

        public bool Default;

        public bool CommunicationsDefault;

        public string State;

        public bool Mute;

        public int Volume;

        public MMDevice Device;
    }
}