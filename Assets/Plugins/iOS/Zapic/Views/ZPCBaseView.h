#import <UIKit/UIKit.h>
#import "ZPCCore.h"

@interface ZPCBaseView : UIView
@property (weak) ZPCCore *viewController;
- (instancetype)initWithSpinner;
- (instancetype)initWithText:(NSString *)text subText:(NSString *)subText showSpinner:(BOOL)showSpinner;
@end
