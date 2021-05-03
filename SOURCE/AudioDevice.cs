/*
  Copyright (c) 2016-2018 Francois Gendron <fg@frgn.ca>
  MIT License

  AudioDeviceCmdlets.cs
  AudioDeviceCmdlets is a suite of PowerShell Cmdlets to control audio devices on Windows
  https://github.com/frgnca/AudioDeviceCmdlets
*/

// To interact with MMDevice

// To act as a PowerShell Cmdlet
using AudioDeviceCmdlets.CoreAudioApi;
using AudioDeviceCmdlets.CoreAudioApi.Enums;

namespace AudioDeviceCmdlets
{
    // Class to interact with a MMDevice as an object with attributes
    public class AudioDevice
    {
        // To be created, a new AudioDevice needs an Index, and the MMDevice it will communicate with
        public AudioDevice(int index, MMDevice baseDevice, bool isMultimediaDefault = false)
        {
            // Set this object's Index to the received integer
            Index = index;

            // Set this object's Default to the received boolean
            Default = isMultimediaDefault;

            // If the received MMDevice is a playback device
            if (baseDevice.DataFlow == DataFlows.Render)
            {
                // Set this object's Type to "Playback"
                Type = "Playback";
            }
            // If not, if the received MMDevice is a recording device
            else if (baseDevice.DataFlow == DataFlows.Capture)
            {
                // Set this object's Type to "Recording"
                Type = "Recording";
            }

            // Set this object's Name to that of the received MMDevice's FriendlyName
            Name = baseDevice.FriendlyName;

            // Set this object's Device to the received MMDevice
            Device = baseDevice;

            // Set this object's ID to that of the received MMDevice's ID
            ID = baseDevice.ID;
        }

        // Order in which this MMDevice appeared from MultiMediaDeviceEnumerator
        public int Index;

        // Default (for its Type) is either true or false
        public bool Default;

        // Type is either "Playback" or "Recording"
        public string Type;

        // Name of the MMDevice ex: "Speakers (Realtek High Definition Audio)"
        public string Name;

        // ID of the MMDevice ex: "{0.0.0.00000000}.{c4aadd95-74c7-4b3b-9508-b0ef36ff71ba}"
        public string ID;

        // The MMDevice itself
        public MMDevice Device;
    }
}