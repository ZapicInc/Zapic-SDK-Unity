#import "ZPCBackgroundView.h"
#import "ZPCNotificationManager.h"
#import "ZPCPlayerManager.h"
#import "ZPCQueryManager.h"
#import "ZPCWebApp.h"

#import <UIKit/UIKit.h>

typedef NS_ENUM(NSUInteger, ZPCEventType) {
    ZPCEventTypeGameplay,
    ZPCEventTypeInteraction,
};

@interface ZPCCore : UIViewController
@property (readonly) ZPCPlayerManager *playerManager;
@property (readonly) ZPCNotificationManager *notificationManager;
@property (readonly, strong) ZPCScriptMessageHandler *messageHandler;
@property (nonnull, readonly) ZPCQueryManager *queryManager;

- (void)start;
- (void)showPage:(NSString *)pageName;
- (void)showDefaultPage;
- (void)submitEvent:(ZPCEventType)eventType withPayload:(NSObject *)payload;
- (void)closePage;
@end
