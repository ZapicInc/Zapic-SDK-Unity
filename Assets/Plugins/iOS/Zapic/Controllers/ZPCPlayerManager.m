#import "ZPCPlayerManager.h"

@interface ZPCPlayerManager ()
@property (nonatomic, strong) NSMutableArray<void (^)(ZPCPlayer *)> *loginHandlers;
@property (nonatomic, strong) NSMutableArray<void (^)(ZPCPlayer *)> *logoutHandlers;
@end

@implementation ZPCPlayerManager

- (instancetype)initWithHandler:(ZPCScriptMessageHandler *)handler {
    if (self = [super init]) {
        _loginHandlers = [[NSMutableArray<void (^)(ZPCPlayer *)> alloc] init];
        _logoutHandlers = [[NSMutableArray<void (^)(ZPCPlayer *)> alloc] init];

        [handler addLoginHandler:^(ZPCPlayer *player) {
            //If there is already a player logged in, log them out
            if (self->_player) {
                [self playerLoggedOut];
            }

            self->_player = player;
            [self playerLoggedIn:player];
        }];

        [handler addLogoutHandler:^{
            [self playerLoggedOut];
        }];
    }
    return self;
}

- (void)playerLoggedIn:(ZPCPlayer *)newPlayer {
    for (id (^handler)(ZPCPlayer *) in _loginHandlers) {
        handler(newPlayer);
    }
}

- (void)playerLoggedOut {
    if (!_player) {
        return;
    }

    for (id (^handler)(ZPCPlayer *) in _logoutHandlers) {
        handler(_player);
    }
    _player = nil;
}

- (void)addLoginHandler:(void (^)(ZPCPlayer *))handler {
    [_loginHandlers addObject:handler];
}

- (void)addLogoutHandler:(void (^)(ZPCPlayer *))handler {
    [_logoutHandlers addObject:handler];
}

@end
