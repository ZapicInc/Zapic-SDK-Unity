#import "ZPCInjectedJS.h"
#import <AdSupport/AdSupport.h>
#import <UIKit/UIKit.h>

@implementation ZPCInjectedJS

+ (NSString *)getInjectedScript {
    //Gets the info to be injected
    NSString *sdkVersion = @"1.3.0";
    NSDictionary *appInfo = NSBundle.mainBundle.infoDictionary;
    NSString *appVersion = appInfo[@"CFBundleShortVersionString"];
    NSString *appBuild = appInfo[@"CFBundleVersion"];
    NSString *bundleId = NSBundle.mainBundle.bundleIdentifier;
    NSString *deviceId = UIDevice.currentDevice.identifierForVendor.UUIDString;
    NSString *iosVersion = UIDevice.currentDevice.systemVersion;
    NSString *installId = [self installId];
    const int sdkApiVersion = 3;
    const int loadTimeout = 10000;

    NSString *adId = @"";

    if (ASIdentifierManager.sharedManager.isAdvertisingTrackingEnabled) {
        adId = ASIdentifierManager.sharedManager.advertisingIdentifier.UUIDString;
    }

    return [NSString stringWithFormat:
                         @"window.iosWebViewWatchdog = window.setTimeout(function () {"
                         @"  window.webkit.messageHandlers.dispatch.postMessage({\"type\":\"APP_FAILED\"});"
                          "}, %d);"
                          "window.zapic = {"
                          "environment: 'webview',"
                          "version: %d,"
                          "iosVersion: '%@',"
                          "bundleId: '%@',"
                          "sdkVersion: '%@',"
                          "installId: '%@',"
                          "deviceId: '%@',"
                          "appVersion: '%@',"
                          "appBuild: '%@',"
                          "adId:'%@',"
                          "onLoaded: function(action$, publishAction) {"
                          "window.clearTimeout(window.iosWebViewWatchdog);"
                          "delete window.iosWebViewWatchdog;"
                          "window.zapic.dispatch = function(action) {"
                          "publishAction(action)"
                          "};"
                          "action$.subscribe(function(action) {"
                          "window.webkit.messageHandlers.dispatch.postMessage(action)"
                          "});"
                          "}"
                          "}",
                         loadTimeout,
                         sdkApiVersion,
                         iosVersion,
                         bundleId,
                         sdkVersion,
                         installId,
                         deviceId,
                         appBuild,
                         appVersion,
                         adId];
}

+ (NSString *)installId {
    NSString *const installKey = @"zapic-install-id";
    NSUserDefaults *userDefaults = [NSUserDefaults standardUserDefaults];
    NSString *installId = [userDefaults stringForKey:installKey];

    //If an install id was not found, create one
    if (!installId) {
        installId = [NSUUID UUID].UUIDString;
        [userDefaults setObject:installId forKey:installKey];
    }

    return installId;
}

@end
