# HOL 03 - Working with Analog Sensors

## Overview

This lab continues where the previous lab left off, but adds support for reading analog values in using an external Analog Digital Converter (ADC) chip.  There are various ADC chip options avaiable, but for the lab we'll assume you are using an MCP3008.

In this lab, the analog value you read will come from a Photoresistor.  Photoresistors are "Light Dependent Resistors".  Their resistance goes up, as the light goes down. The actual range of the resistance values depends on the type of the Photoresistor in use, but in general we can set them up in a voltage divider along with a 10K&#0937; resist and get a good range of values from them. 

The ADC chip will read the voltages out of the voltage divider circuit made up of the Photoresistor and the 10K&#0937; resistor and convert those into a number between 0 and some maximum.  The maxiumum number depends on the "resolution" of the ADC.  The MCP3008 assumed in this lab has a 10-bit resolution and produces a range of output values between 0 and 1023.  There are higher resolution ADCs.  For example, the MCP3208 has a 12-bit resolution and provides output values between 0 and 4096.  If you need more accuracy in your project, you may want to consider a higher resolution ADC. 

## Pre-Requisites

This lab assumes you have completed the previous labs:

- [01 - Installing Windows 10 IoT Core](../HOLs/01-InstallingWindows10IotCore/) 
- [02 - Blinking the LED with UWP](../HOLs/02-BlinkingTheLEDWithUWP/) 

In addition to the pre-requisites from those labs, this lab requires:

- An MCP3008 ADC Chip (<a href="http://ww1.microchip.com/downloads/en/DeviceDoc/21295d.pdf" target="_blank">Data Sheet</a>).  You may use an alternative chip such as the MCP3002, MCP3004, or MCP3208 but you will need to be aware of the differences, especially as to how you should connect them. 
- A Photoresistor (<a href="http://www.token.com.tw/pdf/resistor/cds-resistor-pgm.pdf" target="_blank">Something similar to this</a>)
- a 10K&#0937; Resistor
- Multiple male/female and male/male jumpers.

## Setup

Ensure that you have completed the pre-requisites above.

## Tasks

