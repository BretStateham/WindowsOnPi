# HOL 04 - Intro to Azure IoT Hubs

## Overview

In this last lab, we will use take a look at <a href="https://azure.microsoft.com/en-us/services/iot-hub/" target="_blank">Azure IoT Hubs</a>.  Azure IoT Hubs provide a powerful mechanism for managing secure device communications with the Cloud.  

For this quick example, we will take the Photoresistor sensor data we collected in the previous lab, and publish it to an Azure IoT Hub.  Then we'll see how we can receive data back from the Azure IoT Hub to the device.

## Pre-Requisites

This lab assumes you have completed the previous labs:

- [01 - Installing Windows 10 IoT Core](../HOLs/01-InstallingWindows10IotCore/) 
- [02 - Blinking the LED with UWP](../HOLs/02-BlinkingTheLEDWithUWP/) 
- [03 - BWorking with Analog Sensors](../HOLs/03-WorkingWithAnalogSensors/) 

In addition to the pre-requisites from those labs, this lab requires:

- An active Azure Subscription.  A <a href="https://azure.microsoft.com/en-us/free/" target="_blank">Free Trial</a> is fine.

## Setup

Ensure that you have completed the pre-requisites above.

## Tasks

- [Task 1: Create an Azure IoT Hub](#Task1)
- [Task 2: Use Device Explorer to Register Your Device](#Task2)
- [Task 3: Open the updated UWP App and Review It](#Task3)
- [Task 4: Run the UWP App on the Raspberry Pi 2](#Task3)

---

<a name="Task1"></a>
### Task 1: Create an Azure IoT Hub

Assuming you already have an active Azure Subscription, you can create an Azure IoT Hub in just a few minutes.  Once created, your Azure IoT Hub can receive thousands of messages a day from one or more devices. 

In this lab, we'll only use a single device, but you could have thousands or millions depending on how you scaled your Azure IoT Hub Services.  

1. In your web browser, navigate to <a href="https://portal.azure.com" target="_blank">https://portal.azure.com</a> and login with the credentials associated with your account.  

1. Once logged into the portal, select "**+ New**" | "**Internet of Things**" | "**Azure IoT Hub**", then in the "**IoT hub**" blade complete the fields for your new IoT Hub:

	- Name: big a unique but short name perhaps your initials followed by "iothub", e.g. "bssiothub".
	- Pricing: Select the "F1 - Free" tier unless you intend to use this heavily during or after this lab.
	- IoT Hub units: 1 (can't change on the Free tier)
	- Device-to-cloud partitions: 2 (chan't change on the Free tier)
	- Create a new resource group: Choose a name.  I recommend something similar to your iothub  name, for example bssresourcegroup
	- Subscription: Choose the desired subscription if prompted
	- Location: East US
	- Pin to dashboard: Checked
	- Click "**Create**"

	![01010-CreateNewIoTHubInPortal](images/01010-createnewiothubinportal.png?raw=true "Create New Azure IoT Hub in the Portal")


1. It should only take a few minutes for your IoT Hub to be provisioned. When it is complete, the configuration blades for it should open in the portal:

	![01020-IoTHubBlades](images/01020-iothubblades.png?raw=true "IoT Hub Blades")

1. If the "**Settings**" blade isn't open, click the "**Settings"** (gear icon) at the top of the Iot Hub blade.  Then, in the "**Settings*"" blade, click the "**Shared access policies**" link:

	![01030-SharedAccessPolicies](images/01030-sharedaccesspolicies.png?raw=true "Shared Access Policies")

1. Then click the link for the "**iothubowner**" Shared Access Policy. If you connect ot the IoT Hub using the key associated with this policy, your connection will have full permissions. For a production scenario you might want to use a more restrictive policy, but for the lab, this will suffice: 

	![01040-IoTHubOwnerPolicy](images/01040-iothubownerpolicy.png?raw=true "IoTHubOwner Policy")

1. Finally, click the Icon next to the "**Connection string-primary key**" value to copy it to the clipboard.  We will use this value in a later step.  You may want to keep this browser window open in case you need to re-copy the connection string, or paste the string int a text editor temporarily:

	![01050-CopyConnectionString](images/01050-copyconnectionstring.png?raw=true "Copy Connection String")

---

<a name="Task2"></a>
### Task 2: Use Device Explorer to Register Your Device

A single IoT Hub can broker messages for thousands of devices.  In this lab however, we will use just one.  Before a device can send and receive messages with the IoT Hub, it must first be "registered".  There are APIs you can use to programmatically register devices.  This could also be something that happens as part of a production process.  However, for this lab, we will manually register our device using a sample app called <a href="https://github.com/Azure/azure-iot-sdks/blob/master/tools/DeviceExplorer/doc/how_to_use_device_explorer.md" target="_blank">"Device Explorer"</a>

1. Download the Device Explorer from <a href="https://github.com/Azure/azure-iot-sdks/releases" target="_blank">here</a> (grab the latest SetupDeviceExplorer.msi), then run the installer.

1. Once installed, the executable for Device Explorer should be here `C:\Program Files (x86)\Microsoft\DeviceExplorer\DeviceExplorer.exe`.  You may want to pin that to your start menu to make it easy to find in the future.  Regardless, run the utility.  

1. Once the app loads, on the "**Configuration**" tab, paste the connection string you copied for your IoT Hub's "**iothubowner**" shared access policy into the "**IoT Hub Connection String*"" box, then click the "**Update**" button, and when prompted click "**OK"** to confirm the successful update:

	![02010-ConnectDeviceExplorer](images/02010-connectdeviceexplorer.png?raw=true "Connect Device Explorer")

1. Next, switch to the "**Management**" tab and click the "**Create**" button.  In the "**Create Device**" window:

	- Device ID: Give your device an ID.  Generally you would want a GUID here, but for the lab just enter then name of your Pi, e.g. "**bretspi**"
	- Leave the "**Primary Key**" and "**Secondary Key**" keys alone
	- Auto Generate ID: **UNCHECKED** because we are giving our Pi our own ID.  If you wanted your device's ID to be a GUID, just turn on this checkbox. 
	- Auto Generate Keys: **CHECKED** (we don't want to bother with coming up with key values)
	- Click "**Create**" when done. 
	- Finally click "**Done**" in the confirmation window

	![02020-CreateDevice](images/02020-createdevice.png?raw=true "Create Device")

1. Finally, right click on your newly created device and select "**Copy connection string for selected device**".  This will be used by our app running on the Pi to connect security to the IoT Hub as that specific device.  Keep the Device Explorer running because we'll need it later, and you may need to re-copy the connection string.

	![02030-CopyDeviceConnectionString](images/02030-copydeviceconnectionstring.png?raw=true "Copy Device Connection String")

---

<a name="Task3"></a>
### Task 3: Open the updated UWP App and Review It

Again, we've added some code to the WindowsOnPi UWP app to work with Azure IoT Hubs:

1. If you still have Visual Studio open from the previous lab, close the solution that is open it in.  We will open a fresh copy of the **WindowsOnPi** project for this lab.

1. In the folder where you have exracted the files for these Hands-On-Labs, locate the `HOLs\04-IntroToAzureIoTHubs\code\WindowsOnPi` folder, and double click on the `WindowsOnPi.sln` solution file to open it in Visual Studio.

	![03010-OpenSolution](images/03010-opensolution.png?raw=true "Open Solution")

1. This solution has all the same code from the prevous lab (so the buttons and LED still work), but adds some additional UI and code. 

1. In the Visual Studio "**Solution Explorer**" window, you can see a few notable additions:
	
	- The "**Microsoft.Azure.Devices.Client**" preview NuGet package has been added.  This facilitates the communication with the Azure IoT Hub
	- The "**IotHubSensor.cs**" class file is a helper class created for the lab that streamlines the formatting of the sensor data messages that will be sent to the Azure IoT Hub.
	- The "**IotHubHelper.cs"** class file is a helper class created for the lab that wraps up the IoT Hub operations including intialization as well as sending and receving messages. 

	![03020-SolutionExplorerChanges](images/03020-solutionexplorerchanges.png?raw=true "Solution Explorer Changes")

1. Open the 

1. Step 1
1. Step 2
1. Step 3


---

<a name="Task4"></a>
### Task 4: Run the UWP App on the Raspberry Pi 2

Finally, we'll do this

1. Step 1
1. Step 2
1. Step 3


