# HOL 02 - Blinking the LED with UWP

## Overview

In this lab, we will start with a basic (but not over simplified) Universal Windows Platform (UWP) app, and run it on the Pi.  If you want a sample that is much simpler than what we do here, check out the <a href="http://ms-iot.github.io/content/en-US/win10/samples/Blinky.htm" target="_blank">Blinky Sample</a> from the <a href="http://ms-iot.github.io/content/en-US/win10/StartCoding.htm" target="_blank">Windows on Devices Docs, Tutorials and Samples</a>

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
- [Task 3: Run the UWP App on the Raspberry Pi 2](#Task3)
- [Task 4: Try Running the App Locally](#Task4)

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
	- Notice in the `InitAll()` method, that if any "**Exceptions**" (errors) are "**caught** (`catch (Exception ex)`), then it will get the message from the exception and show it in the StatusText `<TextBlock>` on the UI.  Otherwise, if ther are no exceptions, then it will show the string `"Status: Initialization Completed Successfully!"` in the StatusText `<TextBlock>`.

	- Lastly, there is a the `#region LED and Button Members` which has been commented out here.  We'll discuss that in more depth next. 

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

1.  In the **MainPage.xaml.cs** code file, expand the `LED and Button Members` region by clicking on the "+" button to the left of it.  The `LED and Button Members` contains the members (variables, methods, etc) that will help us work with the LED and momentary pushbutton we wired up to the Raspberry Pi earlier:

	> **Note**: "**Regions**" in visual studio are simply a method to organize a bunch of code in a file.  The allow you to create "regions" of code that can be collapsed or expanded in the editor.  They have no effect on what the code does, they are merely are an editor feature.  

	![01090-LEDAndButtonMembers](images/01090-ledandbuttonmembers.png?raw=true "LED and Button Members")

1. First there are a number of variable declarations.  These represent the GPIO Pin numbers that the LED and Button are connected to, as well as declare a `GpioPin` instance for each that we will use to interact with the pin on the Raspberry Pi. 

	````C#
	/// <summary>
	/// The GPIO Pin Number that the Cathode of the LED is connected to
	/// </summary>
	private const int LED_PIN = 5;

	/// <summary>
	/// A managed interface to the GPIO Pin the LED is connected to
	/// </summary>
	private GpioPin ledPin;

	/// <summary>
	/// The GPIO Pin Number that one leg of the momentary push button is connected to
	/// </summary>
	private const int BUTTON_PIN = 6;

	/// <summary>
	/// A managed interface to the GPIO Pin the button is connected to
	/// </summary>
	private GpioPin buttonPin;
	````
1. Next is the `InitGpio()` method that get's called from the `InitAll()` method we discussed previously.  The cool thing about this, and the IoT Extensions for UWP is that we can ***TRY*** to initialize the GpiController on the current board.  If we are on a board (like the Raspberry Pi 2) that has one, it should work.  If we however are running on our laptop, where there isn't a GpiController, we won't get the controller, but the app won't crash.  The app COULD still work, as long as it could still do something useful without the Gpio functionality.  Anyhow, if the GpioController is found, the LED and button pins are initialized, otherwise, an exception is thrown back to `InitAll()`, which will then use the exceptions message to update the StatusText `<TextBlock>`:

	````C#
	/// <summary>
	/// Initializes the GpioController on the board if it exists, and initializes the Led and Button pins.
	/// </summary>
	private void InitGpio()
	{
	  //Attempt to get the GpioController instance for the 
	  //device.  
	  var gpio = GpioController.GetDefault();

	  // If no GpioController was found (gpio==null), null out the pins
	  // And throw an exception indicating there was no GPIO controller
	  if (gpio == null)
	  {
		 ledPin = null;
		 buttonPin = null;
		 throw new Exception("There is no GPIO controller on this device.");
	  }

	  //Init LED pin
	  ledPin = gpio.OpenPin(LED_PIN);
	  ledPin.SetDriveMode(GpioPinDriveMode.Output);
	  SetLedState(false);

	  //Init Button pin
	  buttonPin = gpio.OpenPin(BUTTON_PIN);

	  //Remember Windows 10 IoT Core runs on multiple boards.
	  //If the board's pin that the button is connected to supports
	  //pullup resistors, enable the pullup resistor, otherwise, just use
	  //it as a normal input
	  if (buttonPin.IsDriveModeSupported(GpioPinDriveMode.InputPullUp))
	  {
		 buttonPin.SetDriveMode(GpioPinDriveMode.InputPullUp);
	  }
	  else
	  {
		 buttonPin.SetDriveMode(GpioPinDriveMode.Input);
	  }

	  //Set a debounce timeout to filter out switch bounce noice from a button press
	  buttonPin.DebounceTimeout = TimeSpan.FromMilliseconds(50);

	  //Wire up an event hander to be called whenever the button gets pressed.
	  buttonPin.ValueChanged += ButtonPin_ValueChanged;
	}
	````

1. Next the `SetLedState()` method is used to actually turn the pin that the LED is connected to on or off. It just does a little safety check to make sure that the GpioPin for the LED has been initialized (`ledPin!=null`). Also, because of the way the circuit is wired, we need to pull the pin LOW to turn the LED ON.  The comments explain it more thoroughly:

	````C#
	/// <summary>
	/// Used to turn the physical LED on or off.
	/// </summary>
	/// <param name="on">Boolean value representing the LED state.  
	/// true = on, false = off</param>
	void SetLedState(bool on)
	{
	  if (ledPin != null)
	  {
		 if (on)
		 {
			//Setting the pin LOW, pulls it down to ground
			//and allows current to flow from the 3.3v source
			//connected to the anode of the LED down through
			//the cathode, and thus the LED is turned ON
			ledPin.Write(GpioPinValue.Low);
		 }
		 else
		 {
			//Setting the pin High, pulls it up to 3.3v
			//which matches the 3.3v source connected 
			//to the anode of the LED thus no current
			//flows, and the LED is turned OFF
			ledPin.Write(GpioPinValue.High);
		 }
	  }
	}
	````
1. The `UpdateToggleButtonAndLED()` method is just a helper method that centralizes the logic to both update the text displayed on the TogglePinButton `<ToggleButton>` as well as to turn the LED on or off, based on a boolean value that is passed in.  

	````C#
	/// <summary>
	/// Used to update the Text on the TogglePinButton button controler
	/// As well as to turn the physical LED on or off.
	/// </summary>
	/// <param name="on">Boolean value representing the button and LED state.  
	/// true = on, false = off</param>
	private void UpdateToggleButtonAndLED(bool on)
	{
	  //If the toggle button is ON (Checked)
	  if (on)
	  {
		 //Turn the LED on.
		 SetLedState(true);

		 //Set the content (text) displayed on the button
		 //To indicate that the LED can now be turned OFF (since it is on)
		 TogglePinButton.Content = "Turn LED OFF";
	  }
	  else
	  {
		 //Turn the LED off.
		 SetLedState(false);

		 //Set the content (text) displayed on the button
		 //To indicate that the LED can now be turned ON (since it is off)
		 TogglePinButton.Content = "Turn LED ON";
	  }
	}
	````

1. If you recall, the TogglePinButton `<ToggleButton>` declared in **MainPage.xaml** had to events, `Checked` and `Unchecked` wired up to methods in the code behind file:
<!-- mark:3-4 -->
````XML
<ToggleButton 
  ...
  Checked="TogglePinButton_Checked"
  Unchecked="TogglePinButton_Unchecked"/>
````

1. Those event handlers are what are declared next in the `LED and Button Members` region.  The `TogglePinButton_Checked` method is called when the users turns ON the toggle button, and the `TogglePinButton_Unchecked` is called when the toggle button is turned off.  Both methods simply leverage the `UpdateToggleButtonAndLED()` method we just discussed, passing in a boolean value representing the current state of the button.  

	````C#
	/// <summary>
	/// Fired whenever the TogglePinButton is "Checked" (ON)
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	private void TogglePinButton_Checked(object sender, RoutedEventArgs e)
	{
	  //Update the toggle button text and turn on the led
	  UpdateToggleButtonAndLED(true);
	}

	/// <summary>
	/// Fired whenever the TogglePinButton is "Unchecked" (OFF)
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	private void TogglePinButton_Unchecked(object sender, RoutedEventArgs e)
	{
	  //Update the toggle button text and turn off the led
	  UpdateToggleButtonAndLED(false);
	}
	````

1. Lastly, is the handler for the Physical pushbutton.  This pushbutton pin, and it's ValueChanged event event handler initiazlied up in the `InitGpio()` method.  We finally get around to telling the Pi what to do when the user pushes the physical push button. The method is already heavily commented, it is left to the reader to read them!  

	````C#
	/// <summary>
	/// Fired by the GpioController whenever the physical push button's value changes
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="args"></param>
	private void ButtonPin_ValueChanged(GpioPin sender, GpioPinValueChangedEventArgs args)
	{
	  //The arguments (args) provided to the event handler indicate if the button
	  //is a FallingEdge (going from 3.3V to 0v) or RisingEdige (going from 0v to 3.3v)
	  //Since the GPIO pin the button is connected to was initialized with it's pullup 
	  //resistor ON, when the button is NOT PRESSED the pin is pulled UP to 3.3v.  
	  //When the button IS PRESSED the pin values "falls" from 3.3V to 0V.  
	  //So when we see a FallingEdge, we know the button is PUSHED (on == true)
	  //otherwise, it is off (on == false)
	  bool on = args.Edge == GpioPinEdge.FallingEdge;

	  //We will use the "on" boolean value we just got from the physical button 
	  //to set the state of the XAML ToggleButton's state in the UI.  
	  //To do that, we can  TogglePinButton.IsChecked boolean propety to 
	  //match the value of the "on" variable we just set.  However, since that 
	  //causes a UI update (the button's visual state changes) we have to run that
	  //code on the main UI thread.  We can do that by using the App's main UI
	  //Dispatcher
	  var task = Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
		 TogglePinButton.IsChecked = on;
	  });
	}
	````

