# AudioDeviceCmdlets

## Description

AudioDeviceCmdlets is a suite of PowerShell Cmdlets to control audio devices on Windows.

This is a fork of the work by Francois Gendron's [AudioDeviceCmdlets](https://github.com/frgnca/AudioDeviceCmdlets) with an overhauled and simplified user interface, and additional features to manage Communications devices.

## Features

* List all audio devices
* Get or Set the default playback or recording **multimedia** device
* Get or Set the default playback or recording **communications** device
* Get or Set volume and mute state of an audio device

## Import Cmdlet to PowerShell

Download a pre-build copy of [AudioDeviceCmdlets.dll](https://github.com/WantStuff/AudioDeviceCmdlets/releases/download/5.0.1/AudioDeviceCmdlets.dll) or use Visual Studio to [build](#Build-Cmdlet-from-source) your own.

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
```

```PowerShell
Set-AudioDevice <AudioDevice> [<Action Parameters>]  # Identifies a device using the pipeline input
                -Id <string> [<Action Parameters>]   # Identifies a device using the devices ID
                -Index <int> [<Action Parameters>]   # Identifies a device using its corresponding Index

<Action Parameters> :=
                 -MultimediaDefault         # Sets the identified device as the default Multimedia device
                 -CommunicationDefault      # Sets the identified device as the default Communications device
                 -Mute=<bool>               # Mutes or unmutes the identified device
                 -MuteToggle                # Toggles the mute state of the identified device
                 -Volume=<int>              # Sets the volume of the identified device to the specified percentage
```

## Usage Examples

### Get-AudioDevice -List

```PowerShell
PS D:\> Get-AudioDevice -List

Index                : 1
Id                   : {0.0.0.00000000}.{7df6e4fe-c811-47da-8540-6237103ff88e}
Name                 : Headphones (Astro MixAmp Pro Game)
Type                 : Playback
MultimediaDefault    : True
CommunicationDefault : False
State                : Active
Mute                 : False
Volume               : 100

Index                : 2
Id                   : {0.0.0.00000000}.{91fbaacc-267c-4426-b6f5-fd0488aa0f4b}
Name                 : Speakers/Headphones (Realtek(R) Audio)
Type                 : Playback
MultimediaDefault    : False
CommunicationDefault : False
State                : Active
Mute                 : False
Volume               : 100

Index                : 3
Id                   : {0.0.0.00000000}.{a5fbb7af-4b5d-4fef-a115-b23e9f471039}
Name                 : Headset and Mic (Astro MixAmp Pro Voice)
Type                 : Playback
MultimediaDefault    : False
CommunicationDefault : True
State                : Active
Mute                 : False
Volume               : 100

Index                : 4
Id                   : {0.0.1.00000000}.{a84fb96c-2424-49f6-b1c3-9fa8f553c791}
Name                 : Microphone (Realtek(R) Audio)
Type                 : Recording
MultimediaDefault    : False
CommunicationDefault : False
State                : Active
Mute                 : False
Volume               : 100

Index                : 5
Id                   : {0.0.1.00000000}.{b55928b7-618c-4906-ab4b-bb5ac12626ba}
Name                 : Line (Astro MixAmp Pro Game)
Type                 : Recording
MultimediaDefault    : False
CommunicationDefault : False
State                : Active
Mute                 : False
Volume               : 100

Index                : 6
Id                   : {0.0.1.00000000}.{e5e7230f-7c5e-4fda-881f-9e6f3cf1f618}
Name                 : Headset Microphone (Astro MixAmp Pro Voice)
Type                 : Recording
MultimediaDefault    : True
CommunicationDefault : True
State                : Active
Mute                 : False
Volume               : 100
```

### Get-AudioDevice | Where-Object

```PowerShell
PS D:\> Get-AudioDevice | Where-Object CommunicationDefault -eq $true

Index                : 3
Id                   : {0.0.0.00000000}.{a5fbb7af-4b5d-4fef-a115-b23e9f471039}
Name                 : Headset and Mic (Astro MixAmp Pro Voice)
Type                 : Playback
MultimediaDefault    : False
CommunicationDefault : True
State                : Active
Mute                 : False
Volume               : 100

Index                : 6
Id                   : {0.0.1.00000000}.{e5e7230f-7c5e-4fda-881f-9e6f3cf1f618}
Name                 : Headset Microphone (Astro MixAmp Pro Voice)
Type                 : Recording
MultimediaDefault    : True
CommunicationDefault : True
State                : Active
Mute                 : False
Volume               : 100
```

### Set-AudioDevice -MultimediaDefault -CommunicationDefault

```PowerShell
PS D:\> Set-AudioDevice -Id "{0.0.0.00000000}.{91fbaacc-267c-4426-b6f5-fd0488aa0f4b}" -MultimediaDefault -CommunicationDefault

Index                : 2
Id                   : {0.0.0.00000000}.{91fbaacc-267c-4426-b6f5-fd0488aa0f4b}
Name                 : Speakers/Headphones (Realtek(R) Audio)
Type                 : Playback
MultimediaDefault    : True
CommunicationDefault : True
State                : Active
Mute                 : False
Volume               : 100
```

## Build Cmdlet from source

1. Using Visual Studio, open the Solution from the `SOURCE` folder  
File -> Open -> Project/Solution...
  
2. Set solution configuration to 'Release'  
Build -> Configuration Manager...

3. Build Cmdlet  
Build -> Build AudioDeviceCmdlets

## Attribution

Based on code posted to Code Project by Ray Molenkamp with comments and suggestions by MadMidi  
<http://www.codeproject.com/Articles/18520/Vista-Core-Audio-API-Master-Volume-Control>

Based on code posted to GitHub by Chris Hunt  
<https://github.com/cdhunt/WindowsAudioDevice-Powershell-Cmdlet>

Based on code posted to GitHub by Francois Gendron  
<https://github.com/frgnca/AudioDeviceCmdlets>
