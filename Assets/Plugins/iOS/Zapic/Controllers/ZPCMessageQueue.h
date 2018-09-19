#import <Foundation/Foundation.h>
#import "ZPCWebApp.h"

static NSString *const ZPCWebFunctionSubmitEvent = @"SUBMIT_EVENT";
static NSString *const ZPCWebFunctionOpenPage = @"OPEN_PAGE";
static NSString *const ZPCWebFunctionClosePage = @"CLOSE_PAGE";
static NSString *const ZPCWebFunctionSetDeviceToken = @"DEVICE_TOKEN";
static NSString *const ZPCWebFunctionNotificationOpened = @"NOTIFICATION_OPENED";
static NSString *const ZPCWebFunctionNotificationReceived = @"NOTIFICATION_RECEIVED";
static NSString *const ZPCWebFunctionQuery = @"QUERY";

@interface ZPCMessageQueue : NSObject
@property (nonatomic, strong) ZPCWebApp *webApp;
- (void)sendMessage:(NSString *)function withPayload:(NSObject *)payload;
- (void)sendMessage:(NSString *)function withPayload:(NSObject *)payload isError:(BOOL)isError;
- (void)sendQueuedMessages;
@end
