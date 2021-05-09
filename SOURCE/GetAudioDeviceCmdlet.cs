/*
  Inspired by the work done by Francois Gendron <fg@frgn.ca>
  https://github.com/frgnca/AudioDeviceCmdlets
*/

using System;
using System.Management.Automation;
using System.Runtime.CompilerServices;
using AudioDeviceCmdlets.CoreAudioApi;
using AudioDeviceCmdlets.CoreAudioApi.Enums;

namespace AudioDeviceCmdlets
{
    [Cmdlet(VerbsCommon.Get, nameof(AudioDevice), DefaultParameterSetName = nameof(List))]
    [OutputType(typeof(AudioDevice))]
    public class GetAudioDeviceCmdlet : Cmdlet
    {
        [Parameter(Mandatory = false, Position = 0, ParameterSetName = nameof(List))]
        public SwitchParameter List { get; set; }

        [Parameter(Mandatory = true, Position = 0, ParameterSetName = nameof(Id))]
        public string Id { get; set; }

        [ValidateRange(1, 42)]
        [Parameter(Mandatory = true, Position = 0, ParameterSetName = nameof(Index))]
        public int Index { get; set; }

        [Alias("Playback")]
        [Parameter(Mandatory = true, Position = 0, ParameterSetName = nameof(MultimediaPlayback))]
        public SwitchParameter MultimediaPlayback { get; set; }

        [Alias("Recording")]
        [Parameter(Mandatory = true, Position = 0, ParameterSetName = nameof(MultimediaRecording))]
        public SwitchParameter MultimediaRecording { get; set; }

        [Parameter(Mandatory = true, Position = 0, ParameterSetName = nameof(CommunicationPlayback))]
        public SwitchParameter CommunicationPlayback { get; set; }

        [Parameter(Mandatory = true, Position = 0, ParameterSetName = nameof(CommunicationRecording))]
        public SwitchParameter CommunicationRecording { get; set; }


        protected override void ProcessRecord()
        {
            if (!string.IsNullOrEmpty(Id))
            {
                GetDeviceById(Id);
                return;
            }

            if (Index > 0)
            {
                GetDeviceByIndex(Index);
                return;
            }

            if (MultimediaPlayback)
            {
                GetPlaybackDevice(DeviceRoles.Multimedia);
                return;
            }

            if (MultimediaRecording)
            {
                GetRecordingDevice(DeviceRoles.Multimedia);
                return;
            }

            if (CommunicationPlayback)
            {
                GetPlaybackDevice(DeviceRoles.Communication);
                return;
            }

            if (CommunicationRecording)
            {
                GetRecordingDevice(DeviceRoles.Communication);
                return;
            }

            // Default = -List
            GetDeviceList();
        }


        private void GetDeviceList()
        {
            var deviceRepo = new MMDeviceRepository();
            foreach (var mmDevice in deviceRepo.ToList())
            {
                WriteObject(new AudioDevice(mmDevice));
            }
        }

        private void GetDeviceById(string id)
        {
            var deviceRepo = new MMDeviceRepository();

            var mmDevice = deviceRepo.Find(x => x.Id == id);
            if (mmDevice == null)
            {
                throw new ArgumentException("No AudioDevice with that ID");
            }

            WriteObject(new AudioDevice(mmDevice));
        }

        private void GetDeviceByIndex(int index)
        {
            var deviceRepo = new MMDeviceRepository();

            var mmDevice = deviceRepo.Find(x => x.Index == index);
            if (mmDevice == null)
            {
                throw new ArgumentException("No AudioDevice with that Index");
            }

            WriteObject(new AudioDevice(mmDevice));
        }

        private void GetPlaybackDevice(DeviceRoles deviceRole)
        {
            var deviceRepo = new MMDeviceRepository();

            var mmDevice = deviceRepo.GetDefaultPlaybackDevice(deviceRole);
            if (mmDevice == null)
            {
                throw new ArgumentException("No default playback found");
            }

            WriteObject(new AudioDevice(mmDevice));
        }

        private void GetRecordingDevice(DeviceRoles deviceRole)
        {
            var deviceRepo = new MMDeviceRepository();

            var mmDevice = deviceRepo.GetDefaultRecordingDevice(deviceRole);
            if (mmDevice == null)
            {
                throw new ArgumentException("No default recording device found");
            }

            WriteObject(new AudioDevice(mmDevice));
        }
    }
}