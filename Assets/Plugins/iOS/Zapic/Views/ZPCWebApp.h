#import <UIKit/UIKit.h>
#import <WebKit/WebKit.h>
#import "ZPCSafariManager.h"
#import "ZPCScriptMessageHandler.h"

@interface ZPCWebApp : UIView <UIScrollViewDelegate, WKNavigationDelegate>
@property ZPCSafariManager *safariManager;
@property (readonly) BOOL errorLoading;
/**
 The handler when a player logs in to Zapic.
 */
@property (nonatomic, copy, nullable) void (^loadErrorHandler)(void);
- (instancetype)initWithHandler:(nonnull ZPCScriptMessageHandler *)messageHandler;
- (void)loadUrl:(NSString *)url;
- (void)evaluateJavaScript:(NSString *)jsString;
@end
