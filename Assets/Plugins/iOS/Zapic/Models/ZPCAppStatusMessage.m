#import "ZPCAppStatusMessage.h"

@implementation ZPCAppStatusMessage

- (instancetype)initWithStatus:(ZPCAppStatus)status {
    if (self = [super init]) {
        _status = status;
    }
    return self;
}
@end
