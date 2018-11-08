#import <Foundation/Foundation.h>
#import <WebKit/WebKit.h>
#import "ZPCAppStatusMessage.h"
#import "ZPCBannerMessage.h"
#import "ZPCPlayer.h"
#import "ZPCShareMessage.h"

static NSString *const ZPCScriptMethodName = @"dispatch";

@interface ZPCScriptMessageHandler : NSObject <WKScriptMessageHandler>
- (void)userContentController:(id)userContentController didReceiveScriptMessage:(id)message;
- (void)addAppStatusHandler:(void (^)(ZPCAppStatusMessage *))handler;
- (void)addBannerHandler:(void (^)(ZPCBannerMessage *))handler;
- (void)addLoginHandler:(void (^)(ZPCPlayer *))handler;
- (void)addLogoutHandler:(void (^)(void))handler;
- (void)addClosePageHandler:(void (^)(void))handler;
- (void)addPageReadyHandler:(void (^)(void))handler;
- (void)addShowPageHandler:(void (^)(void))handler;
- (void)addShowShareHandler:(void (^)(ZPCShareMessage *))handler;
- (void)addQueryResponseHandler:(void (^)(NSDictionary *))handler;
@end
