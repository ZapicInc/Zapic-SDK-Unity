#import "ZPCNotificationManager.h"
#import <UserNotifications/UserNotifications.h>
#import "ZPCLog.h"
#import "Zapic.h"

static BOOL registered = NO;
static ZPCMessageQueue *_messageQueue;

@interface ZPCNotificationManager ()
@property (readonly) ZPCMessageQueue *messageQueue;
@end

@implementation ZPCNotificationManager

- (instancetype)initWithMessageQueue:(ZPCMessageQueue *)messageQueue {
    if (self = [super init]) {
        _messageQueue = messageQueue;
    }
    return self;
}

- (void)registerForPushNotifications {
    if (registered) {
        [ZPCLog warn:@"Already registered for push notifications, ignoring"];
        return;
    }

    registered = YES;

    [ZPCLog info:@"Registering for push notifications"];

    //iOS 10 and above
    if (@available(iOS 10.0, *)) {
        id callback = ^(BOOL granted, NSError *_Nullable error) {
            //If the user accepts the push notifications
            if (granted) {
                [ZPCLog info:@"Notifications permission are granted"];

                [UNUserNotificationCenter.currentNotificationCenter getNotificationSettingsWithCompletionHandler:^(UNNotificationSettings *_Nonnull settings) {
                    [ZPCLog info:@"Notification authorization status %ld", (long)settings.authorizationStatus];

                    if (settings.authorizationStatus == UNAuthorizationStatusAuthorized) {
                        dispatch_async(dispatch_get_main_queue(), ^(void) {
                            [UIApplication.sharedApplication registerForRemoteNotifications];
                        });
                    }
                }];
            } else {
                [ZPCLog info:@"Notifications permission are not granted"];
            }
        };

        //Request permission to send push notifications, aka the popup
        [UNUserNotificationCenter.currentNotificationCenter requestAuthorizationWithOptions:(UNAuthorizationOptionAlert + UNAuthorizationOptionSound + UNAuthorizationOptionBadge) completionHandler:callback];
    } else {
        //iOS 9
        //      let settings = UIUserNotificationSettings(forTypes: [.Sound, .Alert, .Badge], categories: nil)
        //      UIApplication.sharedApplication().registerUserNotificationSettings(settings)
        UIUserNotificationSettings *settings = [UIUserNotificationSettings settingsForTypes:(UIUserNotificationTypeSound | UIUserNotificationTypeAlert | UIUserNotificationTypeBadge) categories:nil];
        [UIApplication.sharedApplication registerUserNotificationSettings:settings];

        // This is an asynchronous method to retrieve a Device Token
        // Callbacks are in AppDelegate.swift
        // Success = didRegisterForRemoteNotificationsWithDeviceToken
        // Fail = didFailToRegisterForRemoteNotificationsWithError
        //      UIApplication.sharedApplication().registerForRemoteNotifications()
        [UIApplication.sharedApplication registerForRemoteNotifications];
    }
}

- (void)setDeviceToken:(NSData *)deviceToken {
    if (!_messageQueue) {
        [ZPCLog error:@"No message queue, unable to update the device token"];
        return;
    }

    const unsigned *tokenBytes = [deviceToken bytes];

    NSString *hexToken = [NSString stringWithFormat:@"%08x%08x%08x%08x%08x%08x%08x%08x",
                                                    ntohl(tokenBytes[0]), ntohl(tokenBytes[1]), ntohl(tokenBytes[2]),
                                                    ntohl(tokenBytes[3]), ntohl(tokenBytes[4]), ntohl(tokenBytes[5]),
                                                    ntohl(tokenBytes[6]), ntohl(tokenBytes[7])];

    NSDictionary *msg = @{
        @"deviceToken": hexToken,
    };

    [_messageQueue sendMessage:ZPCWebFunctionSetDeviceToken withPayload:msg];
}

- (void)receivedNotification:(NSDictionary *)userInfo {
    UIApplicationState state = UIApplication.sharedApplication.applicationState;
    NSDictionary *aps = [userInfo objectForKey:@"aps"];

    // user tapped notification while app was in background or closed
    if (state == UIApplicationStateInactive || state == UIApplicationStateBackground) {
        [_messageQueue sendMessage:ZPCWebFunctionNotificationOpened withPayload:aps];
    } else {
        // App is in UIApplicationStateActive (running in foreground)
        [_messageQueue sendMessage:ZPCWebFunctionNotificationReceived withPayload:aps];
    }
}

@end