---

<a name="Task3"></a>
### Task 3: Run the UWP App on the Raspberry Pi 2

Ok, we've explored the **WindowsOnPi** sample app pretty completely.  **LETS RUN IT!**.

1. In Visual Studio, on the toolbar along the top, first make sure that the Target platform is set to "**ARM**"

	![03010-TargetArmPlatform](images/03010-targetarmplatform.png?raw=true "Target ARM Platform")

1. In the "**Windows IoT Core Watcher**" app, locate the entry for your Raspberry Pi, and copy it's IP address by right-clicking on it, and selecting "**Copy IP Address**" from the pop-up menu:

	![03020-CopyIpFromWatcher](images/03020-copyipfromwatcher.png?raw=true "Copy IP Address From IoT Core Watcher")

1. Back in Visual Studio, from the Target Device drop down, select "**Remote Machine**"

	![03030-TargetRemoteMachine](images/03030-targetremotemachine.png?raw=true "Target Remote Machine")

1. The first time you select "**Remote Machine**" as a target, the "**Remote Connections**" window should appear.  The "**Auto Detected**" devices _rarely_ match your Pi, so it is generally best to enter your setting manually. Under the "**Manual Configuration**" heading:

	- Address: Paste the **IP Address for your Pi** you just copied from "**Windows IoT Core Watcher**" (or just type it in) 
	- Authentication Mode: **Universal (Unencrypted Protocol)**
	- Click "**Select**"

	![03040-RemoteConnections](images/03040-remoteconnections.png?raw=true "Remote Connections Window")

