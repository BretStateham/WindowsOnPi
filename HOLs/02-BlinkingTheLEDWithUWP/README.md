# HOL 02 - Blinking the LED with UWP

## Overview

In this lab, we will start with a relatively simple Universal Windows Platform (UWP) app, and run it on the Pi.  If you want a sample that is even simpler than what we do here, check out the <a href="http://ms-iot.github.io/content/en-US/win10/samples/Blinky.htm" target="_blank">Blinky Sample</a> from the <a href="http://ms-iot.github.io/content/en-US/win10/StartCoding.htm" target="_blank">Windows on Devices Docs, Tutorials and Samples</a>

## Pre-Requisites

To complete this lab you must have completed the steps in lab [01 - Installing Windows 10 IoT Core](../HOLs/01-InstallingWindows10IotCore/) 

In addition to the pre-reqs from the [prevoius lab](../HOLs/01-InstallingWindows10IotCore/), you will need:

- Visual Studio 2015 with Update 1 with the following features:
	- "Microsoft Web Developer Tools"
	- "Universal Windows App Development Tools"
- A Solderless Breadboard
- A 5mm LED
- A 330&#0937; Resistor
- A SPST Momentary Pushbutton
- Multiple Male/Female and Male/Male jumper wires.

## Setup

Ensure that you have completed the pre-requisites above. 

## Tasks

- [Task 1: Complete the LED and Pushbutton Circuit](#Task1)
- [Task 2: Open and review the UWP Project](#Task2)
- [Task 3: Finally Do This](#Task3)

---

<a name="Task1"></a>
### Task 1: Complete the LED and Pushbutton Circuit

In this task you will complete the circuit needed for our first app. 

1. <span style="color: red;">***REMOVE THE POWER AND HDMI CABLES FROM YOUR RASPBERRY PI 2***</span>.  This will ensure that you do not accidentally short-out anything on the board while you are completing the circuit. 

1.  As a reference, you can identify the pins on the Raspberry Pi 2 using the following image.

	![01020-RPi2Pinouts](images/01020-rpi2pinouts.png?raw=true "Raspberry Pi 2 Pinouts")

1. Use the following diagram to complete the circuit. <span style="color: red;">***WE WILL BE ADDING ADDITIONAL COMPONENTS IN THE NEXT LAB, DO YOUR BEST TO LAYOUT THE BREADBOARD AS DIGRAMMED BELOW.  THIS WILL ENSURE THERE IS ROOM FOR THE ADDITIONAL COMPONENTS ADDED IN THE NEXT LAB***</span>:

	![01010-LedAndPushbuttonCircuit](images/01010-ledandpushbuttoncircuit.png?raw=true "LED and Pushbutton Circuit") 

1. Double check your work, and when you are certain the circuit looks correct, re-connect the HDMI cable, then the Power cable to your Raspberry Pi 2 to boot it up. 

---

<a name="Task2"></a>
### Task 2: Open and review the UWP Project

Next, we'll open an existing UWP application that is designed to interact with the LED and Pushbutton from the circuit above if it is being run on a device that has a General Purpose Input Output (GPIO) Controller, like the Raspberry Pi. 

The cool thing about UWP apps though, is that we can write them in such a way as to allow them to continue to function, at least to some degree, even if the device they are running on does not support all of the hardware requirements of the app. 

1. In the folder where you have exracted the files for these Hands-On-Labs, locate the `HOLs\02-BlinkingTheLEDWithUWP\code\WindowsOnPi` folder, and double click on the `WindowsOnPi.sln` solution file to open it in Visual Studio.
1. Step 2
1. Step 3

---

<a name="Task3"></a>
### Task 3: Finally Do This

Finally, we'll do this

1. Step 1
1. Step 2
1. Step 3