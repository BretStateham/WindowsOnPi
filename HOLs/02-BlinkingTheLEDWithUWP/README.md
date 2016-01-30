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

	![01030-WindowsOnPiSolutionFile](images/01030-windowsonpisolutionfile.png?raw=true "WindowsOnPi Solution File")

1. When the solution is opened in Visual Studio, review the contents of the Solution in the Visual Studio "Solution Explorer" window:

	- The **References** folder contains references to the Universal Windows Platform, and the "**Windows IoT Extensions for the UWP**".  This adds support for boards like the Raspberry Pi 2 to UWP apps.
	
	- **MainPage.xaml** is an eXtensible Application Markup Language (XAML) file that describes the user interface elements of the app's "Main Page".

	- **MainPage.xaml.c**s is a C# code file that implements the logic behind the "Main Page". Because it sits "behind" the main page UI, we call it a "code behind" file.  

	![01040-SolutionContents](images/01040-solutioncontents.png?raw=true "WIndowsOnPi Solution Contents")

1. First, rebuild the solution to make sure that it compiles.  From the Visual Studio menu bar, select "**Build**" | "**Rebuild Solution**"

	![01045-RebuildSolution](images/01045-rebuildsolution.png?raw=true "Rebuild Solution")

1. In the Visual Studio "**Outpu**" window (press Ctrl+Alt+O if it isn't visible), verify that the rebuild was successfull

	![01047-RebuildSuccessful](images/01047-rebuildsuccessful.png?raw=true "Rebuild Successful")

1. Double click on the **MainPage.xaml** file to open it in the designer and view the User Interface for the app:

	![01050-MainPageUI](images/01050-mainpageui.png?raw=true "Main Page UI")

1. To make the preview of the XAML on the design surface better, change the target device to a "13.3&quot; Desktop (1280x720) 100% scale" or something similar:

	![01060-ChangeDesignerTargetDevice](images/01060-changedesignertargetdevice.png?raw=true "Change Desitner Target Device")

1. If you are have experience with using Visual Studio to create Windows Forms, WPF, SilverLight or UWP apps the layout should be familiar to you.  If not, here is a quick overview of the Visual Studio layout:

	- 1: The "**Solution Explorer**" window (_Ctrl+Alt+L_) shows you the contents of the solution you have open (the files, etc in your project)
	- 2: The "**XAML**" window (_visible only when you have a XAML document open_) shows you the XAML markup window for the XAML document you have open.
	- 3: The "**Design**" window (_visible only when you have a XAML document open_) shows you a visual representation of the UI that your XAML describes
	- 4: The "**Properties**" window (_F4_) shows you the properties that are set on the XAML element you currently have selected.
	- 5: The "**Document Outline**" window (_Ctrl+Alt+T_) shows you a hierarchical represenation of your XAML markup.  

	![01070-VisualStudioLayout](images/01070-visualstudiolayout.png?raw=true "Visual Studio Layout")

1. Review the XAML markup in the MainPage.xaml file.  You can use the hierarchy in the "**Document Outline**" window to select items and find their corresponding XAML markup, design representation and Properties:

	````XML
	<Page
		 x:Class="WindowsOnPi.MainPage"
		 xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
		 xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
		 xmlns:local="using:WindowsOnPi"
		 xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
		 xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
		 mc:Ignorable="d">

	  <Grid Background="{ThemeResource ApplicationPageBackgroundThemeBrush}">
		 <Grid.RowDefinitions>
			<RowDefinition Height="Auto" />
			<RowDefinition Height="*" />
			<RowDefinition Height="*" />
			<RowDefinition Height="*" />
			<RowDefinition Height="Auto" />
		 </Grid.RowDefinitions>
		 <Grid.ColumnDefinitions>
			<ColumnDefinition Width="*" />
			<ColumnDefinition Width="*" />
		 </Grid.ColumnDefinitions>

		 <TextBlock 
			x:Name="TitleText"
			Text="Windows On Pi Demo App"
			Grid.Row="0" Grid.Column="0" Grid.ColumnSpan="2"
			FontSize="48" FontWeight="Bold"
			HorizontalAlignment="Center"
			VerticalAlignment="Center"
			Margin="10"/>
		 
		 <TextBlock
			x:Name="StatusText"
			Text="STATUS:"
			Grid.Row="1" Grid.Column="0" Grid.ColumnSpan="2"
			FontSize="36" FontWeight="Bold"
			HorizontalAlignment="Center"
			VerticalAlignment="Center"
			Margin="10"/>

		 <ToggleButton 
			x:Name="TogglePinButton"
			Grid.Row="2" Grid.Column="0" Grid.ColumnSpan="2" 
			Content="Turn LED ON"
			FontSize="24" FontWeight="Bold"
			HorizontalAlignment="Stretch"
			VerticalAlignment="Stretch"
			Margin="10" 
			Checked="TogglePinButton_Checked"
			Unchecked="TogglePinButton_Unchecked"/>
		 
	  </Grid>
	</Page>
	````

1. From the Markup above, you can see that we have:

	- A `<Grid>` with a number of rows (`<RowDefinitions>`) and columns (`<ColumnDefinitions>`)
	- A `<TextBlock>` that displays the title text on the page and that spans two columns (`Grid.ColumnSpan="2"`) in the top row (`Grid.Row="0"`) of the grid. 
	- Another `<TextBlock>` that spans two columns in the second row (`Grid.Row="1"`) which will be used to show status text.  
	- A `<ToggleButton>` in the third row that will be used to allow the user to toggle (turn on or off) an LED.  It has two events pointing to methods in the **MainPage.xaml.cs** code behind file `Checked="TogglePinButton_Checked"` and `Unchecked="TogglePinButton_Unchecked"`

1. Next, open the **MainPage.xaml.cs** file by double clicking on it in the "**Solution Explorer**" window.  

	![01080-MainPageCodeBehind](images/01080-mainpagecodebehind.png?raw=true "MainPage Code Behind")

1. There is a fair amount of code in this file so we'll break it down into pieces. In the following code block we have:

	- A class called `MainPage`
	- A Constructor (also called `MainPage`) that gets run automatically when an instance of the class is created.  Since `MainPage` implements the UI for our app, the app automatically creates it at run it, and that is when the constructor gets called. 	The constructor initializes the elements on the page (`this.InitializeComponent()`) and calls a method written for this lab called `IntiAll()`.
	- The `InitAll()` method was written for this lab, and is first used to initialize the "General Purpose Input Output" (GPIO) functionality ***IF IT EXISTS*** on the board where the app runs by calling another method called `InitGpio()`.  Whey have one init method that calls another? We'll we'll have other things to "init" later, like Serial Peripheral Interface (SPI) connections to an Analog Digital Converter (ADC), and connections to an Azure IoTHub.  So this one `InitAll()` method was created so we can easily call the other init methods from it in the future in a nice managed way. 
-	Lastly, notice in the `InitAll()` method, that if any "**Exceptions**" (errors) are "**caught** (`catch (Exception ex)`), then it will get the message from the exception and show it in the StatusText `<TextBlock>` on the UI.  Otherwise, if ther are no exceptions, then it will show the string `"Status: Initialization Completed Successfully!"` in the StatusText `<TextBlock>`.

	````C#
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class MainPage : Page
	{

	  /// <summary>
	  /// MainPage Constructor
	  /// </summary>
	  public MainPage()
	  {
		 this.InitializeComponent();

		 //Single method that calls all other required InitX methods
		 InitAll();
	  }

	  /// <summary>
	  /// One Init method to rule them all.  This one method calls all the other required InitX methods.
	  /// </summary>
	  private async void InitAll()
	  {
		 try
		 {
			InitGpio();         // Initialize GPIO to toggle the LED
		 }
		 catch (Exception ex)
		 {
			StatusText.Text = ex.Message;
			return;
		 }
		 StatusText.Text = "Status: Initialization Completed Successfully!";
	  }

	  // ... LED and Button Members ...

	}
	````



1.  

---

<a name="Task3"></a>
### Task 3: Finally Do This

Finally, we'll do this

1. Step 1
1. Step 2
1. Step 3