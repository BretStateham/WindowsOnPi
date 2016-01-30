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

1. In the "**Solution Explorer**" window, double click on the "**MainPage.xaml.cs**" code behind file open it in the editor.  Then expand the `IoT Hub Members` region, to see the code that was added for this lab. 

1. First, there are some members:

	- The `iotHubHelper` is an instance of the IoTHubHelper class.
	- The `iotHubSendTimer` `DispatcherTimer` and it's `iotHubSendTimerPeriod` control the frequency that sensor data messages are sent to Azure
	- The `iotHubReceiveTimer` `DispatcherTimer` and it's `iotHubReceiveTimerPeriod` control the receiving of messages from Azure

	````C#
	/// <summary>
	/// The IoTHubHelper instance used to simplify Azure IoT Hub communications
	/// </summary>
	IoTHubHelper iotHubHelper;

	/// <summary>
	/// The timer used to manage how often data is sent to the Azure IoT Hub
	/// </summary>
	private DispatcherTimer iotHubSendTimer;

	/// <summary>
	/// The time interval (in milliseconds) for the iotHubSendTimer
	/// </summary>
	private int iotHubSendTimerPeriod = 10000;

	/// <summary>
	/// The timer used to manage how often data is received from the Azure IoT Hub
	/// </summary>
	private DispatcherTimer iotHubReceiveTimer;

	/// <summary>
	/// The time interval (in milliseconds) for the iotHubReceiveTimer
	/// </summary>
	private int iotHubReceiveTimerPeriod = 10000;
	````

1. The `InitIoTHub()` initializes light sensor message formatter, the devices connection to the IoTHub, and it starts the send and receive timers.  A call to IotHub was added to the `InitAll()` method. 

	````C#
	/// <summary>
	/// Initializes the IoT Hub sensors and communications
	/// </summary>
	private void InitIotHub()
	{
	  // We'll just have once sensor, our photo resistor, but you could add more
	  // Since there could be multiple, a collection (LIST) of sensors is required.
	  // 
	  // Each sensor in the collection should have a unique ID.  
	  // We'll use a GUID for that. 
	  // You can easily generate a new GUID in Visual Studio if from the menu bar
	  // you select "Tools" | "Create GUID", then select the registry format, 
	  // generate a GUID, then copy and paste it for the desired sensor 
	  // then remove the curly braces. Whew.
	  List<IoTHubSensor> sensors = new List<IoTHubSensor>()
	  {
		 new IoTHubSensor("0C8A350B-D0B1-44A8-BDD4-245135BAF2F5", "Photoresistor", "Light", "Lumens"),
	  };

	iotHubHelper = new IoTHubHelper(
	  iotDeviceConnectionString: "PASTE_YOUR_IOT_HUB_DEVICE_CONNECTION_STRING_HERE",
	  organization: "ENTER_A_NAME_FOR_YOUR_ORGANIZATION_HERE",
	  location: "ENTER_A_NAME_FOR_YOUR_DEVICES_LOCATION_HERE",
	  sensorList: sensors);

	  // Initialize the iotHubSendTimer
	  iotHubSendTimer = new DispatcherTimer();
	  iotHubSendTimer.Tick += IotHubSendTimer_Tick;
	  iotHubSendTimer.Interval = TimeSpan.FromMilliseconds(iotHubSendTimerPeriod);
	  iotHubSendTimer.Start();

	  // Initialize the iotHubReceiveTimer
	  iotHubReceiveTimer = new DispatcherTimer();
	  iotHubReceiveTimer.Tick += IotHubReceiveTimer_Tick;
	  iotHubReceiveTimer.Interval = TimeSpan.FromMilliseconds(iotHubReceiveTimerPeriod);
	  iotHubReceiveTimer.Start();
	}
	````

1. The `IoTHubSendTimer_Tick` method is invoked by the `iotHubSendTimer`.  It's job is to retrieve the current adcLightSensorValue, and assign in to the IoTHubSensor helper that is used to help format the messages sent to IoTHub. It then asks the `iotHubHelper` to send the sensor's data as a message to the IoT Hub. 

	````C#
	/// <summary>
	/// Called by the iotHubSendTimer.  
	/// Used to periodically send sensor data to an Azure IoT Hub
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	private void IotHubSendTimer_Tick(object sender, object e)
	{
	  // As long as we are connected to the ADC over SPI
	  // publish the data to the IoT Hub.
	  if (SpiADC != null)
	  {
		 // Find the IotHubSensor instance used to publish data for our "Light" sensor..
		 IoTHubSensor lightSensor = iotHubHelper.sensors.Find(s => s.measurename == "Light");
		 if (lightSensor != null)
		 {
			lightSensor.value = adcLightSensorValue;
			iotHubHelper.SendSensorData(lightSensor);
		 }
	  }
	}
	````

