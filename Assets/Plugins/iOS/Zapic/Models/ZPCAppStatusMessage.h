#import <Foundation/Foundation.h>

typedef NS_ENUM(NSInteger, ZPCAppStatus) {
    ZPCAppStatusNone,
    ZPCAppStatusReady,
    ZPCAppStatusFailed,
};

@interface ZPCAppStatusMessage : NSObject

@property (readonly, nonatomic) ZPCAppStatus status;

- (instancetype)initWithStatus:(ZPCAppStatus)status;

@end
