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

- **A computer running Windows 10**.  You can't develop Windows 10 Apps for the Pi without Windows 10 on your host computer.  You can <a href="https://www.microsoft.com/en-us/windows/windows-10-upgrade" target="_blank">upgrade your existing copy of Windows 7 or Windows 8.1 to Windows 10 for Free</a>.  Or if you don't have Windows you can <a href="https://www.microsoft.com/en-us/evalcenter/evaluate-windows-10-enterprise" target="_blank">download a 90 Day Evaluation Copy of Windows 10 Enterprise</a> 
- **Visual Studio 2015 with Update 1** (the Community Edition is acceptable).  Ensure when installing Visual Studio that you include 

	- **Microsoft Web Developer Tools**
	- **Universal Windows App Development Tools**

- To ensure that your computer is setup correctly, follow the steps under the **"Setup your PC"** section here: <a href="http://ms-iot.github.io/content/en-US/GetStarted.htm" target="_blank">http://ms-iot.github.io/content/en-US/GetStarted.htm</a>

## The Lab Hardware

The hardware used in this lab was based on the components available in a kit provided for attendees at a specific set of in-person events.  That said, the components are readily avaiable and relatively in-expensive. If you do not have one of the kits from an event you can use the information below to acquire the hardware independently. 

![00010-KitHardware](images/00010-KitHardware.jpg?raw=true "Kit Hardware")

### Kit Contents
Top to bottom, left to right:

- Micro USB Cable
- 5V/2A Power Supply (<a href="http://www.belkin.com/us/F8J052/p/P-F8J052/" target="_blank">Link</a>)
- Ethernet Cable
- USB to Ethernet Adapter (<a href="http://amzn.com/B00E9655LU" target="_blank">Link</a>)
- HDMI Cable
- Solderless Breadboard
- Bag of components 01
	- Red, Green, Blue and LEDs
	- RGB LED
	- Force Sensitive Resistor (FSR 402) (<a href="http://interlinkelectronics.com/datasheets/Datasheet_FSR.pdf" target="_blank">Data Sheet</a>)
	- Photoresistor (any will do) (<a href="http://www.token.com.tw/pdf/resistor/cds-resistor-pgm.pdf" target="_blank">Data Sheet</a>)
	- TMP36GZ Temperature Sensor (<a href="http://www.analog.com/media/en/technical-documentation/data-sheets/TMP35_36_37.pdf" target="_blank">Data Sheet</a>)
	- 16GB Micro SD Card with Adapter
	- SN74HC595N Shift Register (<a href="http://www.ti.com/lit/ds/symlink/sn74hc595.pdf" target="_blank" >Data Sheet</a>)
	- MCP23008 I2C Port Expander (<a href="http://ww1.microchip.com/downloads/en/DeviceDoc/21919e.pdf" target="_blank">Data Sheet</a>)
	- MCP3008 10-bit Analog-to-Digital Converter (ADC) (<a href="http://ww1.microchip.com/downloads/en/DeviceDoc/21295d.pdf" target="_blank">Data Sheet</a>)

- Bag of components 02

	- SPST Momentary Push Buttons
	- 330&#937; Resistors
	- 10k&#937; Potentiometer
	- 10k&#937; Resistors

- Raspberry Pi 2
- Male to Female Jumpers

### Additional Components You'll Need to Provide

There are some additional components that you will need to be successful:

- **A Monitor with an HDMI input** (Or an adapter that will adapt the HDMI cable from the Raspberry Pi to an appropriate connection on your monitor)

- A **USB Keyboard and Mouse** for the Raspberry Pi 

- A **Micro SD** or **SD Card Reader** for your PC. 

## Hands-On-Labs (HOLs)

This workshop has a number of Hands-On-Labs for you to walk through.  The overall workflow has been broken into a series of individual labs, but they are intended to be completed in order. 

  <div class="container">
    <div class="panel panel-default">
      <div class="panel-heading">
      <h3 class="panel-title">Workshop Hands-On-Labs</h3>
      </div>
      <div class="panel-body">
        <table class="table">
          <thead>
            <tr>
              <th>Lab</th>
              <th>Description</th>
            </tr>
          </thead>
          <tbody>
            <tr>
              <td>1</td>
              <td>[Installing Windows 10 IoT Core](HOLs/01-InstallingWindows10IotCore/) </td>
            </tr>
            <tr>
              <td>2</td>
              <td>[Blinking the LED with UWP](HOLs/02-BlinkingTheLEDWithUWP/) </td>
            </tr>
            <tr>
              <td>3</td>
              <td>[Working with Analog Sensors](HOLs/03-WorkingWithAnalogSensors/) </td>
            </tr>
            <tr>
              <td>4</td>
              <td>[Intro to Azure IoT Hubs](HOLs/04-IntroToAzureIoTHubs/) </td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>
  </div>
  
