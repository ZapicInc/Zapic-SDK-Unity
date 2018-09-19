#import "ZPCCore.h"
#import "ZPCBannerManager.h"
#import "ZPCErrorView.h"
#import "ZPCLoadingView.h"
#import "ZPCLog.h"
#import "ZPCMessageQueue.h"
#import "ZPCSafariManager.h"
#import "ZPCShareManager.h"
#import "ZPCUtils.h"

@interface ZPCCore ()
@property BOOL isVisible;
@property BOOL started;
@property BOOL pageReady;
@property (readonly) ZPCErrorView *errorView;
@property (readonly) ZPCLoadingView *loadingView;
@property (readonly, strong) ZPCBannerManager *bannerManager;
@property (readonly, strong) ZPCSafariManager *safariManager;
@property (readonly, strong) ZPCShareManager *shareManager;
@property (readonly, strong) ZPCMessageQueue *messageQueue;
@property (nonatomic, strong) ZPCBackgroundView *backgroundView;
@property (nonatomic, strong) ZPCWebApp *webApp;
@end

@implementation ZPCCore

- (instancetype)init {
    if (self = [super init]) {
        _safariManager = [[ZPCSafariManager alloc] initWithController:self];
        _shareManager = [[ZPCShareManager alloc] init];

        //Initialize the background view that will hold onto the webview
        _backgroundView = [[ZPCBackgroundView alloc] init];
        _backgroundView.backgroundColor = [UIColor clearColor];
        [_backgroundView placeViewToBackground];

        _messageHandler = [[ZPCScriptMessageHandler alloc] init];
        _messageQueue = [[ZPCMessageQueue alloc] init];

        //Update the notifications so they can send content to the web app
        _notificationManager = [[ZPCNotificationManager alloc] initWithMessageQueue:_messageQueue];

        //Initialize the loading view
        _loadingView = [[ZPCLoadingView alloc] init];
        _loadingView.viewController = self;

        //Initialize the error view
        _errorView = [[ZPCErrorView alloc] init];
        _errorView.viewController = self;

        //Initialize the web app
        _webApp = [[ZPCWebApp alloc] initWithHandler:_messageHandler];
        _webApp.safariManager = _safariManager;
        [_backgroundView addSubview:_webApp];

        _messageQueue.webApp = _webApp;

        //Initialize the query manager
        _queryManager = [[ZPCQueryManager alloc] initWithMessageHandler:_messageHandler messageQueue:_messageQueue];

        //Setup the banners
        _bannerManager = [[ZPCBannerManager alloc] init];
        _bannerManager.messageHandler = _messageHandler;

        _playerManager = [[ZPCPlayerManager alloc] initWithHandler:_messageHandler];

        __weak ZPCCore *weakSelf = self;

        [_messageHandler addAppStatusHandler:^(ZPCAppStatusMessage *msg) {
            if (msg.status == ZPCAppStatusReady) {
                [weakSelf.messageQueue sendQueuedMessages];
                self->_pageReady = YES;
            } else if (msg.status == ZPCAppStatusFailed) {
                //Notify the query manager that things are broken
                weakSelf.queryManager.isReady = NO;
            }
        }];

        [_messageHandler addClosePageHandler:^{
            [weakSelf closePage];
        }];

        [_messageHandler addPageReadyHandler:^{
            weakSelf.view = weakSelf.webApp;
        }];

        [_messageHandler addShowPageHandler:^{
            [weakSelf showPage:@"current"];
        }];

        [_messageHandler addShowShareHandler:^(ZPCShareMessage *msg) {
            [weakSelf.shareManager share:msg];
        }];

        _webApp.loadErrorHandler = ^{
            //Show the error page
            weakSelf.view = weakSelf.errorView;
            //Notify the query manager that things are broken
            weakSelf.queryManager.isReady = NO;
        };
    }
    return self;
}

- (void)start {
    if (_started) {
        [ZPCLog info:@"Zapic is already started. Start should only be called once"];
        return;
    }
    _started = true;

    [ZPCLog info:@"Starting Zapic"];

    //Start loading the Zapic web app
    [_webApp loadUrl:@"https://app.zapic.net"];
}

- (BOOL)prefersStatusBarHidden {
    //Only show the status bar on iPhoneX
    return UIScreen.mainScreen.nativeBounds.size.height != 2436;
}

- (void)closePage {
    [super dismissViewControllerAnimated:YES
                              completion:^{
                                  [self->_messageQueue sendMessage:ZPCWebFunctionClosePage withPayload:@""];
                                  [self->_backgroundView addSubview:self->_webApp];
                              }];
    _isVisible = false;
}

- (void)showDefaultPage {
    [self showPage:@"default"];
}

- (void)showPage:(NSString *)pageName {
    [ZPCLog info:@"Showing %@ page", pageName];

    if (_pageReady) {
        self.view = _webApp;
    } else if (_webApp.errorLoading) {
        self.view = _errorView;
    } else {
        self.view = _loadingView;
    }

    //Trigger the web to update
    [_messageQueue sendMessage:ZPCWebFunctionOpenPage withPayload:pageName];

    if (_isVisible) {
        [ZPCLog info:@"Zapic already visible"];
        return;
    }

    _isVisible = true;

    [self.view.topAnchor constraintEqualToAnchor:self.topLayoutGuide.topAnchor].active = YES;
    [self.view.bottomAnchor constraintEqualToAnchor:self.bottomLayoutGuide.bottomAnchor].active = YES;

    self.modalPresentationStyle = UIModalPresentationOverCurrentContext;

    UIViewController *root = [ZPCUtils getTopViewController];
    [root presentViewController:self animated:true completion:nil];
}

- (void)submitEvent:(ZPCEventType)eventType withPayload:(NSObject *)payload {
    [ZPCLog info:@"Submitting an event to the web client"];

    NSDictionary *msg = @{
        @"type": [ZPCCore getEventTypeName:eventType],
        @"params": payload,
        @"timestamp": [ZPCUtils getIsoNow],
    };

    [_messageQueue sendMessage:ZPCWebFunctionSubmitEvent withPayload:msg];
}

+ (NSString *)getEventTypeName:(ZPCEventType)eventType {
    if (eventType == ZPCEventTypeGameplay) {
        return @"gameplay";
    } else if (eventType == ZPCEventTypeInteraction) {
        return @"interaction";
    } else {
        [ZPCLog error:@"Unknow event type"];
        return @"";
    }
}

@end
