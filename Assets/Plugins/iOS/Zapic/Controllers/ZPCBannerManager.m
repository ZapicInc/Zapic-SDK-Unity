#import "ZPCBannerManager.h"
#import "ZPCBanner.h"
#import "ZPCImageUtils.h"
#import "ZPCUtils.h"
#import "Zapic.h"

@implementation ZPCBannerManager

- (void)setMessageHandler:(ZPCScriptMessageHandler *)messageHandler {
    [messageHandler addBannerHandler:^(ZPCBannerMessage *message) {
        UIImage *icon = message.icon;

        if (!icon) {
            icon = [ZPCImageUtils getZapicLogo];
        }

        ZPCBanner *banner = [[ZPCBanner alloc] initWithTitle:message.title subtitle:message.subtitle image:icon];

        if (message.data) {
            banner.callback = ^{
                [Zapic handleInteraction:message.data];
            };
        }

        [banner show:3.0];
    }];
}

+ (UIImage *)imageWithImage:(UIImage *)image scaledToSize:(CGSize)newSize {
    //UIGraphicsBeginImageContext(newSize);
    // In next line, pass 0.0 to use the current device's pixel scaling factor (and thus account for Retina resolution).
    // Pass 1.0 to force exact pixel size.
    UIGraphicsBeginImageContextWithOptions(newSize, NO, 0.0);
    [image drawInRect:CGRectMake(0, 0, newSize.width, newSize.height)];
    UIImage *newImage = UIGraphicsGetImageFromCurrentImageContext();
    UIGraphicsEndImageContext();
    return newImage;
}

@end
