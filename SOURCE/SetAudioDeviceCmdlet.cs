/*
  Copyright (c) 2016-2018 Francois Gendron <fg@frgn.ca>
  MIT License

  AudioDeviceCmdlets.cs
  AudioDeviceCmdlets is a suite of PowerShell Cmdlets to control audio devices on Windows
  https://github.com/frgnca/AudioDeviceCmdlets
*/

// To interact with MMDevice

// To act as a PowerShell Cmdlet

using System.Management.Automation;
using AudioDeviceCmdlets.CoreAudioApi;
using AudioDeviceCmdlets.CoreAudioApi.Enums;

namespace AudioDeviceCmdlets
{
    [Cmdlet(VerbsCommon.Set, nameof(AudioDevice))]
    public class SetAudioDeviceCmdlet : Cmdlet
    {
        // Parameter receiving the AudioDevice to set as default
        [Parameter(Mandatory = true, ParameterSetName = nameof(InputObject), ValueFromPipeline = true)]
        public AudioDevice InputObject { get; set; }

        // Parameter receiving the ID of the device to set as default
        [Parameter(Mandatory = true, Position = 0, ParameterSetName = nameof(ID))]
        public string ID { get; set; }

        // Parameter receiving the Index of the device to set as default
        [ValidateRange(1, 42)]
        [Parameter(Mandatory = true, Position = 0, ParameterSetName = nameof(Index))]
        public int? Index { get; set; }

        // Parameter called to set the default playback device's mute state
        [Parameter(Mandatory = true, Position = 0, ParameterSetName = nameof(PlaybackMute))]
        public bool? PlaybackMute { get; set; }

        // Parameter called to toggle the default playback device's mute state
        [Parameter(Mandatory = true, Position = 0, ParameterSetName = nameof(PlaybackMuteToggle))]
        public SwitchParameter PlaybackMuteToggle { get; set; }

        // Parameter receiving the volume level to set to the defaut playback device
        [ValidateRange(0, 100.0f)]
        [Parameter(Mandatory = true, Position = 0, ParameterSetName = nameof(PlaybackVolume))]
        public float? PlaybackVolume { get; set; }

        // Parameter called to set the default recording device's mute state
        [Parameter(Mandatory = true, Position = 0, ParameterSetName = nameof(RecordingMute))]
        public bool? RecordingMute { get; set; }

        // Parameter called to toggle the default recording device's mute state
        [Parameter(Mandatory = true, Position = 0, ParameterSetName = nameof(RecordingMuteToggle))]
        public SwitchParameter RecordingMuteToggle { get; set; }

        // Parameter receiving the volume level to set to the defaut recording device
        [ValidateRange(0, 100.0f)]
        [Parameter(Mandatory = true, Position = 0, ParameterSetName = nameof(RecordingVolume))]
        public float? RecordingVolume { get; set; }


        protected override void ProcessRecord()
        {
            // If the InputObject parameter received a value
            if (InputObject != null)
            {
                ProcessSwitchInputObject(InputObject);
            }

            // If the ID parameter received a value
            if (!string.IsNullOrEmpty(ID))
            {
                ProcessSwitchId(ID);
            }

            // If the Index parameter received a value
            if (Index.HasValue)
            {
                ProcessIndexSwitch(Index.Value);
            }

            // If the PlaybackMute parameter received a value
            if (PlaybackMute.HasValue)
            {
                ProcessSwitchPlaybackMute(PlaybackMute.Value);
            }

            // If the PlaybackMuteToggle paramter was called
            if (PlaybackMuteToggle)
            {
                ProcessSwitchPlaybackMuteToggle();
            }

            // If the PlaybackVolume parameter received a value
            if (PlaybackVolume.HasValue)
            {
                ProcessSwitchPlaybackVolume(PlaybackVolume.Value);
            }

            // If the RecordingMute parameter received a value
            if (RecordingMute.HasValue)
            {
                ProcessSwitchRecordingMute(RecordingMute.Value);
            }

            // If the RecordingMuteToggle paramter was called
            if (RecordingMuteToggle)
            {
                ProcessSwitchRecordingMuteToggle();
            }

            // If the RecordingVolume parameter received a value
            if (RecordingVolume.HasValue)
            {
                ProcessSwitchRecordingVolume(RecordingVolume.Value);
            }
        }