- [Task 1: Complete the ADC and Photoresistor Circuit](#Task1)
- [Task 2: Open the updated UWP App and Review It](#Task2)
- [Task 3: Run the UWP App on the Raspberry Pi 2 ](#Task3)

---

<a name="Task1"></a>
### Task 1: Complete the ADC and Photoresistor Circuit

<span style="color: red;">**KEEP THE CIRCUIT FROM THE PREVIOUS LAB INTACT.  THIS LAB BUILDS ON THAT CIRCUIT**</span>

1. <span style="color: red;">***REMOVE THE POWER AND HDMI CABLES FROM YOUR RASPBERRY PI 2***</span>. This will ensure that you do not accidentally short-out anything on the board while you are completing the circuit. 1. As a reference, you can identify the pins on the Raspberry Pi 2 using the following image.

	![01010-rpi2pinouts](images/01010-rpi2pinouts.png?raw=true "Raspberry Pi 2 Pinouts") 

1. In addition, here is the pinout for the MCP3008 (purposely rotated to match the orientation in the circuit diagram below):

	![01015-mcp3008inverted](images/01015-mcp3008inverted.png?raw=true "MCP3008 Inverted")

1. Use the following diagram to complete the circuit. <span style="color: red;">***PAY CLOSE ATTENTION TO THE PINS, AND WHERE YOUR JUMPER CABLES GO.  THE ACTUAL COLOR OF THE JUMPER WIRE USED DOES NOT MATTER***</span>:

	![01020-analogcircuit](images/01020-analogcircuit.png?raw=true "Analog Circuit")

1. Double check your work, and when you are certain the circuit looks correct, re-connect the HDMI cable, then the Power cable to your Raspberry Pi 2 to boot it up. 

---

<a name="Task2"></a>
### Task 2: Open the updated UWP App and Review It

 

1. If you still have Visual Studio open from the previous lab, close the solution that is open it in.  We will open a fresh copy of the **WindowsOnPi** project for this lab.

1. In the folder where you have exracted the files for these Hands-On-Labs, locate the `HOLs\02-BlinkingTheLEDWithUWP\code\WindowsOnPi` folder, and double click on the `WindowsOnPi.sln` solution file to open it in Visual Studio.

	![02010-OpenWindowsOnPi](images/02010-openwindowsonpi.png?raw=true "Open WindowsOnPi solution")

1. This solution has all the same code from the prevous lab (so the buttons and LED still work), but adds some additional UI and code. 

1. In the Visual Studio Solution Explorer, double click on MainPage.xaml to open it in the designer.  In the XAML markup, notice there are some new elements:

	- There are two new `<TextBlock>` elements.  One (`LightSensorValueLabel`) just shows the static text "LIGHT:".  The other, `LightSensorValueText`, will be used to display the text value read from the Photoresistor via the ADC.

	- The `<CheckBox x:name="ToggleLedWhenDark">` will be used to enable the LED to be turned on if the Photoresistor's value is over half of it's resolution (means it's dark out).  

	````XML
	<!-- ============================================ -->
	<!-- BEGIN - XAML ADDED FOR THE ANALOG SENSOR LAB -->
	<!-- ============================================ -->

	<TextBlock
	  x:Name="LightSensorValueLabel"
	  Text="LIGHT:"
	  Grid.Row="3" Grid.Column="0"
	  FontSize="96" FontWeight="Bold"
	  HorizontalAlignment="Center"
	  VerticalAlignment="Center"
	  Margin="10"/>

	<TextBlock
	  x:Name="LightSensorValueText"
	  Text="0"
	  Grid.Row="3" Grid.Column="1"
	  FontSize="96" FontWeight="Bold"
	  HorizontalAlignment="Center"
	  VerticalAlignment="Center"
	  Margin="10"/>

	<CheckBox
	  x:Name="ToggleLedWhenDark"
	  Content="Toggle LED When It Is Dark"
	  IsChecked="True"
	  Grid.Row="4" Grid.Column="0" Grid.ColumnSpan="2"
	  FontSize="24" FontWeight="Bold"
	  HorizontalAlignment="Center"
	  VerticalAlignment="Center"
	  Margin="10" />

	<!-- ============================================ -->
	<!-- END - XAML ADDED FOR THE ANALOG SENSOR LAB   -->
	<!-- ============================================ -->
	````
1. If you open MainPage.xaml.cs, you will see a new `ADC/SPI Members` region.  

1. If you expand the `ADC/SPI Members` region, at the top, you will see a number of enums, constants, and field declarations.

1. First, there are a number of members that have to do with the type of ADC chip being used.  This code could have been simplified if we just assumed an MCP3008, but support for the MCP3002 and MCP3208 were also added because they are very common ADC chips, and it was likely that multiple users of this lab would have them. The most important line of code is the last one, Make sure that you set the `ADC_CHIP` variable to the type of ADC you will be using, again MCP3008 is assumed here. 

	````C#
	/// <summary>
	/// An enumeration of the various types of ADC Chips
	/// </summary>
	enum AdcChip { NONE, MCP3002, MCP3208, MCP3008 };

	// Different bytes need to be sent over SPI depending on the type 
	// of ADC chip you have:

	/// <summary>
	/// The configuration bytes that need to be sent to MCP3002 ADCs
	/// </summary>
	private const byte MCP3002_CONFIG = 0x68; /* 01101000 channel configuration data for the MCP3002 */

	/// <summary>
	/// The configuration bytes that need to be sent to MCP3208 ADCs
	/// </summary>
	private const byte MCP3208_CONFIG = 0x06; /* 00000110 channel configuration data for the MCP3208 */

	/// <summary>
	/// The configuration bytes that need to be sent to MCP3008 ADCs
	/// </summary>
	private readonly byte[] MCP3008_CONFIG = { 0x01, 0x80 }; /* 00000001 10000000 channel configuration data for the MCP3008 */

	/// <summary>
	/// Type type of Analog Digital Converter (ADC) chip being used
	/// </summary>
	private AdcChip ADC_CHIP = AdcChip.MCP3008;
	````

1. The next chunk of members has to do with the "Serial Peripheral Interface" (SPI) configuration. The Raspberry Pi 2 will communicate with the ADC using SPI.  We need to tell it which SPI controller on the Pi to use (SPI0 or SPI1) and on that controller which chip select line to use (0 or 1). Finally, we declare a SpiDevice instance which will be used to actually conduct the SPI communications with the ADC:

	````C#
	/// <summary>
	/// The friendly name of the Raspberry Pi SPI Controller being used: "SPI0" or "SPI1"
	/// </summary>
	private const string SPI_CONTROLLER_NAME = "SPI0";

	/// <summary>
	/// The SPI Chip Select Line being used on the current controller
	/// </summary>
	private const Int32 SPI_CHIP_SELECT_LINE = 0;

	/// <summary>
	/// Manages the SPI communications with the Analog Digital Converter (ADC)
	/// </summary>
	private SpiDevice SpiADC;
	````

1. Next there are a few variables that deal with WHEN the Photoresistor values will be read from the ADC, and WHERE they will be stored.  The `adcTimer` `DispatcherItmer` instance will be used to read values from the ADC at the interval specified by the `adcTimerPeriod`.  When the value is read, it will be stored in the `adcLightSensorValue`

	````C#
	/// <summary>
	/// The timer that is used to periodically read values from the ADC
	/// </summary>
	private DispatcherTimer adcTimer;

	/// <summary>
	/// The time interval (in milliseconds) for the adcTimer
	/// </summary>
	private int adcTimerPeriod = 100;

	/// <summary>
	/// The integer value last read from the ADC
	/// </summary>
	private int adcLightSensorValue;
	````
1. Next is the `InitAdc()` method.  The `InitAll()` method we introduced in the previous lab has been updated to include a call to `InitAdc()`.  The method uses the fields values from above to initialize the SPI communications channel, and start the `adcTimer` `DispatcherTimer`.

	````C#
	/// <summary>
	/// Initializes the SPI communications with the specified Analog Digital Controller (ADC)
	/// </summary>
	/// <returns>An awaitable task that completes when the initialization succeeds or fails</returns>
	private async Task InitAdc()
	{
	  try
	  {
		 // Check the ADC_CHIP variable and make sure it has been set to something other
		 // than None.  This is just a way to ensure that you have intentionally set the 
		 // ADC chip type
		 if (ADC_CHIP == AdcChip.NONE)
		 {
			throw new Exception("Please change the ADC_DEVICE variable to either MCP3002 or MCP3208, or MCP3008");
		 }

		 // First get the "Advanced Query String" (AQS) device selector
		 // for the SPI Controller with the SPI_CONTROLLER_NAME
		 string spiAqs = SpiDevice.GetDeviceSelector(SPI_CONTROLLER_NAME);

		 // Then, get the DeviceInformation on that controller.
		 // This will give us the unqie ID we need to initialize the Spi device
		 var deviceInfo = await DeviceInformation.FindAllAsync(spiAqs);

		 // Create some settings to be used when communicating with the Spi device
		 var settings = new SpiConnectionSettings(SPI_CHIP_SELECT_LINE);
		 settings.ClockFrequency = 500000;   /* 0.5MHz clock rate                                        */
		 settings.Mode = SpiMode.Mode0;      /* The ADC expects idle-low clock polarity so we use Mode0  */

		 //Finally, initialize the Spi device with the give DeviceInfo Id and settings from above. 
		 SpiADC = await SpiDevice.FromIdAsync(deviceInfo[0].Id, settings);

		 /* Now that everything is initialized, create a timer so we read analog data periodically */
		 adcTimer = new DispatcherTimer();
		 adcTimer.Tick += AdcTimer_Tick;
		 adcTimer.Interval = TimeSpan.FromMilliseconds(adcTimerPeriod);
		 adcTimer.Start();
	  }

	  /* If initialization fails, display the exception and stop running */
	  catch (Exception ex)
	  {
		 string message = string.Format("SPI Initialization Failed: {0} - {1}", ex.GetType().Name, ex.Message);
		 throw new Exception(message);
	  }
	}
	````

1. At the end, there is a chain of methods that are called when the `adcTimer` "ticks" and indicates it's time to read the current light sensor (photoresistor) value from the ADC.
	
	- `AdcTimer_Tick()` is the method called by the `adcTimer` `DispatcherTimer` whenever it "ticks".  The method calles the `ReadLightSensor()` method to get the latest light sensor (photoresistor) value, and if the `<CheckBox x:Name="ToggleLedWhenDard"`> checkbox is checked, calls the `LightLED()` method to turn the LED on if it is dark (the `adcLightSensorValue` is over half of the ADC chip's resolution).


	- `ReadLightSensor()` Uses the SPI channel to send a property formatted request to the ADC based on the ADC type, and then receives the data back from it.  It then calls the `convertToInt()` method to convert the sensor data received into an int. 


	- The `convertToInt()` method coverts the byte data received from the ADC in the `ReadLightSensor()` method.  The means of converting the byte data to an int varies by ADC type.  

	- Finally the `LightLED()` method compares the adcLightSensorValue to the maximum value of the given ADCs resolution. If the adcLightSensor value is over half of the ADC resolution, it toggles the `<ToggleButton x:name="TogglePinButton">` state which in turn updates the UI and sets the LED.   

		````C#
		/// <summary>
		/// Called by the adcTimer.  Reads the light sensor value and 
		/// toggles the button control and LED based on the value.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void AdcTimer_Tick(object sender, object e)
		{
		  //Read the latest value from the light sensor
		  ReadLightSensor();
				
		  //Toggle the button control and LED based on the Light Sensor Value.
		  //If the ToggleLedWhenDark checkbox is turned on. 
		  if (ToggleLedWhenDark.IsChecked == true)
		  {
			 LightLED();
		  }
		}

		/// <summary>
		/// Reads data from the light sensor via the ADC
		/// </summary>
		public void ReadLightSensor()
		{
		  byte[] readBuffer = new byte[3]; /* Buffer to hold read data*/
		  byte[] writeBuffer = new byte[3] { 0x00, 0x00, 0x00 };

		  /* Setup the appropriate ADC configuration byte */
		  switch (ADC_CHIP)
		  {
			 case AdcChip.MCP3002:
				writeBuffer[0] = MCP3002_CONFIG;
				break;
			 case AdcChip.MCP3208:
				writeBuffer[0] = MCP3208_CONFIG;
				break;
			 case AdcChip.MCP3008:
				writeBuffer[0] = MCP3008_CONFIG[0];
				writeBuffer[1] = MCP3008_CONFIG[1];
				break;
		  }

		  //Read data from the ADC
		  SpiADC.TransferFullDuplex(writeBuffer, readBuffer);

		  //Convert the returned bytes into an integer value
		  adcLightSensorValue = convertToInt(readBuffer);

		  // UI updates must be invoked on the UI thread 
		  var task = this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
		  {
			 //Display the light sensor value in the UI via the LightSensorValueText control
			 LightSensorValueText.Text = adcLightSensorValue.ToString();
		  });

		}

		/// <summary>
		/// Used to convert raw ADC bytes for the current ADC_CHIP type to an int.
		/// Refer to your ADCs data sheet for more information
		/// </summary>
		/// <param name="data">The raw ADC data to convert</param>
		/// <returns>The converted bytes as an int</returns>
		public int convertToInt(byte[] data)
		{
		  int result = 0;
		  switch (ADC_CHIP)
		  {
			 case AdcChip.MCP3002:
				result = data[0] & 0x03;
				result <<= 8;
				result += data[1];
				break;
			 case AdcChip.MCP3208:
				result = data[1] & 0x0F;
				result <<= 8;
				result += data[2];
				break;
			 case AdcChip.MCP3008:
				result = data[1] & 0x03;
				result <<= 8;
				result += data[2];
				break;
		  }
		  return result;
		}

		/// <summary>
		/// Updates the UI and LED state based on the current adcLightSensorValue 
		/// </summary>
		private void LightLED()
		{
		  int adcResolution = 0;

		  // The resolution of the ADC depends on it's type. 
		  // This basically specifies the maximum range of values
		  // read from the chip.  0 < value < adcResolution
		  switch (ADC_CHIP)
		  {
			 case AdcChip.MCP3002:
				adcResolution = 1024;
				break;
			 case AdcChip.MCP3208:
				adcResolution = 4096;
				break;
			 case AdcChip.MCP3008:
				adcResolution = 1024;
				break;
		  }

		  // Turn on LED if the analog value is over half of it's range
		  if (adcLightSensorValue > (adcResolution / 2))
		  {
			 TogglePinButton.IsChecked = true;
		  }
		  // Otherwise turn it off
		  else
		  {
			 TogglePinButton.IsChecked = false;
		  }
		}
		````

---

<a name="Task3"></a>
### Task 3: Run the UWP App on the Raspberry Pi 2

OK, time to give it a run on the Pi

1. In Visual Studio, ensure that the target platform is set to ARM, and the target debug device is set to "Remote Machine" and configured to deploy to your Pi.  ([You can refer to Task 3 in the previous lab for a more detailed reminder](HOLs/02-BlinkingTheLEDWithUWP#Task3))

	![03010-DebugTargets](images/03010-debugtargets.png?raw=true "Debug Targets")

	![03020-RemoteConnections](images/03020-remoteconnections.png?raw=true "Remote Connections")


1. Press the Debug button (or press _F5_) to start the debug session on the Pi.

1. On the Raspberry Pi 2's monitor, you should see the app running.  Notice that when the Photoresistor's value is LESS than half of the ADC chip's max value (1023 in the case of the MCP3008), the LED is off. 

	![03040-LEDOffInBrightLight](images/03040-ledoffinbrightlight.png?raw=true "LED Off In Bright Light")

	![03030-PhotoResistorValueLow](images/03030-photoresistorvaluelow.png?raw=true "Photoresistor Value Low")

1. However, if you cover the Photoresistor with your finger so it receives less light, it should react by reporting higher values via the ADC.  If the value is greather than half of the ADCs max value (1023, or a 512 threshold), the LED should turn on as long ast the `<CheckBox x:Name="ToggleLedWhenDark">` checkbox is checked.  

	![03040-LEDOnWhenDark](images/03040-ledonwhendark.png?raw=true "LED ON When Dark")

	![03060-ToggleButtonOnWhenDark](images/03060-togglebuttononwhendark.png?raw=true "ToggleButtonOnWhenDark")

1. Lastly, if you turn OFF the "**Toggle LED WHen It Is Dark**" checkbox, the photoresistor can still report dard values, but not toggle the led:

	![03070-DarkButNoLight](images/03070-darkbutnolight.png?raw=true "Dark but No Light")

	![03080-DarkButNotToggle](images/03080-darkbutnottoggle.png?raw=true "Dark but no Toggle")

1. When you are done testing, stop debugging in Visual Studio to close the app. 

1. Close Visual Studio when you are done.  We will open a fresh copy of the **WindowsOnPi** solution in the next lab. 

