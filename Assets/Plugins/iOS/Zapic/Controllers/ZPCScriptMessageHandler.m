#import "ZPCScriptMessageHandler.h"
#import "ZPCLog.h"
#import "ZPCUtils.h"

@interface ZPCScriptMessageHandler ()

@property (nonatomic, strong) NSMutableArray<void (^)(ZPCBannerMessage *)> *bannerHandlers;
@property (nonatomic, strong) NSMutableArray<void (^)(ZPCAppStatusMessage *)> *statusHandlers;
@property (nonatomic, strong) NSMutableArray<void (^)(ZPCPlayer *)> *loginHandlers;
@property (nonatomic, strong) NSMutableArray<void (^)(void)> *logoutHandlers;
@property (nonatomic, strong) NSMutableArray<void (^)(void)> *closePageHandlers;
@property (nonatomic, strong) NSMutableArray<void (^)(void)> *pageReadyHandlers;
@property (nonatomic, strong) NSMutableArray<void (^)(void)> *showPageHandlers;
@property (nonatomic, strong) NSMutableArray<void (^)(ZPCShareMessage *)> *showShareHandlers;
@property (nonatomic, strong) NSMutableArray<void (^)(NSDictionary *)> *queryResponseHandlers;
@end

@implementation ZPCScriptMessageHandler

static NSString *const AppStarted = @"APP_STARTED";
static NSString *const AppFailed = @"APP_FAILED";
static NSString *const ShowBanner = @"SHOW_BANNER";
static NSString *const ShowPage = @"SHOW_PAGE";
static NSString *const ShowShare = @"SHOW_SHARE_MENU";
static NSString *const PageReady = @"PAGE_READY";
static NSString *const ClosePageRequest = @"CLOSE_PAGE_REQUESTED";
static NSString *const LoggedIn = @"LOGGED_IN";
static NSString *const LoggedOut = @"LOGGED_OUT";
static NSString *const QueryResponse = @"QUERY_RESPONSE";

- (instancetype)init {
    if (self = [super init]) {
        _bannerHandlers = [[NSMutableArray<void (^)(ZPCBannerMessage *)> alloc] init];
        _statusHandlers = [[NSMutableArray<void (^)(ZPCAppStatusMessage *)> alloc] init];
        _loginHandlers = [[NSMutableArray<void (^)(ZPCPlayer *)> alloc] init];
        _logoutHandlers = [[NSMutableArray<void (^)(void)> alloc] init];
        _closePageHandlers = [[NSMutableArray<void (^)(void)> alloc] init];
        _pageReadyHandlers = [[NSMutableArray<void (^)(void)> alloc] init];
        _showPageHandlers = [[NSMutableArray<void (^)(void)> alloc] init];
        _showShareHandlers = [[NSMutableArray<void (^)(ZPCShareMessage *)> alloc] init];
        _queryResponseHandlers = [[NSMutableArray<void (^)(NSDictionary *)> alloc] init];
    }
    return self;
}

- (void)userContentController:(id)userContentController didReceiveScriptMessage:(id)message {
    NSString *name = [message valueForKey:@"name"];
    if (![name isEqualToString:ZPCScriptMethodName]) {
        [ZPCLog warn:@"Received unknown method from JS"];
        return;
    }

    NSDictionary *json = [message valueForKey:@"body"];

    if (json == nil || ![json isKindOfClass:[NSDictionary class]]) {
        [ZPCLog warn:@"Received invalid message format"];
        return;
    }

    NSString *type = [[json valueForKey:@"type"] uppercaseString];

    if (!type.length) {
        [ZPCLog warn:@"Received a message with a missing message type"];
        return;
    }

    [ZPCUtils cleanDictionary:json];

    [self handleMessage:type withData:json];
}

- (void)addAppStatusHandler:(void (^)(ZPCAppStatusMessage *))handler {
    [_statusHandlers addObject:handler];
}

- (void)addBannerHandler:(void (^)(ZPCBannerMessage *))handler {
    [_bannerHandlers addObject:handler];
}

- (void)addClosePageHandler:(void (^)(void))handler {
    [_closePageHandlers addObject:handler];
}

- (void)addPageReadyHandler:(void (^)(void))handler {
    [_pageReadyHandlers addObject:handler];
}

- (void)addLoginHandler:(void (^)(ZPCPlayer *))handler {
    [_loginHandlers addObject:handler];
}

- (void)addLogoutHandler:(void (^)(void))handler {
    [_logoutHandlers addObject:handler];
}

- (void)addShowPageHandler:(void (^)(void))handler {
    [_showPageHandlers addObject:handler];
}

