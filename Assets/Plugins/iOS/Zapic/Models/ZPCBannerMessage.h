#import <Foundation/Foundation.h>
#import <UIKit/UIKit.h>

@interface ZPCBannerMessage : NSObject

@property (readonly, strong) NSString *title;
@property (readonly, strong) NSString *subtitle;
@property (readonly, strong) NSString *data;
@property (readonly, strong) UIImage *icon;

- (instancetype)initWithTitle:(nonnull NSString *)title
                 withSubtitle:(nonnull NSString *)subtitle
                     withData:(nonnull NSString *)data
                     withIcon:(nonnull UIImage *)icon;

+ (instancetype)bannerWithTitle:(nonnull NSString *)title
                   withSubtitle:(nonnull NSString *)subtitle
                       withData:(nonnull NSString *)data
                       withIcon:(nonnull UIImage *)icon;
@end
