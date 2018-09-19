#import "ZPCSafariManager.h"
#import <SafariServices/SafariServices.h>
#import "ZPCLog.h"

@interface ZPCSafariManager ()
@property (readonly) UIViewController *viewController;
@end

@implementation ZPCSafariManager

- (instancetype)initWithController:(UIViewController *)viewController {
    if (self = [super init]) {
        _viewController = viewController;
    }
    return self;
}

- (void)openUrl:(NSURL *)url {
    if (![url.scheme isEqualToString:@"http"] && ![url.scheme isEqualToString:@"https"]) {
        [ZPCLog warn:@"Unable to open scheme %@ in Safari", url.scheme];
        return;
    }

    [ZPCLog info:@"Opening url %@ in safari", url.absoluteString];

    SFSafariViewController *svc = [[SFSafariViewController alloc] initWithURL:url];

    if (!svc) {
        [ZPCLog error:@"Unable to create SFSafariViewController"];
        return;
    }

    [svc setValue:self forKey:@"delegate"];
    [_viewController presentViewController:svc animated:YES completion:nil];
}

/**
 Callback with the embedded safari view is "Done"
 
 @param controller The controller that is done
 */
- (void)safariViewControllerDidFinish:(id)controller {
    [controller dismissViewControllerAnimated:YES completion:nil];
}

@end
