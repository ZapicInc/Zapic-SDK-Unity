#import "ZPCGradientBar.h"

@interface ZPCGradientBar ()
@property (nonatomic, strong) CAGradientLayer *gradient;
@end

@implementation ZPCGradientBar

- (instancetype)init {
    if (self = [super initWithFrame:CGRectZero]) {
        self.backgroundColor = UIColor.redColor;
        _gradient = [CAGradientLayer layer];

        _gradient.frame = self.bounds;

        UIColor *zBlue = [UIColor colorWithRed:0.00 green:0.87 blue:0.68 alpha:1.0];
        UIColor *zGreen = [UIColor colorWithRed:0.00 green:0.52 blue:0.89 alpha:1.0];
        _gradient.startPoint = CGPointMake(1, 0);
        _gradient.endPoint = CGPointZero;
        _gradient.colors = @[(id)zBlue.CGColor, (id)zGreen.CGColor];

        [self.layer insertSublayer:_gradient atIndex:0];
    }
    return self;
}

- (void)layoutSubviews {
    _gradient.frame = self.frame;
}

- (void)didMoveToSuperview {
    [super didMoveToSuperview];

    UIView *superview = self.superview;

    if (!superview) {
        return;
    }

    self.translatesAutoresizingMaskIntoConstraints = NO;
    [self.topAnchor constraintEqualToAnchor:superview.topAnchor].active = YES;
    [self.heightAnchor constraintEqualToConstant:6].active = YES;
    [self.leadingAnchor constraintEqualToAnchor:superview.leadingAnchor].active = YES;
    [self.trailingAnchor constraintEqualToAnchor:superview.trailingAnchor].active = YES;
}

@end
