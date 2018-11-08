#import <Foundation/Foundation.h>
#import <UIKit/UIKit.h>

@interface ZPCImageUtils : NSObject
+ (UIImage *)getZapicLogo;
+ (UIImage *)decodeBase64ToImage:(NSString *)strEncodeData;
@end