1. If the "**Remote Connections**" window didn't appear, the remote target was likely set previously.  You can always change it by going to the project's properites, and on the Debug tab, setting the target there:

	![03050-ProjectDebugProperties](images/03050-projectdebugproperties.png?raw=true "ProjectDebugProperties")
	
1. On the Visual Studio toolbar, click the "**Remote Machine**" button (or press **F5** on your computer keyboard to start debugging):

	![03060-DebugOnRemoteMachine](images/03060-debugonremotemachine.png?raw=true "DebugOnRemoteMachine")

1. On the Raspberry Pi 2's HDMI Monitor, you should see the UI for the app display:

	- Note the "**Status: Initialization Completed Successfully!" status.  That means that the GpioController was found, and the LED and Button pins where initialized correctly. 

	- The TogglePinButton `<ToggleButton>` is currently OFF (not checked).  You can tell because it has a grey background, and the text on it reads "Turn LED ON".

	![03070-UiOnPi](images/03070-uionpi.png?raw=true "UI on Pi")

1. If you look at the LED, you should see that it is currently NOT lit:

	![03080-LEDOff](images/03080-ledoff.png?raw=true "LED Off")

	- Using the mouse on attached to the Raspberry Pi, click on the `<ToggleButton>` to toggle it's state:

	![03090-ToggleButton](images/03090-togglebutton.png?raw=true "Toggle the Toggle Button")

