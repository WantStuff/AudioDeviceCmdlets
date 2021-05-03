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
    [Cmdlet(VerbsCommon.Get, "AudioDevice")]
    public class GetAudioDeviceCmdlet : Cmdlet
    {
        // Parameter called to list all devices
        [Parameter(Mandatory = true, Position = 0, ParameterSetName = nameof(List))]
        public SwitchParameter List { get; set; }

        // Parameter receiving the ID of the device to get
        [Parameter(Mandatory = true, Position = 0, ParameterSetName = nameof(ID))]
        public string ID { get; set; }

        // Parameter receiving the Index of the device to get
        [ValidateRange(1, 42)]
        [Parameter(Mandatory = true, Position = 0, ParameterSetName = nameof(Index))]
        public int? Index { get; set; }

        // Parameter called to list the default playback device
        [Parameter(Mandatory = true, Position = 0, ParameterSetName = nameof(Playback))]
        public SwitchParameter Playback { get; set; }

        // Parameter called to list the default playback device's mute state
        [Parameter(Mandatory = true, Position = 0, ParameterSetName = nameof(PlaybackMute))]
        public SwitchParameter PlaybackMute { get; set; }

        // Parameter called to list the default playback device's volume
        [Parameter(Mandatory = true, Position = 0, ParameterSetName = nameof(PlaybackVolume))]
        public SwitchParameter PlaybackVolume { get; set; }

        // Parameter called to list the default recording device
        [Parameter(Mandatory = true, Position = 0, ParameterSetName = nameof(Recording))]
        public SwitchParameter Recording { get; set; }

        // Parameter called to list the default recording device' mute state
        [Parameter(Mandatory = true, Position = 0, ParameterSetName = nameof(RecordingMute))]
        public SwitchParameter RecordingMute { get; set; }

        // Parameter called to list the default recording device' volume
        [Parameter(Mandatory = true, Position = 0, ParameterSetName = nameof(RecordingVolume))]
        public SwitchParameter RecordingVolume { get; set; }


        // Cmdlet execution
        protected override void ProcessRecord()
        {
            // If the List switch parameter was called
            if (List)
            {
                ProcessSwitchList();
                return;
            }

            // If the ID parameter received a value
            if (!string.IsNullOrEmpty(ID))
            {
                ProcessSwitchId(ID);
                return;
            }

            // If the Index parameter received a value
            if (Index.HasValue)
            {
                ProcessIndexSwitch(Index.Value);
                return;
            }

            // If the Playback switch parameter was called
            if (Playback)
            {
                ProcessSwitchPlayback();
                return;
            }

            // If the PlaybackMute switch parameter was called
            if (PlaybackMute)
            {
                ProcessSwitchPlaybackMute();
                return;
            }

            // If the PlaybackVolume switch parameter was called
            if (PlaybackVolume)
            {
                ProcessSwitchPlaybackVolume();
                return;
            }

            // If the Recording switch parameter was called
            if (Recording)
            {
                ProcessSwitchRecording();
                return;
            }

            // If the RecordingMute switch parameter was called
            if (RecordingMute)
            {
                ProcessSwitchRecordingMute();
                return;
            }

            // If the RecordingVolume switch parameter was called
            if (RecordingVolume)
            {
                ProcessSwitchRecordingVolume();
                return;
            }
        }


        private void ProcessSwitchList()
        {
            // Create a new MultiMediaDeviceEnumerator
            var devEnum = new MMDeviceEnumerator();

            // Create a MMDeviceCollection of every devices that are enabled
            var deviceCollection = devEnum.EnumerateAudioEndPoints(DataFlows.All, DeviceStates.DEVICE_STATE_ACTIVE);

            // Collect default devices
            var defaultMultimediaRecorder = devEnum.GetDefaultAudioEndpoint(DataFlows.Capture, DeviceRoles.Multimedia);
            var defaultMultimediaPlayback = devEnum.GetDefaultAudioEndpoint(DataFlows.Render, DeviceRoles.Multimedia);

            // For every MMDevice in DeviceCollection
            for (var i = 0; i < deviceCollection.Count; i++)
            {
                // If this MMDevice's ID is either, the same the default playback device's ID, or the same as the default recording device's ID
                var isMultimediaDefault =
                    deviceCollection[i].ID == defaultMultimediaRecorder?.ID ||
                    deviceCollection[i].ID == defaultMultimediaPlayback?.ID;

                // Output the result of the creation of a new AudioDevice while assining it an index, and the MMDevice itself
                WriteObject(new AudioDevice(i + 1, deviceCollection[i], isMultimediaDefault));
            }
        }

        private void ProcessSwitchId(string id)
        {
            // Create a new MultiMediaDeviceEnumerator
            var devEnum = new MMDeviceEnumerator();

            // Create a MMDeviceCollection of every devices that are enabled
            var deviceCollection = devEnum.EnumerateAudioEndPoints(DataFlows.All, DeviceStates.DEVICE_STATE_ACTIVE);

            // Collect default devices
            var defaultMultimediaRecorder = devEnum.GetDefaultAudioEndpoint(DataFlows.Capture, DeviceRoles.Multimedia);
            var defaultMultimediaPlayback = devEnum.GetDefaultAudioEndpoint(DataFlows.Render, DeviceRoles.Multimedia);

            // For every MMDevice in DeviceCollection
            for (var i = 0; i < deviceCollection.Count; i++)
            {
                // If this MMDevice's ID is the same as the string received by the ID parameter
                if (string.Compare(deviceCollection[i].ID, id, System.StringComparison.CurrentCultureIgnoreCase) == 0)
                {
                    // If this MMDevice's ID is either, the same the default playback device's ID, or the same as the default recording device's ID
                    var isMultimediaDefault =
                        deviceCollection[i].ID == defaultMultimediaRecorder?.ID ||
                        deviceCollection[i].ID == defaultMultimediaPlayback?.ID;

                    // Output the result of the creation of a new AudioDevice while assining it an index, and the MMDevice itself
                    WriteObject(new AudioDevice(i + 1, deviceCollection[i], isMultimediaDefault));

                    // Stop checking for other parameters
                    return;
                }
            }

            // Throw an exception about the received ID not being found
            throw new System.ArgumentException("No AudioDevice with that ID");
        }

        private void ProcessIndexSwitch(int index)
        {
            // Create a new MultiMediaDeviceEnumerator
            var devEnum = new MMDeviceEnumerator();

            // Create a MMDeviceCollection of every devices that are enabled
            var deviceCollection = devEnum.EnumerateAudioEndPoints(DataFlows.All, DeviceStates.DEVICE_STATE_ACTIVE);

            // Collect default devices
            var defaultMultimediaRecorder = devEnum.GetDefaultAudioEndpoint(DataFlows.Capture, DeviceRoles.Multimedia);
            var defaultMultimediaPlayback = devEnum.GetDefaultAudioEndpoint(DataFlows.Render, DeviceRoles.Multimedia);

            // If the Index is valid
            if (index >= 1 && index <= deviceCollection.Count)
            {
                // If this MMDevice's ID is either, the same the default playback device's ID, or the same as the default recording device's ID
                var isMultimediaDefault =
                    deviceCollection[index - 1].ID == defaultMultimediaRecorder?.ID ||
                    deviceCollection[index - 1].ID == defaultMultimediaPlayback?.ID;

                // Output the result of the creation of a new AudioDevice while assining it the an index, and the MMDevice itself
                WriteObject(new AudioDevice(index, deviceCollection[index - 1], isMultimediaDefault));

                // Stop checking for other parameters
                return;
            }

            // Throw an exception about the received Index not being found
            throw new System.ArgumentException("No AudioDevice with that Index");
        }

        private void ProcessSwitchPlayback()
        {
            // Create a new MultiMediaDeviceEnumerator
            var devEnum = new MMDeviceEnumerator();

            // Create a MMDeviceCollection of every devices that are enabled
            var deviceCollection = devEnum.EnumerateAudioEndPoints(DataFlows.All, DeviceStates.DEVICE_STATE_ACTIVE);

            // Collect default devices
            var defaultMultimediaPlayback = devEnum.GetDefaultAudioEndpoint(DataFlows.Render, DeviceRoles.Multimedia);

            // For every MMDevice in DeviceCollection
            for (var i = 0; i < deviceCollection.Count; i++)
            {
                // If this MMDevice's ID is the same the default playback device's ID
                if (deviceCollection[i].ID == defaultMultimediaPlayback?.ID)
                {
                    // Output the result of the creation of a new AudioDevice while assigning it an index, and the MMDevice itself, and a default value of true
                    WriteObject(new AudioDevice(i + 1, deviceCollection[i], true));

                    // Stop checking for other parameters
                    return;
                }
            }
        }

        private void ProcessSwitchPlaybackMute()
        {
            // Create a new MultiMediaDeviceEnumerator
            var devEnum = new MMDeviceEnumerator();

            // Collect default devices
            var defaultMultimediaPlayback = devEnum.GetDefaultAudioEndpoint(DataFlows.Render, DeviceRoles.Multimedia);

            // Output the mute state of the default playback device
            WriteObject(defaultMultimediaPlayback?.AudioEndpointVolume?.Mute);
        }

        private void ProcessSwitchPlaybackVolume()
        {
            // Create a new MultiMediaDeviceEnumerator
            var devEnum = new MMDeviceEnumerator();

            // Collect default devices
            var defaultMultimediaPlayback = devEnum.GetDefaultAudioEndpoint(DataFlows.Render, DeviceRoles.Multimedia);

            // Output the current volume level of the default playback device
            WriteObject($"{defaultMultimediaPlayback?.AudioEndpointVolume?.MasterVolumeLevelScalar * 100}%");
        }

        private void ProcessSwitchRecording()
        {
            // Create a new MultiMediaDeviceEnumerator
            var devEnum = new MMDeviceEnumerator();

            // Create a MMDeviceCollection of every devices that are enabled
            var deviceCollection = devEnum.EnumerateAudioEndPoints(DataFlows.All, DeviceStates.DEVICE_STATE_ACTIVE);

            // Collect default devices
            var defaultMultimediaRecorder = devEnum.GetDefaultAudioEndpoint(DataFlows.Capture, DeviceRoles.Multimedia);

            // For every MMDevice in DeviceCollection
            for (var i = 0; i < deviceCollection.Count; i++)
            {
                // If this MMDevice's ID is the same the default recording device's ID
                if (deviceCollection[i].ID == defaultMultimediaRecorder?.ID)
                {
                    // Output the result of the creation of a new AudioDevice while assigning it an index, and the MMDevice itself, and a default value of true
                    WriteObject(new AudioDevice(i + 1, deviceCollection[i], true));

                    // Stop checking for other parameters
                    return;
                }
            }
        }

        private void ProcessSwitchRecordingMute()
        {
            // Create a new MultiMediaDeviceEnumerator
            var devEnum = new MMDeviceEnumerator();

            // Collect default devices
            var defaultMultimediaRecorder = devEnum.GetDefaultAudioEndpoint(DataFlows.Capture, DeviceRoles.Multimedia);

            // Output the mute state of the default recording device
            WriteObject(defaultMultimediaRecorder?.AudioEndpointVolume?.Mute);
        }

        private void ProcessSwitchRecordingVolume()
        {
            // Create a new MultiMediaDeviceEnumerator
            var devEnum = new MMDeviceEnumerator();

            // Collect default devices
            var defaultMultimediaRecorder = devEnum.GetDefaultAudioEndpoint(DataFlows.Capture, DeviceRoles.Multimedia);

            // Output the current volume level of the default recording device
            WriteObject($"{defaultMultimediaRecorder?.AudioEndpointVolume?.MasterVolumeLevelScalar * 100}%");
        }
    }
}