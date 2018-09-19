#import "ZPCWebApp.h"
#import "ZPCInjectedJS.h"
#import "ZPCLog.h"
#import "ZPCUtils.h"

@interface ZPCWebApp ()

@property (nonatomic, strong) NSString *appUrl;
@property (nonatomic, assign) bool loadSuccessful;
@property (nonatomic, assign) int retryAttempt;
@property (readonly) WKWebView *webView;

@end

@implementation ZPCWebApp

- (instancetype)initWithHandler:(nonnull ZPCScriptMessageHandler *)messageHandler {
    if (self = [super initWithFrame:CGRectZero]) {
        WKWebViewConfiguration *config = [ZPCWebApp getWebViewConfiguration];
        [config.userContentController addScriptMessageHandler:messageHandler name:ZPCScriptMethodName];

        _webView = [[WKWebView alloc] initWithFrame:CGRectZero configuration:config];

        _webView.translatesAutoresizingMaskIntoConstraints = NO;
        _webView.autoresizingMask = (UIViewAutoresizingFlexibleWidth | UIViewAutoresizingFlexibleHeight);
        _webView.scrollView.scrollEnabled = NO;
        _webView.scrollView.bounces = NO;
        _webView.scrollView.delegate = self;
        _webView.navigationDelegate = self;

        [self addSubview:_webView];
        [_webView.topAnchor constraintEqualToAnchor:self.topAnchor].active = YES;
        [_webView.leftAnchor constraintEqualToAnchor:self.leftAnchor].active = YES;
        [_webView.rightAnchor constraintEqualToAnchor:self.rightAnchor].active = YES;
        [_webView.bottomAnchor constraintEqualToAnchor:self.bottomAnchor].active = YES;
    }
    return self;
}

- (void)evaluateJavaScript:(NSString *)jsString {
    dispatch_async(dispatch_get_main_queue(), ^{
        [ZPCLog info:@"Dispatching %@", jsString];

        if (!self.webView) {
            [ZPCLog error:@"Webview has not been set, unable to send"];
            return;
        }

        [self.webView evaluateJavaScript:jsString
                       completionHandler:^(id _Nullable result, NSError *_Nullable error) {
                           if (error) {
                               [ZPCLog error:@"JS Error: %@", error];
                           } else if (result) {
                               [ZPCLog info:@"JS Result: %@", result];
                           }
                       }];
    });
}

- (void)loadUrl:(NSString *)url {
    _appUrl = url;
    [self load];
}

- (void)load {
    NSURLRequest *appRequest = [NSURLRequest requestWithURL:[NSURL URLWithString:_appUrl]];

    [_webView loadRequest:appRequest];
}

+ (WKWebViewConfiguration *)getWebViewConfiguration {
    WKWebViewConfiguration *config = [[WKWebViewConfiguration alloc] init];

    //Gets the JS code to be injected
    NSString *injected = [ZPCInjectedJS getInjectedScript];

    WKUserScript *script = [[WKUserScript alloc] initWithSource:injected injectionTime:WKUserScriptInjectionTimeAtDocumentStart forMainFrameOnly:YES];

    [config.userContentController addUserScript:script];

    return config;
}

- (void)retryAfterDelay {
    if (_loadSuccessful) {
        return;
    }

    _retryAttempt += 1;

    static CGFloat const base = 5;
    static CGFloat const maxDelay = 300; //5 minutes

    //Calculate the delay before the next retry
    float delay = MAX(1, drand48() * MIN(maxDelay, base * pow(2.0, _retryAttempt)));

    [ZPCLog info:@"Will try to reload in %f sec", delay];

    dispatch_after(dispatch_time(DISPATCH_TIME_NOW, (int64_t)(delay * NSEC_PER_SEC)), dispatch_get_main_queue(), ^{
        [self load];
    });
}

#pragma mark - WKNavigationDelegate

- (void)webView:(WKWebView *)webView didFinishNavigation:(WKNavigation *)navigation {
    [ZPCLog info:@"Finished loading web app"];
    _retryAttempt = 0;
    _loadSuccessful = YES;
    _errorLoading = NO;
}

/**
   Handles any errors loading the web view
   
   @param webView The web view invoking the delegate method.
   @param navigation The navigation object that started to load a page.
   @param error The error that occurred.
   */
- (void)webView:(WKWebView *)webView didFailProvisionalNavigation:(WKNavigation *)navigation withError:(NSError *)error {
    if ([error.domain isEqual:@"WebKitErrorDomain"] && error.code == 102) {
        [ZPCLog info:@"Skipping known error message loading url"];
        return;
    }

    [ZPCLog warn:@"Error loading Zapic webview"];

    [self retryAfterDelay];

    _errorLoading = YES;

    if (_loadErrorHandler) {
        _loadErrorHandler();
    }
}

- (void)webView:(WKWebView *)webView decidePolicyForNavigationAction:(WKNavigationAction *)navigationAction decisionHandler:(void (^)(WKNavigationActionPolicy))decisionHandler {
    NSURL *url = [navigationAction valueForKeyPath:@"request.URL"];

    int const cancel = 0;
    int const allow = 1;

    //Skip if there is not a valid url
    if (!url) {
        decisionHandler(cancel);
        return;
    }

    //Skip if the app has not loaded yet
    if (!_appUrl) {
        decisionHandler(cancel);
        return;
    }

    //Allow the webview to open other links that are within our web app.
    if ([url.absoluteString hasPrefix:_appUrl]) {
        decisionHandler(allow);
        return;
    }
    NSString *scheme = url.scheme;

    if (!scheme) {
        decisionHandler(cancel);
        return;
    }

    //Allow the OS to open the itms links directly into the app store
    if ([scheme hasPrefix:@"itms"]) {
        [UIApplication.sharedApplication openURL:url];
        decisionHandler(cancel);
        return;
    }

    //Opens the a safari view with the content
    [_safariManager openUrl:url];

    //Tell the webview not to open the link
    decisionHandler(cancel);
}

#pragma mark - UIScrollViewDelegate

- (void)scrollViewWillBeginZooming:(UIScrollView *)scrollView withView:(UIView *)view {
    scrollView.pinchGestureRecognizer.enabled = NO;
    scrollView.panGestureRecognizer.enabled = NO;
    [scrollView setZoomScale:1.0 animated:NO];
}

- (void)scrollViewDidZoom:(UIScrollView *)scrollView {
    if (scrollView.zoomScale == 1) {
        return;
    }
    [scrollView setZoomScale:1.0 animated:NO];
}
@end
