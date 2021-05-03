Import-Module $PSScriptRoot\AudioDeviceCmdlets.dll


Get-AudioDevice -List | Where Type -Like 'Playback' | Where Name -Like '*Real*' | Set-AudioDeviceCommunication -Verbose