        private void ProcessSwitchInputObject(AudioDevice inputObject)
        {
            // Create a new MultiMediaDeviceEnumerator
            var devEnum = new MMDeviceEnumerator();

            // Create a MMDeviceCollection of every devices that are enabled
            var deviceCollection = devEnum.EnumerateAudioEndPoints(DataFlows.All, DeviceStates.DEVICE_STATE_ACTIVE);

            // For every MMDevice in DeviceCollection
            for (int i = 0; i < deviceCollection.Count; i++)
            {
                // If this MMDevice's ID is the same as the ID of the MMDevice received by the InputObject parameter
                if (deviceCollection[i].ID == inputObject.ID)
                {
                    // Create a new audio PolicyConfigClient
                    var client = new PolicyConfigClient();

                    // Using PolicyConfigClient, set the given device as the default playback device
                    client.SetDefaultEndpoint(deviceCollection[i].ID, DeviceRoles.Multimedia);

                    // Output the result of the creation of a new AudioDevice while assigning it the an index, and the MMDevice itself, and a default value of true
                    WriteObject(new AudioDevice(i + 1, deviceCollection[i], true));

                    // Stop checking for other parameters
                    return;
                }
            }

            // Throw an exception about the received device not being found
            throw new System.ArgumentException("No such enabled AudioDevice found");
        }

        private void ProcessSwitchId(string id)
        {
            // Create a new MultiMediaDeviceEnumerator
            var devEnum = new MMDeviceEnumerator();

            // Create a MMDeviceCollection of every devices that are enabled
            var deviceCollection = devEnum.EnumerateAudioEndPoints(DataFlows.All, DeviceStates.DEVICE_STATE_ACTIVE);

            // For every MMDevice in DeviceCollection
            for (var i = 0; i < deviceCollection.Count; i++)
            {
                // If this MMDevice's ID is the same as the string received by the ID parameter
                if (string.Equals(deviceCollection[i].ID, id, System.StringComparison.CurrentCultureIgnoreCase))
                {
                    // Create a new audio PolicyConfigClient
                    var client = new PolicyConfigClient();

                    // Using PolicyConfigClient, set the given device as the default device (for its type)
                    client.SetDefaultEndpoint(deviceCollection[i].ID, DeviceRoles.Multimedia);

                    // Output the result of the creation of a new AudioDevice while assigning it the index, and the MMDevice itself, and a default value of true
                    WriteObject(new AudioDevice(i + 1, deviceCollection[i], true));

                    // Stop checking for other parameters
                    return;
                }
            }

            // Throw an exception about the received ID not being found
            throw new System.ArgumentException("No enabled AudioDevice found with that ID");
        }

        private void ProcessIndexSwitch(int index)
        {
            // Create a new MultiMediaDeviceEnumerator
            var devEnum = new MMDeviceEnumerator();

            // Create a MMDeviceCollection of every devices that are enabled
            var deviceCollection = devEnum.EnumerateAudioEndPoints(DataFlows.All, DeviceStates.DEVICE_STATE_ACTIVE);

            // If the Index is valid
            if (index >= 1 && index <= deviceCollection.Count)
            {
                // Create a new audio PolicyConfigClient
                var client = new PolicyConfigClient();

                // Using PolicyConfigClient, set the given device as the default device (for its type)
                client.SetDefaultEndpoint(deviceCollection[index - 1].ID, DeviceRoles.Multimedia);

                // Output the result of the creation of a new AudioDevice while assigning it the index, and the MMDevice itself, and a default value of true
                WriteObject(new AudioDevice(index, deviceCollection[index - 1], true));

                // Stop checking for other parameters
                return;
            }

            // Throw an exception about the received Index not being found
            throw new System.ArgumentException("No enabled AudioDevice found with that Index");
        }

