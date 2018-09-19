#import <Foundation/Foundation.h>
#import <UIKit/UIKit.h>

@interface ZPCBanner : UIView

@property (nonatomic, copy) void (^callback)(void);

- (instancetype)initWithTitle:(NSString *)title subtitle:(NSString *)subtitle image:(UIImage *)image;
- (void)show:(NSTimeInterval)duration;

@end
