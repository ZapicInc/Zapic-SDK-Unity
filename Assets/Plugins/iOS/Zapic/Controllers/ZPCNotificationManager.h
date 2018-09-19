#import <Foundation/Foundation.h>
#import "ZPCMessageQueue.h"

@interface ZPCNotificationManager : NSObject
- (instancetype)initWithMessageQueue:(ZPCMessageQueue *)messageQueue;
- (void)registerForPushNotifications;
- (void)setDeviceToken:(NSData *)deviceToken;
- (void)receivedNotification:(NSDictionary *)userInfo;
@end