        private static void ProcessSwitchPlaybackMute(bool recordingMute)
        {
            // Create a new MultiMediaDeviceEnumerator
            var devEnum = new MMDeviceEnumerator();

            // Collect default devices
            var defaultMultimediaPlayback = devEnum.GetDefaultAudioEndpoint(DataFlows.Render, DeviceRoles.Multimedia);

            if (defaultMultimediaPlayback == null)
            {
                throw new System.ArgumentException("No default playback device found");
            }

            // Set the mute state of the default recording device to that of the boolean value received by the Cmdlet
            defaultMultimediaPlayback.AudioEndpointVolume.Mute = recordingMute;
        }

        private static void ProcessSwitchPlaybackMuteToggle()
        {
            // Create a new MultiMediaDeviceEnumerator
            var devEnum = new MMDeviceEnumerator();

            // Collect default devices
            var defaultMultimediaPlayback = devEnum.GetDefaultAudioEndpoint(DataFlows.Render, DeviceRoles.Multimedia);

            if (defaultMultimediaPlayback == null)
            {
                throw new System.ArgumentException("No default playback device found");
            }

            // Toggle the mute state of the default recording device
            defaultMultimediaPlayback.AudioEndpointVolume.Mute = !defaultMultimediaPlayback.AudioEndpointVolume.Mute;
        }

        private static void ProcessSwitchPlaybackVolume(float playbackVolume)
        {
            // Create a new MultiMediaDeviceEnumerator
            var devEnum = new MMDeviceEnumerator();

            // Collect default devices
            var defaultMultimediaPlayback = devEnum.GetDefaultAudioEndpoint(DataFlows.Render, DeviceRoles.Multimedia);

            if (defaultMultimediaPlayback == null)
            {
                throw new System.ArgumentException("No default playback device found");
            }

            // Set the volume level of the default playback device to that of the float value received by the PlaybackVolume parameter
            defaultMultimediaPlayback.AudioEndpointVolume.MasterVolumeLevelScalar = playbackVolume / 100.0f;
        }
        
        private static void ProcessSwitchRecordingMute(bool recordingMute)
        {
            // Create a new MultiMediaDeviceEnumerator
            var devEnum = new MMDeviceEnumerator();

            // Collect default devices
            var defaultMultimediaRecorder = devEnum.GetDefaultAudioEndpoint(DataFlows.Capture, DeviceRoles.Multimedia);

            if (defaultMultimediaRecorder == null)
            {
                throw new System.ArgumentException("No default recording device found");
            }

            // Set the mute state of the default recording device to that of the boolean value received by the Cmdlet
            defaultMultimediaRecorder.AudioEndpointVolume.Mute = recordingMute;
        }

        private static void ProcessSwitchRecordingMuteToggle()
        {
            // Create a new MultiMediaDeviceEnumerator
            var devEnum = new MMDeviceEnumerator();

            // Collect default devices
            var defaultMultimediaRecorder = devEnum.GetDefaultAudioEndpoint(DataFlows.Capture, DeviceRoles.Multimedia);

            if (defaultMultimediaRecorder == null)
            {
                throw new System.ArgumentException("No default recording device found");
            }

            // Toggle the mute state of the default recording device
            defaultMultimediaRecorder.AudioEndpointVolume.Mute = !defaultMultimediaRecorder.AudioEndpointVolume.Mute;
        }

        private static void ProcessSwitchRecordingVolume(float recordingVolume)
        {
            // Create a new MultiMediaDeviceEnumerator
            var devEnum = new MMDeviceEnumerator();

            // Collect default devices
            var defaultMultimediaRecorder = devEnum.GetDefaultAudioEndpoint(DataFlows.Capture, DeviceRoles.Multimedia);

            if (defaultMultimediaRecorder == null)
            {
                throw new System.ArgumentException("No default recording device found");
            }

            // Set the volume level of the default recording device to that of the float value received by the RecordingVolume parameter
            defaultMultimediaRecorder.AudioEndpointVolume.MasterVolumeLevelScalar = recordingVolume / 100.0f;
        }
    }
}