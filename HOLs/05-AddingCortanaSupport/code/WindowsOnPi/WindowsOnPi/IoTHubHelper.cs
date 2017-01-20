using Microsoft.Azure.Devices.Client;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsOnPi
{
  class IoTHubHelper
  {

    public List<IoTHubSensor> sensors;

    public string IoTHubDeviceConnectionString { get; set; }
    public string Organization { get; set; }
    public string Location { get; set; }
    public bool HubConnectionInitialized {get; set; } = false;

    // Http connection string, SAS tokem and client
    DeviceClient deviceClient;


    public IoTHubHelper(string iotDeviceConnectionString = "",
        string organization = "",
        string location = "",
        List<IoTHubSensor> sensorList = null)
    {
      IoTHubDeviceConnectionString = iotDeviceConnectionString;
      Organization = organization;
      Location = location;
      sensors = sensorList;

      //Update the sensors org and location to match the above data
      ApplySettingsToSensors();

      //Initialize the connection to the IoTHub
      InitIoTHubConnection();
    }

    /// <summary>
    ///  Apply settings to sensors collection
    /// </summary>
    public void ApplySettingsToSensors()
    {
      foreach (IoTHubSensor sensor in sensors)
      {
        sensor.location = Location;
        sensor.organization = Organization;
      }
    }

    private void SendAllSensorData()
    {
      foreach (IoTHubSensor sensor in sensors)
      {
        sensor.timecreated = DateTime.UtcNow.ToString("o");
        sendMessage(sensor.ToJson());
      }
    }

    public void SendSensorData(IoTHubSensor sensor)
    {
      sensor.timecreated = DateTime.UtcNow.ToString("o");
      sendMessage(sensor.ToJson());
    }

    /// <summary>
    /// Send message to an IoT Hub using IoT Hub SDK
    /// </summary>
    /// <param name="message"></param>
    public async void sendMessage(string message)
    {
      if (this.HubConnectionInitialized)
      {
        try
        {
          var content = new Message(Encoding.UTF8.GetBytes(message));
          await deviceClient.SendEventAsync(content);

          Debug.WriteLine("Message Sent: {0}", message, null);
        }
        catch (Exception e)
        {
          Debug.WriteLine("Exception when sending message:" + e.Message);
        }
      }
    }


    /// <summary>
    /// Receive a message from the IoTHub via the SDK
    /// </summary>
    /// <returns>An awaitable task with a string result which is the read message</returns>
    public async Task<string> ReceiveMessage()
    {
      if (this.HubConnectionInitialized)
      {
        try
        {
          var receivedMessage = await this.deviceClient.ReceiveAsync();

          if (receivedMessage != null)
          {
            var messageData = Encoding.ASCII.GetString(receivedMessage.GetBytes());
            this.deviceClient.CompleteAsync(receivedMessage);
            Debug.WriteLine(string.Format("Received Message: {0}", messageData));
            return messageData;
          }
          else
          {
            return string.Empty;
          }
        }
        catch (Exception e)
        {
          string message = string.Format("Exception when receiving message: {0}", e.Message);
          Debug.WriteLine(message);
          return string.Empty;
        }
      }
      else
      {
        return string.Empty;
      }
    }

    /// <summary>
    /// Initialize Hub connection
    /// </summary>
    public bool InitIoTHubConnection()
    {
      try
      {
        this.deviceClient = DeviceClient.CreateFromConnectionString(IoTHubDeviceConnectionString);
        this.HubConnectionInitialized = true;
        return true;
      }
      catch (Exception ex)
      {
        return false;
      }
    }

  }
}
