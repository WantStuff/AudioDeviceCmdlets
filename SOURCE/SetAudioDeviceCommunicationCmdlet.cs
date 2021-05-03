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
    [Cmdlet(VerbsCommon.Set, "AudioDeviceCommunication")]
    public class SetAudioDeviceCommunicationCmdlet : Cmdlet
    {
        // Parameter receiving the AudioDevice to set as default
        [Parameter(Mandatory = true, ParameterSetName = "InputObject", ValueFromPipeline = true)]
        public AudioDevice InputObject
        {
            get { return inputObject; }
            set { inputObject = value; }

        }
        private AudioDevice inputObject;

        // Parameter receiving the ID of the device to set as default
        [Parameter(Mandatory = true, Position = 0, ParameterSetName = "ID")]
        public string ID
        {
            get { return id; }
            set { id = value; }
        }
        private string id;

        // Parameter receiving the Index of the device to set as default
        [ValidateRange(1, 42)]
        [Parameter(Mandatory = true, Position = 0, ParameterSetName = "Index")]
        public int? Index
        {
            get { return index; }
            set { index = value; }
        }
        private int? index;



        // Cmdlet execution
        protected override void ProcessRecord()
        {
            // Create a new MultiMediaDeviceEnumerator
            MMDeviceEnumerator DevEnum = new MMDeviceEnumerator();
            // Create a MMDeviceCollection of every devices that are enabled
            MMDeviceCollection DeviceCollection = DevEnum.EnumerateAudioEndPoints(DataFlows.All, DeviceStates.DEVICE_STATE_ACTIVE);

            // If the InputObject parameter received a value
            if (inputObject != null)
            {
                // For every MMDevice in DeviceCollection
                for (int i = 0; i < DeviceCollection.Count; i++)
                {
                    // If this MMDevice's ID is the same as the ID of the MMDevice received by the InputObject parameter
                    if (DeviceCollection[i].ID == inputObject.ID)
                    {
                        // Create a new audio PolicyConfigClient
                        PolicyConfigClient client = new PolicyConfigClient();
                        // Using PolicyConfigClient, set the given device as the default playback device
                        client.SetDefaultEndpoint(DeviceCollection[i].ID, DeviceRoles.Communication);

                        // Output the result of the creation of a new AudioDevice while assining it the an index, and the MMDevice itself, and a default value of true
                        WriteObject(new AudioDevice(i + 1, DeviceCollection[i], true));

                        // Stop checking for other parameters
                        return;
                    }
                }

                // Throw an exception about the received device not being found
                throw new System.ArgumentException("No such enabled AudioDevice found");
            }

            // If the ID parameter received a value
            if (!string.IsNullOrEmpty(id))
            {
                // For every MMDevice in DeviceCollection
                for (int i = 0; i < DeviceCollection.Count; i++)
                {
                    // If this MMDevice's ID is the same as the string received by the ID parameter
                    if (string.Compare(DeviceCollection[i].ID, id, System.StringComparison.CurrentCultureIgnoreCase) == 0)
                    {
                        // Create a new audio PolicyConfigClient
                        PolicyConfigClient client = new PolicyConfigClient();
                        // Using PolicyConfigClient, set the given device as the default device (for its type)
                        client.SetDefaultEndpoint(DeviceCollection[i].ID, DeviceRoles.Communication);

                        // Output the result of the creation of a new AudioDevice while assining it the index, and the MMDevice itself, and a default value of true
                        WriteObject(new AudioDevice(i + 1, DeviceCollection[i], true));

                        // Stop checking for other parameters
                        return;
                    }
                }

                // Throw an exception about the received ID not being found
                throw new System.ArgumentException("No enabled AudioDevice found with that ID");
            }

            // If the Index parameter received a value
            if (index != null)
            {
                // If the Index is valid
                if (index.Value >= 1 && index.Value <= DeviceCollection.Count)
                {
                    // Create a new audio PolicyConfigClient
                    PolicyConfigClient client = new PolicyConfigClient();
                    // Using PolicyConfigClient, set the given device as the default device (for its type)
                    client.SetDefaultEndpoint(DeviceCollection[index.Value - 1].ID, DeviceRoles.Communication);

                    // Output the result of the creation of a new AudioDevice while assining it the index, and the MMDevice itself, and a default value of true
                    WriteObject(new AudioDevice(index.Value, DeviceCollection[index.Value - 1], true));

                    // Stop checking for other parameters
                    return;
                }
                else
                {
                    // Throw an exception about the received Index not being found
                    throw new System.ArgumentException("No enabled AudioDevice found with that Index");
                }
            }
        }
    }
}