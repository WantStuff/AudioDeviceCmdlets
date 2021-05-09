Import-Module $PSScriptRoot\AudioDeviceCmdlets.dll

Clear-Host



Write-Host ""
Write-Host ""
Write-Host "All Devices"
Write-Host "-----------"
Get-AudioDevice -List


Write-Host ""
Write-Host ""
Write-Host "By Id"
Write-Host "-----"
Get-AudioDevice -Id '{0.0.0.00000000}.{a5fbb7af-4b5d-4fef-a115-b23e9f471039}'


Write-Host ""
Write-Host ""
Write-Host "By Index"
Write-Host "--------"
Get-AudioDevice -Index 3


Write-Host ""
Write-Host ""
Write-Host "Multimedia Playback Device"
Write-Host "--------------------------"
Get-AudioDevice -MultimediaPlayback


Write-Host ""
Write-Host ""
Write-Host "Multimedia Recording Device"
Write-Host "---------------------------"
Get-AudioDevice -MultimediaRecording


Write-Host ""
Write-Host ""
Write-Host "Communication Playback Device"
Write-Host "-----------------------------"
Get-AudioDevice -CommunicationPlayback


Write-Host ""
Write-Host ""
Write-Host "Communication Recording Device"
Write-Host "-----------------------------"
Get-AudioDevice -CommunicationRecording








Write-Host ""
Write-Host ""
Write-Host "Set Default Multimedia and Communication Device using Id"
Write-Host "--------------------------------------------------------"
Set-AudioDevice -Id "{0.0.0.00000000}.{91fbaacc-267c-4426-b6f5-fd0488aa0f4b}" -DefaultMultimedia -DefaultCommunication


Write-Host ""
Write-Host ""
Write-Host "Mute and set 50% Volume using Index"
Write-Host "-----------------------------------"
Set-AudioDevice -Index 1 -Mute $true -Volume 50


Start-Sleep 2


Write-Host ""
Write-Host ""
Write-Host "Set Default Multimedia Device using InputObject"
Write-Host "-----------------------------------------------"
Get-AudioDevice -List | Where Type -Like 'Playback' | Where Name -Like '*Astro MixAmp Pro Game*' | Set-AudioDevice -Default


Write-Host ""
Write-Host ""
Write-Host "Set Default Communication Device using InputObject"
Write-Host "--------------------------------------------------"
Get-AudioDevice -List | Where Type -Like 'Playback' | Where Name -Like '*Astro MixAmp Pro Voice*' | Set-AudioDevice -DefaultCommunication


Write-Host ""
Write-Host ""
Write-Host "Toggle Mute and 100% Volume using Index"
Write-Host "---------------------------------------"
Set-AudioDevice -Index 1 -MuteToggle -Volume 100
