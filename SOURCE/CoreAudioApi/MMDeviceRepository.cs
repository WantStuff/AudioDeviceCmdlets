using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using AudioDeviceCmdlets.CoreAudioApi.Enums;
using AudioDeviceCmdlets.CoreAudioApi.Interfaces;

namespace AudioDeviceCmdlets.CoreAudioApi
{
    // ReSharper disable once InconsistentNaming
    public class MMDeviceRepository
    {
        //private readonly IMMDeviceEnumerator _realEnumerator = new MMDeviceEnumeratorCom() as IMMDeviceEnumerator;
        private readonly List<MMDevice> _mmDeviceCache;

        internal MMDeviceRepository()
        {
            // ReSharper disable once SuspiciousTypeConversion.Global
            var deviceEnumerator = new MMDeviceEnumeratorCom() as IMMDeviceEnumerator;

            // Get all devices
            _mmDeviceCache = GetAllDevices(deviceEnumerator, DeviceStates.DEVICE_STATE_ACTIVE)
                .ToList();
        }


        public MMDevice Find(Predicate<MMDevice> match)
        {
            return _mmDeviceCache.Find(match);
        }

        public MMDevice GetDefaultPlaybackDevice(DeviceRoles deviceRole)
        {
            switch (deviceRole)
            {
                case DeviceRoles.Multimedia:
                    return _mmDeviceCache.Find(x => x.IsMultimediaDefault && x.DataFlow == DataFlows.Render);
                case DeviceRoles.Communication:
                    return _mmDeviceCache.Find(x => x.IsCommunicationsDefault && x.DataFlow == DataFlows.Render);
            }

            return null;
        }

        public MMDevice GetDefaultRecordingDevice(DeviceRoles deviceRole)
        {
            switch (deviceRole)
            {
                case DeviceRoles.Multimedia:
                    return _mmDeviceCache.Find(x => x.IsMultimediaDefault && x.DataFlow == DataFlows.Capture);
                case DeviceRoles.Communication:
                    return _mmDeviceCache.Find(x => x.IsCommunicationsDefault && x.DataFlow == DataFlows.Capture);
            }

            return null;
        }

        public IReadOnlyCollection<MMDevice> ToList()
        {
            return _mmDeviceCache;
        }


        private IEnumerable<MMDevice> GetAllDevices(IMMDeviceEnumerator deviceEnumerator, DeviceStates deviceStates)
        {
            // Collect default devices
            var communicationPlaybackId = GetDefaultDeviceId(deviceEnumerator, DataFlows.Render, DeviceRoles.Communication);
            var communicationRecordingId = GetDefaultDeviceId(deviceEnumerator, DataFlows.Capture, DeviceRoles.Communication);
            var multimediaPlaybackId = GetDefaultDeviceId(deviceEnumerator, DataFlows.Render, DeviceRoles.Multimedia);
            var multimediaRecordingId = GetDefaultDeviceId(deviceEnumerator, DataFlows.Capture, DeviceRoles.Multimedia);

            IMMDeviceCollection deviceCollection;
            Marshal.ThrowExceptionForHR(deviceEnumerator.EnumAudioEndpoints(DataFlows.All, deviceStates, out deviceCollection));

            uint count;
            Marshal.ThrowExceptionForHR(deviceCollection.GetCount(out count));

            for (uint i = 0; i < count; i++)
            {
                IMMDevice result;
                deviceCollection.Item(i, out result);

                var mmDevice = new MMDevice(result, 
                    (int) i + 1, 
                    communicationPlaybackId, 
                    communicationRecordingId, 
                    multimediaPlaybackId, 
                    multimediaRecordingId);

                yield return mmDevice;
            }
        }

        private static string GetDefaultDeviceId(IMMDeviceEnumerator deviceEnumerator, DataFlows dataFlow, DeviceRoles role)
        {
            IMMDevice _device = null;
            Marshal.ThrowExceptionForHR(deviceEnumerator.GetDefaultAudioEndpoint(dataFlow, role, out _device));

            string ID;
            Marshal.ThrowExceptionForHR(_device.GetId(out ID));
            return ID;
        }
    }
}