using Foundation;
using UIKit;

using WindowsAzure.Messaging;

namespace AzurePush
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the
    // User Interface of the application, as well as listening (and optionally responding) to application events from iOS.
    [Register("AppDelegate")]
    public class AppDelegate : UIApplicationDelegate
    {
        // class-level declarations

        private SBNotificationHub Hub { get; set; }

		public const string ConnectionString = "Endpoint=sb://...";
		public const string NotificationHubPath = "AzurePushHub";

        public override UIWindow Window
        {
            get;
            set;
        }

        public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
        {
            // Set our preferred notification settings for the app
			var pushSettings = UIUserNotificationSettings.GetSettingsForTypes(
				UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound,
				new NSSet());

            // Register for notifications
			UIApplication.SharedApplication.RegisterUserNotificationSettings(pushSettings);
			UIApplication.SharedApplication.RegisterForRemoteNotifications();

            return true;
        }

		public override void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
		{
            // Create a new notification hub with the connection string and hub path
			Hub = new SBNotificationHub(ConnectionString, NotificationHubPath);

            // Unregister any previous instances using the device token
			Hub.UnregisterAllAsync(deviceToken, (error) =>
			{
				if (error != null)
				{
					// Error unregistering
					return;
				}

                // Register this device with the notification hub
				Hub.RegisterNativeAsync(deviceToken, null, (registerError) =>
				{
                    if (registerError != null) 
                    {
						// Error registering
					}
				});
			});
		}

        public override void ReceivedRemoteNotification(UIApplication application, NSDictionary userInfo)
        {
            // This method is called when a remote notification is received and the
            // App is in the foreground - i.e., not backgrounded

            // We need to check that the notification has a payload (userInfo) and the payload
            // has the root "aps" key in the dictionary - this "aps" dictionary contains defined
            // keys by Apple which allows the system to determine how to handle the alert
            if (null != userInfo && userInfo.ContainsKey(new NSString("aps")))
            {
                // Get the aps dictionary from the alert payload
                NSDictionary aps = userInfo.ObjectForKey(new NSString("aps")) as NSDictionary;

                // Here we can do any additional processing upon receiving the notification

                // As the app is in the foreground, we can handle this alert manually
                // here by creating a UIAlert for example

            }

        }

        public override void OnResignActivation(UIApplication application)
        {
            // Invoked when the application is about to move from active to inactive state.
            // This can occur for certain types of temporary interruptions (such as an incoming phone call or SMS message) 
            // or when the user quits the application and it begins the transition to the background state.
            // Games should use this method to pause the game.
        }

        public override void DidEnterBackground(UIApplication application)
        {
            // Use this method to release shared resources, save user data, invalidate timers and store the application state.
            // If your application supports background exection this method is called instead of WillTerminate when the user quits.
        }

        public override void WillEnterForeground(UIApplication application)
        {
            // Called as part of the transiton from background to active state.
            // Here you can undo many of the changes made on entering the background.
        }

        public override void OnActivated(UIApplication application)
        {
            // Restart any tasks that were paused (or not yet started) while the application was inactive. 
            // If the application was previously in the background, optionally refresh the user interface.
        }

        public override void WillTerminate(UIApplication application)
        {
            // Called when the application is about to terminate. Save data, if needed. See also DidEnterBackground.
        }
    }
}