1. The `IotHubReceiveTimer_Tick` method is invoked by the `iotHubReceiveTimer` timer. It uses the `iotHubHelper` instance to retrieve the next message (if any) from the IoT Hub.  It then parses the message data looking for an "ON" command.  If "ON" is found, it sets the `TogglePinButton.IsChecked` property to true which should turn on the LED. Otherwise it sets the property to false which should turn off the LED.  For this to truly work though, the light sensor shouldn't be turning on the LED when it is dark.  You will want to turn off the `<CheckBox x:Name="ToggleLedWhenDark">` at runtime to make this work. 

	````C#
	/// <summary>
	/// Called by the iotHubReceiveTimer.
	/// Used to periodically receive data from an Azure IoT Hub
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	private async void IotHubReceiveTimer_Tick(object sender, object e)
	{
	  string message = await iotHubHelper.ReceiveMessage();

	  if (message != string.Empty)
	  {
		 string debugMsg = string.Format("Command Received: {0}", message);
		 Debug.WriteLine(debugMsg);
			  
		 switch (message.ToUpperInvariant())
		 {
			case "ON":
			  //Simulate the toggle button being Checked
			  TogglePinButton.IsChecked = true;
			  break;
			default:
			  //Simulate the toggle button being UnChecked
			  TogglePinButton.IsChecked = false;
			  break;
		 }

	  }
	}
	````

1. Before you can run the app, you need to make some changes in the `InitIotHub()` method locate the following block of code: 

	````C#
	iotHubHelper = new IoTHubHelper(
	  iotDeviceConnectionString: "PASTE_YOUR_IOT_HUB_DEVICE_CONNECTION_STRING_HERE",
	  organization: "ENTER_A_NAME_FOR_YOUR_ORGANIZATION_HERE",
	  location: "ENTER_A_NAME_FOR_YOUR_DEVICES_LOCATION_HERE",
	  sensorList: sensors);
	````

1. In the block above, replace `"PASTE_YOUR_IOT_HUB_DEVICE_CONNECTION_STRING_HERE"` with the connection string you copied from Device Explorer in the last task, and replace `"ENTER_A_NAME_FOR_YOUR_ORGANIZATION_HERE"` and `"ENTER_A_NAME_FOR_YOUR_DEVICES_LOCATION_HERE"` with appropriate values.  For example:

	![03030-CopyDeviceConnectionString](images/03030-copydeviceconnectionstring.png?raw=true "Copy Device Connection String Reminder")

	````C#
	iotHubHelper = new IoTHubHelper(
	  iotDeviceConnectionString: "HostName=bssiothub.azure-devices.net;DeviceId=bretspi;SharedAccessKey=...=",
	  organization: "SDSU",
	  location: "E207",
      sensorList: sensors);
	````
---

<a name="Task4"></a>
### Task 4: Run the UWP App on the Raspberry Pi 2

Let's give it a try.

1. In Visual Studio, ensure that the target platform is set to ARM, and the target debug device is set to "Remote Machine" and configured to deploy to your Pi.  ([You can refer to Task 3 in the Blinking the LED lab for a more detailed reminder](HOLs/02-BlinkingTheLEDWithUWP#Task3))

	![04010-debugtargets](images/04010-debugtargets.png?raw=true "Debug Targets")

	![04020-remoteconnections](images/04020-remoteconnections.png?raw=true "Remote Connections")

1. Press the Debug button (or press _F5_) to start the debug session on the Pi.

1. Once the app is running on the Pi, return to the "Device Explorer" app on your computer.  Switch to the "Manage" tab:

	- Confirm the "**Event Hub**" name
	- Confirm the "**Device ID"** (make sure it matches the ID you created for your device previously)
	- Click the "**Monitor**" button

	![04030-MonitorDevice](images/04030-monitordevice.png?raw=true "MonitorDevice")

1. Once every 10 seconds (10000 milliseconds), the app should seen the current adcLightSensorValue to the IoT Hub.  If you want to change the frequency, modify the `iotHubSendTimerPeriod` value:

	![04040-MessagesSending](images/04040-messagessending.png?raw=true "Messages Sending")

1. You can also send messages from the IoT Hub back to the device.  We'll send messages to control the LED remotely, but before we do we need to stop automatically setting the LED based on the current light sensor value.  While the app is still running on the Pi, **UNCHECK** the "**Toggle LED When It Is Dark**" checkbox:

	![04050-TurnOffCheckbox](images/04050-turnoffcheckbox.png?raw=true "Turn Off Checkbox")

1. Next, on your computer, return to the "Device Explorer" app, and switch to the "**Messages To Device**" tab. Again, ensure the right **IoT Hub** and **Device ID** are selected.  

	- Type **ON** in the Message box.
	- **UNCHECK** the "Add Time Stamp" checkbox
	- **CHECK** the "Monitor Feedback Endpoint" checkbox
	- Click the "Send" button. 

	![04060-SendMessage](images/04060-sendmessage.png?raw=true "Send Message")

1. Within 10 seconds (10000 milliseconds) the Pi should receive and parse the message and turn "ON" the LED.  

	![04070-ToggledOn](images/04070-toggledon.png?raw=true "Toggled On")

1. You can send an message other than "ON" to turn the LED back off. Here, we'll send "OFF":

	> **Note**: With the "**Monitor Feedback Endpoint** checkbox on, it looks for confirmation messages coming back from the Pi that the message was read. 

	![04080-SendOffMessage](images/04080-sendoffmessage.png?raw=true "Send OFF Message")

	![04090-ToggledOff](images/04090-toggledoff.png?raw=true "Toggled Off")

1. You can stop the debugger and close Visual Studio when you are done.