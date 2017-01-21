using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

//Added for Cortana Support
using Windows.Media.SpeechRecognition;
using Windows.ApplicationModel.VoiceCommands;
using Windows.Storage;
using System.Diagnostics;
using Windows.UI.Popups;

namespace WindowsOnPi
{
  /// <summary>
  /// Provides application-specific behavior to supplement the default Application class.
  /// </summary>
  sealed partial class App : Application
  {
    /// <summary>
    /// Initializes the singleton application object.  This is the first line of authored code
    /// executed, and as such is the logical equivalent of main() or WinMain().
    /// </summary>
    public App()
    {
      this.InitializeComponent();
      this.Suspending += OnSuspending;
    }

    /// <summary>
    /// Invoked when the application is launched normally by the end user.  Other entry points
    /// will be used such as when the application is launched to open a specific file.
    /// </summary>
    /// <param name="e">Details about the launch request and process.</param>
    protected async override void OnLaunched(LaunchActivatedEventArgs e)
    {

#if DEBUG
      if (System.Diagnostics.Debugger.IsAttached)
      {
        this.DebugSettings.EnableFrameRateCounter = false;
      }
#endif

      Frame rootFrame = Window.Current.Content as Frame;

      // Do not repeat app initialization when the Window already has content,
      // just ensure that the window is active
      if (rootFrame == null)
      {
        // Create a Frame to act as the navigation context and navigate to the first page
        rootFrame = new Frame();

        rootFrame.NavigationFailed += OnNavigationFailed;

        if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
        {
          //TODO: Load state from previously suspended application
        }

        // Place the frame in the current Window
        Window.Current.Content = rootFrame;

      }

      if (rootFrame.Content == null)
      {
        // When the navigation stack isn't restored navigate to the first page,
        // configuring the new page by passing required information as a navigation
        // parameter
        rootFrame.Navigate(typeof(MainPage), e.Arguments);
      }
      // Ensure the current window is active
      Window.Current.Activate();

      await InstallVoiceCommandDefinitions();
    }

    /// <summary>
    /// Invoked when Navigation to a certain page fails
    /// </summary>
    /// <param name="sender">The Frame which failed navigation</param>
    /// <param name="e">Details about the navigation failure</param>
    void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
    {
      throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
    }

    /// <summary>
    /// Invoked when application execution is being suspended.  Application state is saved
    /// without knowing whether the application will be terminated or resumed with the contents
    /// of memory still intact.
    /// </summary>
    /// <param name="sender">The source of the suspend request.</param>
    /// <param name="e">Details about the suspend request.</param>
    private void OnSuspending(object sender, SuspendingEventArgs e)
    {
      var deferral = e.SuspendingOperation.GetDeferral();
      //TODO: Save application state and stop any background activity
      deferral.Complete();
    }

    /// <summary>
    /// Installs the custom Cortana Voice Commands.  This method is called by OnLaunch when the app loads. 
    /// </summary>
    /// <returns></returns>
    private static async System.Threading.Tasks.Task InstallVoiceCommandDefinitions()
    {
      try
      {
        StorageFile vcd = await Package.Current.InstalledLocation.GetFileAsync(@"VoiceCommandDefinitions.xml");
        await VoiceCommandDefinitionManager.InstallCommandDefinitionsFromStorageFileAsync(vcd);
        string msg = "Successfully installed the command definitions";
        Debug.Write(msg);

      }
      catch (Exception ex)
      {
        string msg = string.Format("An {0} occurred when installing the Voice Command Definitions: {1}", ex.GetType().Name, ex.Message);
        Debug.Write(msg);
      }
    }

    /// <summary>
    /// This runs when the app is activated.  Here we check to see if a voice command was used to activate the app, and if so we pass that information along to MainPage. 
    /// </summary>
    /// <param name="args"></param>
    protected async override void OnActivated(IActivatedEventArgs args)
    {
      base.OnActivated(args);

      if (args.Kind == ActivationKind.VoiceCommand)
      {
        VoiceCommandActivatedEventArgs cmd = args as VoiceCommandActivatedEventArgs;
        if (cmd != null)
        {
          SpeechRecognitionResult result = cmd.Result;

          // Re"peat the same basic initialization as OnLaunched() above, taking into account whether
          // or not the app is already active.
          Frame rootFrame = Window.Current.Content as Frame;

          // Do not repeat app initialization when the Window already has content,
          // just ensure that the window is active
          if (rootFrame == null)
          {
            // Create a Frame to act as the navigation context and navigate to the first page
            rootFrame = new Frame();

            rootFrame.NavigationFailed += OnNavigationFailed;

            // Place the frame in the current Window
            Window.Current.Content = rootFrame;
          }

          // Since we're expecting to always show a details page, navigate even if 
          // a content frame is in place (unlike OnLaunched).
          // Navigate to either the main trip list page, or if a valid voice command
          // was provided, to the details page for that trip.
          rootFrame.Navigate(typeof(MainPage), result);

          // Ensure the current window is active
          Window.Current.Activate();

          //dlg.ShowAsync();
        }
      }
    }
  }
}
