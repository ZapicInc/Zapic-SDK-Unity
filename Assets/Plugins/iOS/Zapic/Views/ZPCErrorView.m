#import "ZPCErrorView.h"

@implementation ZPCErrorView

- (instancetype)init {
    if (self = [super initWithText:@"Could Not Connect" subText:@"Please check your network connection and try again" showSpinner:NO]) {
    }
    return self;
}

@end
