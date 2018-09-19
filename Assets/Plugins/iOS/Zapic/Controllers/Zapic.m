#import "Zapic.h"
#import <Foundation/Foundation.h>
#import "ZPCCore.h"
#import "ZPCLog.h"
#import "ZPCSelectorHelpers.h"

static ZPCCore *_core;
static void (^_loginHandler)(ZPCPlayer *);
static void (^_logoutHandler)(ZPCPlayer *);

@implementation Zapic : NSObject

#pragma mark - Page names

NSString *const ZPCPageChallenges = @"challenges";
NSString *const ZPCPageCompetition = @"competition";
NSString *const ZPCPageCreateChallenge = @"createChallenge";
NSString *const ZPCPageLogin = @"login";
NSString *const ZPCPageProfile = @"profile";
NSString *const ZPCPageStats = @"stats";

#pragma mark - Login callbacks

+ (void (^)(ZPCPlayer *))loginHandler {
    return _loginHandler;
}

+ (void (^)(ZPCPlayer *))logoutHandler {
    return _logoutHandler;
}

+ (void)setLoginHandler:(void (^)(ZPCPlayer *))loginHandler {
    _loginHandler = loginHandler;
}

+ (void)setLogoutHandler:(void (^)(ZPCPlayer *))logoutHandler {
    _logoutHandler = logoutHandler;
}

+ (void)initialize {
    if (self == [Zapic self]) {
        _core = [[ZPCCore alloc] init];

        [_core.playerManager addLoginHandler:^(ZPCPlayer *player) {
            if (_loginHandler) {
                _loginHandler(player);
            }
        }];

        [_core.playerManager addLogoutHandler:^(ZPCPlayer *player) {
            if (_logoutHandler) {
                _logoutHandler(player);
            }
        }];
    }
}

#pragma mark - Zapic Methods

+ (void)start {
    [_core start];
}

+ (void)showPage:(NSString *)pageName {
    [_core showPage:pageName];
}

+ (void)showDefaultPage {
    [_core showDefaultPage];
}

+ (void)handleInteractionData:(NSDictionary<NSString *, NSString *> *)data {
    if (!data) {
        [ZPCLog warn:@"Missing data, unable to handleInteraction"];
        return;
    }

    NSString *zapic = [data objectForKey:@"zapic"];

    [self handleInteraction:zapic];
}

+ (void)handleInteraction:(NSString *)string {
    if (!string) {
        [ZPCLog warn:@"Interaction string must be valid string"];
        return;
    }

    [_core submitEvent:ZPCEventTypeInteraction withPayload:string];
}

+ (void)submitEvent:(NSDictionary *)parameters {
    [_core submitEvent:ZPCEventTypeGameplay withPayload:parameters];
}

#pragma mark - Data Queries

+ (void)getPlayer:(void (^)(ZPCPlayer *, NSError *))completionHandler {
    [_core.queryManager getPlayer:completionHandler];
}

+ (void)getCompetitions:(void (^)(NSArray<ZPCCompetition *> *competitions, NSError *error))completionHandler {
    [_core.queryManager getCompetitions:completionHandler];
}

+ (void)getStatistics:(void (^)(NSArray<ZPCStatistic *> *statistics, NSError *error))completionHandler {
    [_core.queryManager getStatistics:completionHandler];
}

+ (void)getChallenges:(void (^)(NSArray<ZPCChallenge *> *challenges, NSError *error))completionHandler {
    [_core.queryManager getChallenges:completionHandler];
}

@end
