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
    // Set Cmdlet

    // Write Cmdlet
    [Cmdlet(VerbsCommunications.Write, "AudioDevice")]
    public class WriteAudioDeviceCmdlet : Cmdlet
    {
        // Parameter called to output audiometer result of the default playback device as a progress bar
        [Parameter(Mandatory = true, Position = 0, ParameterSetName = "PlaybackMeter")]
        public SwitchParameter PlaybackMeter
        {
            get { return playbackmeter; }
            set { playbackmeter = value; }
        }
        private bool playbackmeter;

        // Parameter called to output audiometer result of the default playback device as a stream of values
        [Parameter(Mandatory = true, Position = 0, ParameterSetName = "PlaybackStream")]
        public SwitchParameter PlaybackStream
        {
            get { return playbackstream; }
            set { playbackstream = value; }
        }
        private bool playbackstream;

        // Parameter called to output audiometer result of the default recording device as a progress bar
        [Parameter(Mandatory = true, Position = 0, ParameterSetName = "RecordingMeter")]
        public SwitchParameter RecordingMeter
        {
            get { return recordingmeter; }
            set { recordingmeter = value; }
        }
        private bool recordingmeter;

        // Parameter called to output audiometer result of the default recording device as a stream of values
        [Parameter(Mandatory = true, Position = 0, ParameterSetName = "RecordingStream")]
        public SwitchParameter RecordingStream
        {
            get { return recordingstream; }
            set { recordingstream = value; }
        }
        private bool recordingstream;

        // Cmdlet execution
        protected override void ProcessRecord()
        {
            // Create a new MultiMediaDeviceEnumerator
            MMDeviceEnumerator DevEnum = new MMDeviceEnumerator();

            // If the PlaybackMeter parameter was called
            if (playbackmeter)
            {
                // Create a new progress bar to output current audiometer result of the default playback device
                ProgressRecord pr = new ProgressRecord(0, DevEnum.GetDefaultAudioEndpoint(DataFlows.Render, DeviceRoles.Multimedia).FriendlyName, "Peak Value");
                // Set the progress bar to zero
                pr.PercentComplete = 0;

                // Loop until interruption ex: CTRL+C
                do
                {
                    // Set progress bar to current audiometer result
                    pr.PercentComplete = System.Convert.ToInt32(DevEnum.GetDefaultAudioEndpoint(DataFlows.Render, DeviceRoles.Multimedia).AudioMeterInformation.MasterPeakValue * 100);

                    // Write current audiometer result as a progress bar
                    WriteProgress(pr);

                    // Wait 100 milliseconds
                    System.Threading.Thread.Sleep(100);
                }
                // Loop interrupted ex: CTRL+C
                while (!Stopping);
            }

            // If the PlaybackStream parameter was called
            if (playbackstream)
            {
                // Loop until interruption ex: CTRL+C
                do
                {
                    // Write current audiometer result as a value
                    WriteObject(System.Convert.ToInt32(DevEnum.GetDefaultAudioEndpoint(DataFlows.Render, DeviceRoles.Multimedia).AudioMeterInformation.MasterPeakValue * 100));

                    // Wait 100 milliseconds
                    System.Threading.Thread.Sleep(100);
                }
                // Loop interrupted ex: CTRL+C
                while (!Stopping);
            }

            // If the RecordingMeter parameter was called
            if (recordingmeter)
            {
                // Create a new progress bar to output current audiometer result of the default recording device
                ProgressRecord pr = new ProgressRecord(0, DevEnum.GetDefaultAudioEndpoint(DataFlows.Capture, DeviceRoles.Multimedia).FriendlyName, "Peak Value");
                // Set the progress bar to zero
                pr.PercentComplete = 0;

                // Loop until interruption ex: CTRL+C
                do
                {
                    // Set progress bar to current audiometer result
                    pr.PercentComplete = System.Convert.ToInt32(DevEnum.GetDefaultAudioEndpoint(DataFlows.Capture, DeviceRoles.Multimedia).AudioMeterInformation.MasterPeakValue * 100);

                    // Write current audiometer result as a progress bar
                    WriteProgress(pr);

                    // Wait 100 milliseconds
                    System.Threading.Thread.Sleep(100);
                }
                // Loop interrupted ex: CTRL+C
                while (!Stopping);
            }

            // If the RecordingStream parameter was called
            if (recordingstream)
            {
                // Loop until interruption ex: CTRL+C
                do
                {
                    // Write current audiometer result as a value
                    WriteObject(System.Convert.ToInt32(DevEnum.GetDefaultAudioEndpoint(DataFlows.Capture, DeviceRoles.Multimedia).AudioMeterInformation.MasterPeakValue * 100));

                    // Wait 100 milliseconds
                    System.Threading.Thread.Sleep(100);
                }
                // Loop interrupted ex: CTRL+C
                while (!Stopping);
            }
        }
    }
}