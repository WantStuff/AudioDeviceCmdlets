/*
  Inspired by the work done by Francois Gendron <fg@frgn.ca>
  https://github.com/frgnca/AudioDeviceCmdlets
*/

using System;
using System.Management.Automation;
using AudioDeviceCmdlets.CoreAudioApi;
using AudioDeviceCmdlets.CoreAudioApi.Enums;

namespace AudioDeviceCmdlets
{
    [Cmdlet(VerbsCommon.Set, nameof(AudioDevice))]
    public class SetAudioDeviceCmdlet : Cmdlet
    {
        [Parameter(Mandatory = true, Position = 0, ParameterSetName = nameof(InputObject), ValueFromPipeline = true)]
        public AudioDevice InputObject { get; set; }

        [Parameter(Mandatory = true, Position = 0, ParameterSetName = nameof(Id), ValueFromPipelineByPropertyName = true)]
        public string Id { get; set; }

        [ValidateRange(1, 42)]
        [Parameter(Mandatory = true, Position = 0, ParameterSetName = nameof(Index))]
        public int? Index { get; set; }

        [Parameter(Mandatory = false, Position = 1)]
        public SwitchParameter MultimediaDefault { get; set; }

        [Parameter(Mandatory = false, Position = 1)]
        public SwitchParameter CommunicationDefault { get; set; }

        [Parameter(Mandatory = false, Position = 1)]
        public bool? Mute { get; set; }

        [Parameter(Mandatory = false, Position = 1)]
        public SwitchParameter MuteToggle { get; set; }

        [ValidateRange(0, 100.0f)]
        [Parameter(Mandatory = false, Position = 1)]
        public float? Volume { get; set; }


        protected override void ProcessRecord()
        {

            // Position 0 parameters
            // ---------------------

            MMDevice mmDevice = null;
            if (InputObject != null)
            {
                mmDevice = GetMMDevice(InputObject);
            }
            else if (!string.IsNullOrEmpty(Id))
            {
                mmDevice = GetMMDevice(Id);
            }
            else if (Index.HasValue)
            {
                mmDevice = GetMMDevice(Index.Value);
            }


            // Position 1 parameters
            // ---------------------

            if ((!CommunicationDefault && !Mute.HasValue && !MuteToggle && !Volume.HasValue))
            {
                // If no position 1 parameters were set, default to DefaultMultimedia
                MultimediaDefault = true;
            }

            if (MultimediaDefault)
            {
                SetDefault(mmDevice, DeviceRoles.Multimedia);
            }

            if (CommunicationDefault)
            {
                SetDefault(mmDevice, DeviceRoles.Communication);
            }

            if (Mute.HasValue)
            {
                SetMute(mmDevice, Mute.Value);
            }

            if (MuteToggle)
            {
                ToggleMute(mmDevice);
            }

            if (Volume.HasValue)
            {
                SetVolume(mmDevice, Volume.Value);
            }

            WriteObject(new AudioDevice(mmDevice));
        }


        private static MMDevice GetMMDevice(AudioDevice inputObject)
        {
            var deviceRepo = new MMDeviceRepository();

            var mmDevice = deviceRepo.Find(x => x.Id == inputObject.Id);
            if (mmDevice == null)
            {
                throw new ArgumentException("No such enabled AudioDevice found");
            }

            return mmDevice;
        }

        private static MMDevice GetMMDevice(string id)
        {
            var deviceRepo = new MMDeviceRepository();

            var mmDevice = deviceRepo.Find(x => x.Id == id);
            if (mmDevice == null)
            {
                throw new ArgumentException("No enabled AudioDevice found with that ID");
            }

            return mmDevice;
        }

        private static MMDevice GetMMDevice(int index)
        {
            var deviceRepo = new MMDeviceRepository();

            var mmDevice = deviceRepo.Find(x => x.Index == index);
            if (mmDevice == null)
            {
                throw new ArgumentException("No enabled AudioDevice found with that Index");
            }

            return mmDevice;
        }


        private void SetDefault(MMDevice mmDevice, DeviceRoles deviceRole)
        {
            var client = new PolicyConfigClient();
            client.SetDefaultEndpoint(mmDevice.Id, deviceRole);
        }

        private void SetMute(MMDevice mmDevice, bool mute)
        {
            mmDevice.AudioEndpointVolume.Mute = mute;
        }

        private void ToggleMute(MMDevice mmDevice)
        {
            mmDevice.AudioEndpointVolume.Mute = !mmDevice.AudioEndpointVolume.Mute;
        }

        private void SetVolume(MMDevice mmDevice, float playbackVolume)
        {
            mmDevice.AudioEndpointVolume.MasterVolumeLevelScalar = playbackVolume / 100.0f;
        }
    }
}