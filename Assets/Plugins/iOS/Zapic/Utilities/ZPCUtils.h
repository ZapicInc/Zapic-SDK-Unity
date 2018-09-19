#import <Foundation/Foundation.h>
#import <UIKit/UIKit.h>

@interface ZPCUtils : NSObject
+ (UIViewController *)getTopViewController;
+ (UIView *)getTopView;
+ (NSString *)getIsoNow;
+ (NSString *)toIsoDate:(NSDate *)date;
+ (NSDate *)parseDateIso:(NSString *)dateString;
+ (void)cleanDictionary:(NSDictionary *)dict;
@end
