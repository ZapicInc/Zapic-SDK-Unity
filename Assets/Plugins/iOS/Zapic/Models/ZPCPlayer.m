#import "ZPCPlayer.h"

@implementation ZPCPlayer

- (instancetype)initWithData:(NSDictionary *)data {
    if (self = [super init]) {
        _identifier = data[@"id"];
        _name = data[@"name"];
        _notificationToken = data[@"notificationToken"];
        _iconUrl = [NSURL URLWithString:data[@"iconUrl"]];
    }
    return self;
}

@end