1. You should also see that the LED is now lit:

	![03100-LEDOn](images/03100-ledon.png?raw=true "LED On")

1. Finally, try pushing the physical pushbutton.  You should see that when you push the button, the LED turns on, and when you release it, the LED turns off.  Not only that, but the TogglePinButton `<ToggleButton>`'s state updates to match the physical button.  That is because in the physical button's "ValueChanged" event handler, we had it basically just set the ToggleButton.IsChecked state to drive the UI and LED state.  So turning the physcial button on (by pushing it) does exactly the same thing as turing the `<ToggleButton>` button control on, and turning the physical button off (by releasing it) does exactly the same thing as turning the `<ToggleButton>` control off. Kinda cool.


	![03110-FingerOffButton](images/03110-fingeroffbutton.png?raw=true "Finger Off Button")

	![03120-FingerOnButton](images/03120-fingeronbutton.png?raw=true "Finger On Button")

1. When you are done debugging your app on the Pi, in Visual Studio, click the "**STOP**" button on toolbar (_Shift+F5_):

	![03130-StopDebugging](images/03130-stopdebugging.png?raw=true "Stop Debugging")

1. When your app has stopped debugging on the Pi, notice that the Raspberry Pi reverts back to the default IoT Core application.  Only a SINGLE app can control the UI on Windows IoT Core at a time.  

	![03140-DefaultApp](images/03140-defaultap.png?raw=true "Default App")

---

<a name="Task4"></a>
### Task 4: Try Running the App Locally

In this last task, we're simply going to run the same app we just ran on the Raspberry Pi 2, but this time we'll run it our our laptop instead.  Why?  Well it proves the point of UWP apps being "Universal".  They CAN run on my Windows platform.  That means that if you have an app ***THAT MAKES SENSE*** to run on say Windows, Phone, XBOX, HoloLens, and or the Raspberry Pi, you can do so.  Of course, that doesn't mean you **HAVE** to run your app everywhere. The choice is yours.  

1. Back in Visual Studio on your computer change the Target platform to x86:

	![04010-TargetX86](images/04010-targetx86.png?raw=true "Target x86")

1. You should see that the target debug device automatically switched back to "**Local Machine**".  If not, from the drop down, select "**Local Machine**":

	![04020-DebugTargetLocalMachine](images/04020-debugtargetlocalmachine.png?raw=true "Debug Target Local Machine")

1.  Next, click the Debug button (or press _F5_) to start debugging on the local machine. 

	![04030-DebugLocally](images/04030-debuglocally.png?raw=true "Debug Locally")

1. When the app runs, notice that the StatusText `<TextBlock>` states that there is no GPIO controller on the device.  That message came from the `InitGpio()` method when it tried to initialize the GpioController.  

	![04040-NoGpioController](images/04040-nogpiocontroller.png?raw=true "No Gpio Controller")

1. However the toggle button still works, although, with no GpioController, and no LEDs, there is no obvious effect other than the button's background color changing:

	![04040-ToggleButtonWorks](images/04040-togglebuttonworks.png?raw=true "Toggle Button Works")

1. What does that mean?  Well as a developer you may have a need for an app that runs on multiple devices, but takes advantages of whatever features are on the device where it runs.  Here we can see we have an app that runs, albeit with limited functionality on one platform, but takes advantage of available hardware when running on another platform.  

1. When you are done testing, stop debugging in Visual Studio to close the app. 

1. Close Visual Studio when you are done.  We will open a fresh copy of the **WindowsOnPi** solution in the next lab. 