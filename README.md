# AudioDeviceCmdlets

## Description

AudioDeviceCmdlets is a suite of PowerShell Cmdlets to control audio devices on Windows.

Forked from Francois Gendron's [AudioDeviceCmdlets](https://github.com/frgnca/AudioDeviceCmdlets) to add/improve feature.

## Features

* List all audio devices
* Get or Set the default playback or recording multimedia device
* Get or Set the default playback or recording communications device
* Get or Set volume and mute state of an audio device

## Import Cmdlet to PowerShell

Build `AudioDeviceCmdlets.dll` in [Visual Studio](#Build-Cmdlet-from-source).

```PowerShell
New-Item "$($profile | split-path)\Modules\AudioDeviceCmdlets" -Type directory -Force
Copy-Item "C:\Path\to\AudioDeviceCmdlets.dll" "$($profile | split-path)\Modules\AudioDeviceCmdlets\AudioDeviceCmdlets.dll"
Set-Location "$($profile | Split-Path)\Modules\AudioDeviceCmdlets"
Get-ChildItem | Unblock-File
Import-Module AudioDeviceCmdlets
```

## Usage

```PowerShell
Get-AudioDevice [-List]                   # Outputs a list of all devices as <AudioDevice>
                -Id <string>              # Outputs the device with the ID corresponding to the given <string>
                -Index <int>              # Outputs the device with the Index corresponding to the given <int>
                -Playback                 # Same as -MultimediaPlayback
                -Recording                # Same as -MultimediaRecording
                -MultimediaPlayback       # Outputs the default Multimedia playback device as <AudioDevice>
                -MultimediaRecording      # Outputs the default Multimedia recording device as <AudioDevice>
                -CommunicationsPlayback   # Outputs the default Communications playback device as <AudioDevice>
                -CommunicationsRecording  # Outputs the default Communications recording device as <AudioDevice>
```

```PowerShell
Set-AudioDevice <AudioDevice> [<Action Parameters>]  # Identifies a device using the pipeline input
                -Id <string> [<Action Parameters>]   # Identifies a device using the devices ID
                -Index <int> [<Action Parameters>]   # Identifies a device using its corresponding Index

<Action Parameters> :=
                 -Default                   # Sets the identified device as the default Multimedia device
                 -MultimediaDefault         # Sets the identified device as the default Multimedia device
                 -DefaultCommunication      # Sets the identified device as the default Communications device
                 -Mute=<bool>               # Mutes or unmutes the identified device
                 -MuteToggle                # Toggles the mute state of the identified device
                 -Volume=<int>              # Sets the volume of the identified device to the specified percentage
```

## Build Cmdlet from source

1. Using Visual Studio Community, create new project from SOURCE folder  
File -> New -> Project From Existing Code...
  
    Type of project: Visual C#
    Folder: SOURCE
    Name: AudioDeviceCmdlets
    Output type: Class Library

2. Install System.Management.Automation NuGet package  
Project -> Manage NuGet Packages...

    Browse: System.Management.Automation
    Install: v6.3+

3. Set project properties  
Project -> AudioDeviceCmdlets Properties...

    Assembly name: AudioDeviceCmdlets
    Target framework: .NET Framework 4.5+

4. Set solution configuration  
Build -> Configuration Manager...

    Active solution configuration: Release

5. Build Cmdlet  
Build -> Build AudioDeviceCmdlets

    AudioDeviceCmdlets\bin\Release\AudioDeviceCmdlets.dll

## Attribution

Based on code posted to Code Project by Ray Molenkamp with comments and suggestions by MadMidi  
<http://www.codeproject.com/Articles/18520/Vista-Core-Audio-API-Master-Volume-Control>

Based on code posted to GitHub by Chris Hunt  
<https://github.com/cdhunt/WindowsAudioDevice-Powershell-Cmdlet>

Based on code posted to GitHub by Francois Gendron  
<https://github.com/frgnca/AudioDeviceCmdlets>
