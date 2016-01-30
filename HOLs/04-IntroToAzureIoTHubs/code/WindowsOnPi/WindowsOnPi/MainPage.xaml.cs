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
        await InitAdc();    // Initialize the SPI bus for communicating with the ADC
        InitIotHub();       // Initialize the IoT Hub sensors and connections
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

    #region ADC/SPI Members

    /// <summary>
    /// An enumeration of the various types of ADC Chips
    /// </summary>
    enum AdcChip { NONE, MCP3002, MCP3208, MCP3008 };

    /// <summary>
    /// Type type of Analog Digital Converter (ADC) chip being used
    /// </summary>
    private AdcChip ADC_CHIP = AdcChip.MCP3008;

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

    #endregion ADC/SPI members

    #region IoT Hub Members

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

      //iotHubHelper = new IoTHubHelper(
      //  iotDeviceConnectionString: "PASTE_YOUR_IOT_HUB_DEVICE_CONNECTION_STRING_HERE",
      //  organization: "ENTER_A_NAME_FOR_YOUR_ORGANIZATION_HERE",
      //  location: "ENTER_A_NAME_FOR_YOUR_DEVICES_LOCATION_HERE");

      iotHubHelper = new IoTHubHelper(
        iotDeviceConnectionString: "HostName=OcAzureHub.azure-devices.net;DeviceId=WindowsOnPiDemo;SharedAccessKey=5vOBQ9DHuSSwElIRUHqxdwivJNy3O59UnSX3i0wTBIA=",
        organization: "Microsoft",
        location: "Bret's Garage",
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

    #endregion IoT Hub Members
  }
}
