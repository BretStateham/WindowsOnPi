# HOL 01 - Installing Windows 10 IoT Core

## Overview

In this lab you will setup the Raspberry Pi, Install Windows 10 IoT Core on it, connect to it, and configure the Pi.  

## Pre-Requisites

- A Computer Running Windows 10 
- A Raspberry Pi 2
- A 5V/2A Power Supply with a Micro USB Connector
- An 8GB or larger Micro SD Card
- A Micro SD Card Reader for your computer
- An Ethernet Port on your computer (A USB Ethernet Dongle is fine)
- An Ethernet Cable
- An HDMI Monitor for the Raspberry Pi
- A USB Keyboard and Mouse for the Raspberry Pi 

## Setup

There are no specific setup steps required prior to starting this lab other than to ensure that you have all of the pre-requisites met. 

## Tasks

- [Task 1: Install the Windows 10 IoT Core for Raspberry Pi 2 Tools on your PC](#Task1)
- [Task 2: Flash the SD Card with Windows 10 IoT Core](#Task2)
- [Task 3: Setting up the Pi and Booting off Windows](#Task3)
- [Task 4: Setting the time on your Pi using SSH](#Task4)

---

<a name="Task1"></a>
### Task 1: Install the Windows 10 IoT Core for Raspberry Pi 2 Tools on Your PC

To boot your Raspberry Pi 2 off of Windows 10 IoT Core we need to get the Windows 10 IoT Core operating system flashed on the Micro SD card used by the Raspberry Pi 2.  This requires that we first install some tools and the source image file on our computer. 

While specific steps will be outlined here, you can always find the latest Windows 10 IoT Core installation Instructions here: [http://ms-iot.github.io/content/en-US/GetStarted.htm](http://ms-iot.github.io/content/en-US/GetStarted.htm)

1. Download the [Windows 10 IoT Core image ISO](http://go.microsoft.com/fwlink/?LinkId=691711)

	> **Note:** If you are at an in-person workshop, the required download files may have been provided on a USB drive or via some alternative source.  Please refer to the event specific locations if they exist.  The ISO file download is appx. 500MB in size and can cause issues when downloading over the event WiFi simultaneously with other event attendees. 

	![01005-DownloadIso](images/01005-downloadiso.png?raw=true "Download ISO")

1. Once you have downloaded the .iso file, right click on it, and select "**Mount**" from the popup menu:

	![01010-MountIso](images/01010-MountIso.png?raw=true "Mount ISO")

1. When you mount the .iso file, a new window should appear to show you it's contents.  Double click on the .msi file to run the installer:

	![01020-RunInstaller](images/01020-runinstaller.png?raw=true "Run Installer")

1. Complete the Setup Wizard:

	![01030-SetupWizard](images/01030-setupwizard.png?raw=true "Setup Wizard")

1. Once the setup wizard has completed, you can un-mount the .iso file by right clicking on the virtual DVD drive that was created when you mounted it, and selecting "**Eject**" from the popup menu 

	![01040-EjectIso](images/01040-ejectiso.png?raw=true "Eject ISO")

1. Finally, the Windows 10 Iot Core tools and image should now be on your computer under the "**C:\Program Files (x86)\Microsoft IoT**" directory.

	- The "**DISM**" folder contains the "**Deployment Image Servicing and Management*"" utility that is used to apply the image to the Micro SD card. 
	- The "**FFU\RaspberryPi2**" folder contains the "**flash.ffu**" ("**Full Flash Update**") image file for Windows 10 IoT Core for the Raspberry Pi 2.  This is the actual image that will be flashed to the Micro SD Card. 
	- The "**IoTCoreImageHelper.exe**" utility is used to simplify the use of "**DISM**" to flash the "**flash.ffu**" file to the Micro SD Card. 
	- The "**WindowsIoTCoreWatcher.exe**" utility is used to identify Raspberry Pi 2 devices running Windows 10 IoT Core on your network and provide a simplified means to manage them. 

	![01050-MicrosoftIoTFolder](images/01050-microsoftiotfolder.png?raw=true "Microsoft IoT Folder")

---

<a name="Task2"></a>
### Task 2: Flash the SD Card with Windows 10 IoT Core

In this task, we'll use the utilities and image we installed in the previous task to flash the Micro SD card with Windows 10 IoT Core.  

1. Start by first mounting the Micro SD Card on your computer.

	> **Note**: Use an external SD Card Reader and Micro SD to SD Adapter as needed. 

	![02010-MountMicroSDCard](images/02010-MountMicroSDCard.png?raw=true "Mount Micro SD Card on your Computer")

1. Once the card is inserted, identify the drive letter associated with the SD Card

	> **Note**: Take care to identify the correct drive letter  if you have multiple SD cards mounted on your system.  You don't want to accidentally flash the OS to the wrong SD card. 

	![02020-SDCardDriveLetter](images/02020-sdcarddriveletter.png?raw=true "SD Card Drive Letter")

1. Then press the Windows Key on your computer to open the Windows Start Menu, then type "WindowsIoTImageHelper" to search for the imaging utility we installed previously

	> **Note**: You can also just open the "**C:\Program Files (x86)\Microsoft IoT\IoTCoreImageHelper.exe**" executable in Windows Explorer.

	![02030-OpenImagHelper](images/02030-openimaghelper.png?raw=true "Open Windows IoT Core Image Helper")

1. In the "Windows IoT Core Image Helper" window

	- Select the drive letter associated with your SD card that we identified earlier. 
	- Click the "**Browse**" button, and select the "**C:\Program Files (x86)\Microsoft IoT\FFU\RaspberryPi2\flash.ffu**" image file
	- Click the "**Flash**" button to begin the flashing process

	![02040-FlashTheSdCard](images/02040-flashthesdcard.png?raw=true "Flash the SD Card")

1. When prompted, ensure that you have selected the correct SD card, then click "**Continue**" to confirm the erasure. 

	![02050-ConfirmFormatting](images/02050-confirmformatting.png?raw=true "Confirm Formatting")

1. During flashing, and console window will open showing the progress of the DISM utility as it flashes the image to your SD Card.

	> **Note:** You will likely receive a "**User Account Control**" confirmation dialog.  Confirm by clicking "**Yes**".

	![02060-DISMProgress](images/02060-dismprogress.png?raw=true "DISM Progress")

1. Once the flashing process is complete, the SD card should now have the Windows 10 IoT Core Files on it

	![02070-Windows10IoTCoreFiles](images/02070-windows10iotcorefiles.png?raw=true "Windows 10 IoT Core Files")

1. You can now eject the SD card by right clicking on the drive letter associated with it, and selecting the "**Eject**" from the popup menu

	![02080-EjectSDCard](images/02080-ejectsdcard.png?raw=true "Eject SD Card")

1. Once windows notifies you that the card is safe to remove, you can remove the Micro SD card from your computer.  
	
---

<a name="Task3"></a>
### Task 3: Setting up the Pi and Booting off Windows

We're now ready to hook up the Raspberry Pi, and boot off Windows 10 Iot Core.


<span style="color: red;">**PLEASE READ**</span>:  This lab will use a hardwired Ethernet network connection for the Raspberry Pi rather than a WiFi connection.  Windows 10 IoT Core does support WiFi, but only on [a limited number of supported WiFi dongles](https://ms-iot.github.io/content/en-US/win10/SupportedInterfaces.htm#WiFi-Dongles).  At the time the hardware kits for the labs were assembled the only supported WiFi dongle was the "[Official Raspberry Pi WiFi Dongle](https://www.raspberrypi.org/products/usb-wifi-dongle/)" and they were in very short supply.  Since then the adapters are more readily available and there are additional WiFi adapters being supported. If you have one of the [supported WiFi dongles](https://ms-iot.github.io/content/en-US/win10/SupportedInterfaces.htm#WiFi-Dongles),  you can follow the steps on the "[Using WiFi on your Windows 10 IoT Core device](https://ms-iot.github.io/content/en-US/win10/SetupWiFi.htm)" to configure your Raspberry Pi's WiFi connection.

For the following steps, we will use this hardware configuration:

![03010-LabHardwareConfiguration](images/03010-LabHardwareConfiguration.png?raw=true "Lab Hardware Configuration")

1. Insert the Micro SD Card we flashed above into the slot ***on the back*** of the Raspberry Pi 2.  Ensure that it is inserted completely.  You should hear a ***faint click*** when it is inserted completely.
1. Next, connect the Keyboard, Mouse and HDMI monitor as identified in the diagram above.
1. Plug the ends of the Ethernet Cable into the Raspberry Pi, and the Venue's hardwired Ethernet network.
1. Lastly, connect the Micro USB cable to the Raspberry Pi 2's Power Port on one end and the full size USB cable to the 5V/2A Power Supply on the other end, and plug the Power Supply in to boot the Pi.
1. Watch the screen as the pi boots.  Once it finally loads, a default app will run that displays information about your Pi, and allows you to configure some settings and shutdown or reboot the pi.  Pay attention to the details under the "Raspberry Pi 2" heading to see your Pi's name and IP address.  Make a note of your Pi's "**IP address**" so you can connect to it later.  

	> **Note**: By default, every Pi is named "**minwinpc**", we will change that name in a minute, but each Pi on the network will have a unique "**IP address**".  Initially, we will use the unique "**IP address**" as a way to indentify our Pi on a network on which number of other Pi's with the same "**minwinpc**" name are connected.

	![03020-PiDetails](images/03020-pidetails.png?raw=true "Pi Details")

1. Take some time to explore the other features of the default app by clicking on the buttons along the top

	- Tutorials
	- Setup (the Gear Icon)
	- Power Options (the Power Icon)

1. Back on your PC, click the Windows button to open the Windows 10 Start Menu. Then type "**WindowsIoTCoreWatcher**" then click on the "WindowsIoTCoreWatcher" icon to launch the utility:

	> **Note**: You may optionally want to right click on the app icon and select "**Pin to Start**" to make the app easier to find from your start menu in the future:

	![03030-PinToStart](images/03030-pintostart.png?raw=true "Pin to Start")

	![03040-OpenWindowsIoTCoreWatcher](images/03040-openwindowsiotcorewatcher.png?raw=true "Open Windows IoT Core Watcher")

1. The "**Windows IoT Core Watcher**" app should start and list the various Raspberry Pi devices on the network that are running Windows 10 IoT Core. 

	> **Note**: The "**Windows 10 IoT Core**" distribution we flashed to the Raspberry Pi's Micro SD card includes an executable called "**ebootpinger.exe**" that launches automatically.  The "**ebootpinger.exe**" program sends a UDP multicast packet every five seconds.  That packet includes details about the Pi including it's name, physical MAC address, IP Address, and OS version information.  The "**Windows IoT Core Watcher**" then listens for the multicast packets on it's network and displays the information it receives from the various Pis. 

	![03050-WindowsIoTCoreWatcher](images/03050-windowsiotcorewatcher.png?raw=true "Windows IoT Core Watcher")

1. Note that on a network with a number of Pi's with freshly flashed Micro SD cards, you can't differentiate them by their name.  Use the IP Address for your Pi you noted earlier to identify Your Pi in the list.  

	![03060-FindYourPiByIp](images/03060-findyourpibyip.png?raw=true "Find Your Pi by its IP Address")

1. You can then right-click on the line for your Raspberry Pi, and select "**Web Browser Here**" from the pop-up menu.  This will open up the "**[Windows Device Portal](https://ms-iot.github.io/content/en-US/win10/tools/DevicePortal.htm)**".  The "**Windows Device Portal**" is a built in web based management utility.  By default it listens on port "**8080**" and allows you to connect to your Pi via a browser to perform a wide range of configuration functions:

	![03070-WebBrowserHere](images/03070-webbrowserhere.png?raw=true "Web Browser Here")

1. When prompted, enter the default "Windows IoT Core" credentials:

	- User name: **Administrator**
	- Password:	**p@ssw0rd** (That's a zero, not the letter "o")

![03080-DefaultCredentials](images/03080-defaultcredentials.png?raw=true "Default Credentials")

1. When the "Windows Device Portal" web interface appears, take some time to click through the various pages on the left.  You should see that there is a fair amount you can do to manage your Raspberry Pi running Windows 10 IoT Core from the portal:

	![03090-PortalPageLinks](images/03090-portalpagelinks.png?raw=true "Portal Page Links")

1. Return to the "**Home**" page, and under the "**Change your device name**" header, enter a new name (instead of "**minwinpc**") for your Pi, then click the "**Save**" button. You need to use a valid windows computer name:

	- Less than 15 characters long 
	- No spaces
	- No special characters (@, /, \, etc.)
	- I recommend using your name or better yet your initials and the word pi, e.g. "**bretspi**", or "**bsspi**"

	![03100-RenamePi](images/03100-renamepi.png?raw=true "Rename Pi")

1. When prompted to reboot your Pi for the new name to take effect, click "**OK**"

	![03110-RebootPi](images/03110-rebootpi.png?raw=true "Reboot Pi")

1. Once your Pi has rebooted, you should see your new name both on the Pi's screen as well as in the "Windows IoT Core Watcher":

	![03120-NewNameOnPi](images/03120-newnameonpi.png?raw=true "New Name On Pi")

	![03130-NewNameInWatcher](images/03130-newnameinwatcher.png?raw=true "New Name in Windows IoT Core Watcher")

---

<a name="Task4"></a>
### Task 4: Setting the time on your Pi using SSH

Later, when we push data to Azure, we need the time on the Pi to be correct.  It ***SHOULD*** synchronize periodically with Windows Time Servers, but on some networks it may have a hard time doing so.  You can see if your Pi has the correct time by looking at the time displayed on the top of the Pi's monitor:

![04005-TimeOnPi](images/04005-timeonpi.png?raw=true "TimeOnPi")

If that time does not match the current time, we need to fix it.  Event if your time is correct, you should complete the steps in this task.  They will teach you how to connect to your Pi via SSH.  

For this task, we will use SSH to remotely connect to the command prompt on the Pi to set the time.  You could also use [Remote PowerShell](https://ms-iot.github.io/content/en-US/win10/samples/PowerShell.htm) for this, but for this lab [we will use SSH](http://ms-iot.github.io/content/en-US/win10/samples/SSH.htm).  This means that you will need an SSH client on your Windows computer (SSH isn't currently built-in in Windows).  For this lab, we recommend using [PuTTY](http://the.earth.li/~sgtatham/putty/latest/x86/putty-0.66-installer.exe) but any ssh client will do.  You just need to know how to use it if you use anything other than PuTTY. 

1. If necessary, [download and install the PuTTY SSH Client](http://the.earth.li/~sgtatham/putty/latest/x86/putty-0.66-installer.exe)   

1. Once installed, from the Windows "**Start Menu**", select "**All Apps**" | "**PuTTY**" | "**PuTTY**"

	![04010-RunPutty](images/04010-runputty.png?raw=true "Run PuTTY")

1. In the "**PuTTY Configuration**" window:

	- Connection Type: **SSH**
	- Host Name (or IP Address): **Your Raspberry Pi's IP Address**
	- Port: **22**
	- Click "**Open**"

	![04020-PuTTYConfiguration](images/04020-puttyconfiguration.png?raw=true "PuTTY Configuration")

1. You should receive a "**PuTTY Security Alert**" the first time you connect to your Pi over ssh.  Click "**Yes**" to add the Pi's SSH host key to your cache:

	![04030-PuTTYSecurityAlert](images/04030-puttysecurityalert.png?raw=true "PuTTY Security Alert")

1.  After a few seconds (it may take a while to connect) you will be prompted to login.  Use the default credentials (assuming you didn't change the password earlier) 

	- Login:	**Administrator**
	- Password: **p@ssw0rd** (That is a zero, not the letter "o")

	![04040-SSHLogin](images/04040-sshlogin.png?raw=true "SSH Login")

1. The date and time and time zone on the Pi must be correct for the security tokens used to publish to Azure later in the lab to be valid.  To check the current time zone setting on the Pi, type:

	`tzutil /g`

	![04050-TzutilG](images/04050-tzutilg.png?raw=true "TZUTIL G")

1. If the time zone reported is not correct, you can find a list of valid time zones using (you may need to increase the buffer size on your powershell window):

	`tzutil /l`

	![04060-TzUtilL](images/04060-tzutill.png?raw=true "TZUTIL L")

1. To set the time zone, locate the id of the time zone you want from the step above, then use:

	`tzutil /s "Your TimeZone Name"

	For example, for "Pacific Standard Time"

	`tzutil /s "Pacific Standard Time"

	![04070-TzUtilS](images/04070-tzutils.png?raw=true "TZUTIL S")

1. To check the date on the Raspberry Pi, type

	`date /T`

	![04080-GetDate](images/04080-getdate.png?raw=true "GetDate")

1. If the date is incorrect, you can enter a new date when prompted, or if the date is correct simply press `ENTER` to accept the current date:

	`date mm-dd-yy`

	For Example, if it was January 3rd, 2016:

	`date 01-03-16`

	![04090-SetDate](images/04090-setdate.png?raw=true "Set Date")

1. To check the Time on the pi, type:

	`time /T`

	![04100-GetTime](images/04100-gettime.png?raw=true "Get Time")

1. If the time is incorrect, you can enter a new time 

	`time hh:mm ampm`

	For example, if it is 12:15 pm:

	`time 12:15 pm`

	![04110-SetTime](images/04110-settime.png?raw=true "Set Time")

1. You can run most any standard Windows command prompt command from the SSH window.  You can check the online documentation for a list of [common command line utilities](http://ms-iot.github.io/content/en-US/win10/tools/CommandLineUtils.htm)
