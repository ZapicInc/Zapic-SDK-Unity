#import <Foundation/Foundation.h>
#import <UIKit/UIKit.h>

@interface ZPCShareMessage : NSObject
@property (readonly, strong) NSString *text;
@property (readonly, strong) NSString *target;
@property (readonly, strong) NSString *subject;
@property (readonly, strong) NSURL *url;
@property (readonly, strong) UIImage *image;

- (instancetype)initWithText:(nullable NSString *)text
                      target:(nullable NSString *)target
                     subject:(nullable NSString *)subject
                   withImage:(nullable UIImage *)image
                     withURL:(nullable NSURL *)url;
@end
