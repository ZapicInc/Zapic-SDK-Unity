#import <Foundation/Foundation.h>

@interface ZPCLog : NSObject

extern bool isEnabled;

+ (void)error:(NSString *)message, ... NS_FORMAT_FUNCTION(1, 2);
+ (void)warn:(NSString *)message, ... NS_FORMAT_FUNCTION(1, 2);
+ (void)info:(NSString *)format, ... NS_FORMAT_FUNCTION(1, 2);

@end
