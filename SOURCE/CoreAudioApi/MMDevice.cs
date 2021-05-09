/*
  Based on the work done by Francois Gendron <fg@frgn.ca>
  https://github.com/frgnca/AudioDeviceCmdlets
*/

using System;
using System.Runtime.InteropServices;
using AudioDeviceCmdlets.CoreAudioApi.Enums;
using AudioDeviceCmdlets.CoreAudioApi.Interfaces;
using AudioDeviceCmdlets.CoreAudioApi.Structs;

namespace AudioDeviceCmdlets.CoreAudioApi
{
    // ReSharper disable once InconsistentNaming
    public class MMDevice
    {
        private readonly IMMDevice _realDevice;
        private PropertyStore _propertyStore;
        private AudioMeterInformation _audioMeterInformation;
        private AudioEndpointVolume _audioEndpointVolume;
        private AudioSessionManager _audioSessionManager;
        private static Guid _audioMeterInformationGuid = typeof(IAudioMeterInformation).GUID;
        private static Guid _audioEndpointVolumeGuid = typeof(IAudioEndpointVolume).GUID;
        private static Guid _audioSessionManagerGuid = typeof(IAudioSessionManager2).GUID;


        internal MMDevice(IMMDevice realDevice, int index, bool isCommunicationsDefault, bool isMultimediaDefault)
        {
            _realDevice = realDevice;
            Index = index;
            IsCommunicationsDefault = isCommunicationsDefault;
            IsMultimediaDefault = isMultimediaDefault;
        }

        internal MMDevice(IMMDevice realDevice, int index, string defaultCommunicationsPlaybackId, string defaultCommunicationsRecordingId, string defaultMultimediaPlaybackId, string defaultMultimediaRecordingId)
        {
            _realDevice = realDevice;
            Index = index;

            var id = Id;
            IsCommunicationsDefault = defaultCommunicationsPlaybackId == id || defaultCommunicationsRecordingId == id;
            IsMultimediaDefault = defaultMultimediaPlaybackId == id || defaultMultimediaRecordingId == id;
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

        public PropertyStore PropertyStore
        {
            get
            {
                if (_propertyStore == null)
                {
                    IPropertyStore propertyStore;
                    Marshal.ThrowExceptionForHR(_realDevice.OpenPropertyStore(StgmAccesses.STGM_READ, out propertyStore));
                    _propertyStore = new PropertyStore(propertyStore);
                }

                return _propertyStore;
            }
        }

        public string FriendlyName
        {
            get
            {
                try
                {
                    return (string) PropertyStore[PKEY.PKEY_DeviceInterface_FriendlyName].Value;
                }
                catch
                {
                    return null;
                }
            }
        }


        public string Id
        {
            get
            {
                string Result;
                Marshal.ThrowExceptionForHR(_realDevice.GetId(out Result));
                return Result;
            }
        }

        public int Index { get; }

        public bool IsCommunicationsDefault { get; }

        public bool IsMultimediaDefault { get; }

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
