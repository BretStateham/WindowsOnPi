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
- [Task 3: Finally Do This](#Task3)

---

<a name="Task1" />
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

<a name="Task2" />
### Task 2: Flash the SD Card with Windows 10 IoT Core

In this task, we'll use the utilities and image we installed in the previous task to flash the Micro SD card with Windows 10 IoT Core.  

1. Step 1
1. Step 2
1. Step 3

---

<a name="Task3" />
### Task 3: Finally Do This

Finally, we'll do this

1. Step 1
1. Step 2
1. Step 3



