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
using AudioDeviceCmdlets.CoreAudioApi.Enums;

namespace AudioDeviceCmdlets
{
    [Cmdlet(VerbsCommon.Set, nameof(AudioDevice))]
    public class SetAudioDeviceCmdlet : SetAudioDeviceCmdletBase
    {
        public SetAudioDeviceCmdlet()
        {
            DeviceRole = DeviceRoles.Multimedia;
        }
    }
}