- (void)addShowShareHandler:(void (^)(ZPCShareMessage *))handler {
    [_showShareHandlers addObject:handler];
}

- (void)addQueryResponseHandler:(void (^)(NSDictionary *))handler {
    [_queryResponseHandlers addObject:handler];
}

- (void)handleMessage:(nonnull NSString *)type
             withData:(nonnull NSDictionary *)data {
    [ZPCLog info:@"Received %@ from JS", type];

    if ([type isEqualToString:AppStarted]) {
        [self handleStatusUpdated:ZPCAppStatusReady];
    } else if ([type isEqualToString:AppFailed]) {
        [self handleStatusUpdated:ZPCAppStatusFailed];
    } else if ([type isEqualToString:ClosePageRequest]) {
        [self handleClosePageRequested];
    } else if ([type isEqualToString:LoggedIn]) {
        [self handleLogin:data];
    } else if ([type isEqualToString:LoggedOut]) {
        [self handleLogout];
    } else if ([type isEqualToString:PageReady]) {
        [self handlePageReady];
    } else if ([type isEqualToString:ShowBanner]) {
        [self handleBanner:data];
    } else if ([type isEqualToString:ShowPage]) {
        [self handleShowPage];
    } else if ([type isEqualToString:ShowShare]) {
        [self handleShowShare:data];
    } else if ([type isEqualToString:QueryResponse]) {
        [self handleQueryResponse:data];
    } else {
        [ZPCLog info:@"Recevied unhandled message type: %@", type];
    }
}

- (void)handleBanner:(nonnull NSDictionary *)data {
    NSDictionary *msg = data[@"payload"];
    NSString *title = msg[@"title"];
    NSString *subtitle = msg[@"subtitle"];
    NSString *metadata = msg[@"data"];
    UIImage *img = [self decodeBase64ToImage:msg[@"icon"]];

    ZPCBannerMessage *bannerMessage = [ZPCBannerMessage bannerWithTitle:title withSubtitle:subtitle withData:metadata withIcon:img];

    for (id (^handler)(ZPCBannerMessage *) in _bannerHandlers) {
        handler(bannerMessage);
    }
}

- (void)handleShowPage {
    for (id (^handler)(void) in _showPageHandlers) {
        handler();
    }
}

- (void)handleShowShare:(nonnull NSDictionary *)data {
    NSDictionary *msg = data[@"payload"];
    NSString *text = msg[@"text"];
    NSString *urlStr = msg[@"url"];
    NSString *imgStr = msg[@"image"];
    NSString *target = msg[@"target"];
    NSString *subject = msg[@"subject"];

    NSURL *url;
    UIImage *img;

    if (urlStr && urlStr != (id)[NSNull null]) {
        url = [NSURL URLWithString:urlStr];
    }

    if (imgStr) {
        img = [self decodeBase64ToImage:imgStr];
    }

    if (!text) {
        text = msg[@"body"];
    }

    ZPCShareMessage *shareMsg = [[ZPCShareMessage alloc] initWithText:text target:target subject:subject withImage:img withURL:url];

    for (id (^handler)(ZPCShareMessage *) in _showShareHandlers) {
        handler(shareMsg);
    }
}

- (void)handleLogin:(nonnull NSDictionary *)data {
    NSDictionary *msg = data[@"payload"];
    ZPCPlayer *player = [[ZPCPlayer alloc] initWithData:msg];

    for (id (^handler)(ZPCPlayer *) in _loginHandlers) {
        handler(player);
    }
}

- (void)handleLogout {
    for (id (^handler)(void) in _logoutHandlers) {
        handler();
    }
}

- (void)handleStatusUpdated:(ZPCAppStatus)status {
    ZPCAppStatusMessage *statusMessage = [[ZPCAppStatusMessage alloc] initWithStatus:status];

    for (id (^handler)(ZPCAppStatusMessage *) in _statusHandlers) {
        handler(statusMessage);
    }
}

- (void)handleClosePageRequested {
    for (id (^handler)(void) in _closePageHandlers) {
        handler();
    }
}

- (void)handlePageReady {
    for (id (^handler)(void) in _pageReadyHandlers) {
        handler();
    }
}

- (void)handleQueryResponse:(nonnull NSDictionary *)data {
    for (id (^handler)(NSDictionary *) in _queryResponseHandlers) {
        handler(data);
    }
}

- (UIImage *)decodeBase64ToImage:(NSString *)strEncodeData {
    if (!strEncodeData.length) {
        return nil;
    }
    NSData *data = [[NSData alloc] initWithBase64EncodedString:strEncodeData options:NSDataBase64DecodingIgnoreUnknownCharacters];
    return [UIImage imageWithData:data];
}

@end
