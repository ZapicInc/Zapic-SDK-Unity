#import "ZPCUtils.h"
#import "ZPCLog.h"

@implementation ZPCUtils

+ (UIViewController *)getTopViewController {
    return UIApplication.sharedApplication.delegate.window.rootViewController;
}

+ (UIView *)getTopView {
    UIViewController *root = [self getTopViewController];

    if (root.presentedViewController) {
        return root.presentedViewController.view;
    }

    return root.view;
}

+ (NSDateFormatter *)getIsoFormatter {
    NSDateFormatter *dateFormatter = [[NSDateFormatter alloc] init];
    NSLocale *enUSPOSIXLocale = [NSLocale localeWithLocaleIdentifier:@"en_US_POSIX"];
    [dateFormatter setLocale:enUSPOSIXLocale];
    [dateFormatter setDateFormat:@"yyyy-MM-dd'T'HH:mm:ssZZZZZ"];
    return dateFormatter;
}

+ (NSString *)getIsoNow {
    NSDate *now = [NSDate date];
    return [ZPCUtils toIsoDate:now];
}

+ (NSString *)toIsoDate:(NSDate *)date {
    NSDateFormatter *dateFormatter = [ZPCUtils getIsoFormatter];
    return [dateFormatter stringFromDate:date];
}

+ (NSDate *)parseDateIso:(NSString *)dateString {
    NSDateFormatter *dateFormatter = [ZPCUtils getIsoFormatter];
    return [dateFormatter dateFromString:dateString];
}

+ (void)cleanDictionary:(NSDictionary *)dict {
    NSMutableArray *keys = [NSMutableArray array];
    for (id key in dict) {
        id value = dict[key];

        if ([value isKindOfClass:[NSDictionary class]]) {
            [ZPCUtils cleanDictionary:value];
        } else if ([value isEqual:[NSNull null]]) {
            [keys addObject:key];
        } else if ([value isKindOfClass:[NSMutableArray class]]) {
            [ZPCUtils cleanArray:value];
        }
    }

    for (id key in keys) {
        [dict setValue:nil forKey:key];
    }
}

+ (void)cleanArray:(NSMutableArray *)array {
    for (id value in array) {
        if ([value isKindOfClass:[NSDictionary class]]) {
            [ZPCUtils cleanDictionary:value];
        } else if ([value isKindOfClass:[NSArray class]]) {
            [ZPCUtils cleanArray:value];
        }
    }
}

+ (int)notchSize {
    if (@available(iOS 11.0, *)) {
        UIWindow *window = UIApplication.sharedApplication.keyWindow;
        return window.safeAreaInsets.top;
    } else {
        return 0;
    }
}

@end
