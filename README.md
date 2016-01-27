# Windows On Pi Workshop

## Overview

The Windows On Pi Workshop is a hands-on workshope geared towards helping the reader become familiar with building **Universal Windows Platform** (UWP) applications that run on the **Windows 10 IoT Core** operating system on the **Raspberry Pi 2** development board, and that publish data to the internet using **Microsoft Azure IoT Hubs**, and **Web Apps**.

At the completion of this workshop, the reader should be able to:

- Deploy Windows 10 IoT Core on the Raspberry Pi 2
- Understand various methods of connecting the Raspberry Pi to the network
- Connect to the Raspberry Pi using the Windows Device Portal, PowerShell or SSH
- Configure Windows 10 IoT Core on the Raspberry Pi 2
- Develop Universal Windows Platform (UWP) applications using C# and XAML and deploy them to the Raspberry Pi
- Create a simple prototype circuit using the Raspberry Pi development board and additional components
- Publish data from the Raspberry Pi to an Azure IoT Hub
- Consume the Azure IoT Hub data from an Azure Web Application

## Computer Requirements

To complete the steps in this lab, you will need 

- **A computer running Windows 10**.  You can't develop Windows 10 Apps for the Pi without Windows 10 on your host computer.  You can [upgrade your existing copy of Windows 7 or Windows 8.1 to Windows 10 for Free](https://www.microsoft.com/en-us/windows/windows-10-upgrade).  Or if you don't have Windows you can [download a 90 Day Evaluation Copy of Windows 10 Enterprise](https://www.microsoft.com/en-us/evalcenter/evaluate-windows-10-enterprise) 
- **Visual Studio 2015 with Update 1** (the Community Edition is acceptable).  Ensure when installing Visual Studio that you include 

	- **Microsoft Web Developer Tools**
	- **Universal Windows App Development Tools**

- To ensure that your computer is setup correctly, follow the steps under the **"Setup your PC"** section here: http://ms-iot.github.io/content/en-US/GetStarted.htm

## The Lab Hardware

The hardware used in this lab was based on the components available in a kit provided for attendees at a specific set of in-person events.  That said, the components are readily avaiable and relatively in-expensive. If you do not have one of the kits from an event you can use the information below to acquire the hardware independently. 

![00010-KitHardware](images/00010-KitHardware.jpg?raw=true "Kit Hardware")

### Kit Contents
Top to bottom, left to right:

- Micro USB Cable
- 5V/2A Power Supply ([Link](http://www.belkin.com/us/F8J052/p/P-F8J052/))
- Ethernet Cable
- USB to Ethernet Adapter ([Link](http://amzn.com/B00E9655LU))
- HDMI Cable
- Solderless Breadboard
- Bag of components 01
	- Red, Green, Blue and LEDs
	- RGB LED
	- Force Sensitive Resistor (FSR 402) ([Data Sheet](http://interlinkelectronics.com/datasheets/Datasheet_FSR.pdf))
	- Photoresistor (any will do) ([Data Sheet](http://www.token.com.tw/pdf/resistor/cds-resistor-pgm.pdf))
	- TMP36GZ Temperature Sensor ([Data Sheet](http://www.analog.com/media/en/technical-documentation/data-sheets/TMP35_36_37.pdf))
	- 16GB Micro SD Card with Adapter
	- SN74HC595N Shift Register ([Data Sheet](http://www.ti.com/lit/ds/symlink/sn74hc595.pdf))
	- MCP23008 I2C Port Expander ([Data Sheet](http://ww1.microchip.com/downloads/en/DeviceDoc/21919e.pdf))
	- MCP3008 10-bit Analog-to-Digital Converter (ADC) ([Data Sheet](http://ww1.microchip.com/downloads/en/DeviceDoc/21295d.pdf))

- Bag of components 02

	- SPST Momentary Push Buttons
	- 330&#937; Resistors
	- 10k&#937; Potentiometer
	- 10k&#937; Resistors

- Raspberry Pi 2
- Male to Female Jumpers

### Additional Components You'll Need to Provide

There are some additional components that you will need to be successful:

- **A Monitor with an HDMI input** (Or an an adapter that will adapt the HDMI cable from the Raspberry Pi to an appropriate connection on your monitor)

- A **USB Keyboard and Mouse** for the Raspberry Pi 

- A **Micro SD** or **SD Card Reader** for your PC. 

## Hands-On-Labs (HOLs)

This workshop has a number of Hands-On-Labs for you to walk through.  The overall workflow has been broken into a series of individual labs, but they are intended to be completed in order. 

| **Lab** | **Description**                   |
|:-------:|----------------------------------:|
|    1    | [Installing Windows 10 IoT Core](./HOLs/01-InstallingWindows10IotCore/Readme.htm)    |
|    2    | [Blinking the LED with UWP]((./HOLs/02-BlinkingTheLEDWithUWP/Readme.htm))         |
|    3    | [Working with Analog Sensors]((./HOLs/03-WorkingWithAnalogSensors/Readme.htm))       |
|    4    | [Intro to Azure IoT Hubs]((./HOLs/04-IntroToAzureIoTHubs/Readme.htm))           |

