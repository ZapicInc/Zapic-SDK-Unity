#import <Foundation/Foundation.h>
#import "ZPCPlayer.h"
#import "ZPCScriptMessageHandler.h"

@interface ZPCPlayerManager : NSObject
- (instancetype)initWithHandler:(ZPCScriptMessageHandler *)handler;
- (void)addLoginHandler:(void (^)(ZPCPlayer *))handler;
- (void)addLogoutHandler:(void (^)(ZPCPlayer *))handler;
@end
