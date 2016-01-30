using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Devices.Gpio;
using Windows.Devices.Spi;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace WindowsOnPi
{
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

    #region LED and Button Members

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

    #endregion LED and Button Members

  }
}
