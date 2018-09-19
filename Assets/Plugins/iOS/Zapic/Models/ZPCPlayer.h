#import <Foundation/Foundation.h>

@interface ZPCPlayer : NSObject
@property (readonly) NSString *identifier;
@property (readonly) NSString *notificationToken;
@property (readonly) NSString *name;
@property (readonly) NSURL *iconUrl;
- (instancetype)initWithData:(NSDictionary *)data;
@end
