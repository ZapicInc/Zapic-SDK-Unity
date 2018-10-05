#import "ZPCBanner.h"
#import "ZPCGradientBar.h"
#import "ZPCLog.h"
#import "ZPCUtils.h"

@interface ZPCBanner ()
@property (nonatomic, assign) float topPadding;
@property (nonatomic, strong) NSLayoutConstraint *showingConstraint;
@property (nonatomic, strong) NSLayoutConstraint *hiddenConstraint;
@end

@implementation ZPCBanner

static CGFloat const bannerHeight = 66;
static CGFloat const animationDuration = 0.4;

- (instancetype)initWithTitle:(NSString *)title subtitle:(NSString *)subtitle image:(UIImage *)image {
    if (self = [super initWithFrame:CGRectZero]) {
        _topPadding = [ZPCUtils notchSize];

        if (![UIApplication sharedApplication].isStatusBarHidden) {
            _topPadding = [UIApplication sharedApplication].statusBarFrame.size.height;
        }

        self.backgroundColor = UIColor.whiteColor;
        self.translatesAutoresizingMaskIntoConstraints = NO;

        float bannerWidth = MIN(375, MIN(UIScreen.mainScreen.bounds.size.width, UIScreen.mainScreen.bounds.size.height));
        [self.widthAnchor constraintEqualToConstant:bannerWidth].active = YES;
        [self.heightAnchor constraintEqualToConstant:bannerHeight].active = YES;

        //Add the gradient to the top
        ZPCGradientBar *gradient = [[ZPCGradientBar alloc] init];
        [self addSubview:gradient];

        //Adds the icon
        UIImageView *iconView = [[UIImageView alloc] initWithImage:image];
        iconView.translatesAutoresizingMaskIntoConstraints = NO;
        iconView.contentMode = UIViewContentModeScaleAspectFit;
        iconView.layer.cornerRadius = 5;
        iconView.layer.masksToBounds = YES;

        [self addSubview:iconView];

        CGFloat const iconPadding = 8;
        [iconView.topAnchor constraintEqualToAnchor:gradient.bottomAnchor constant:iconPadding].active = YES;
        [iconView.leadingAnchor constraintEqualToAnchor:self.leadingAnchor constant:iconPadding].active = YES;
        [iconView.bottomAnchor constraintEqualToAnchor:self.bottomAnchor constant:-iconPadding].active = YES;
        [iconView.widthAnchor constraintEqualToAnchor:iconView.heightAnchor].active = YES;

        //Add the content container
        UIView *content = [[UIView alloc] init];
        content.translatesAutoresizingMaskIntoConstraints = NO;

        [self addSubview:content];

        static CGFloat const contentPadding = 5;
        [content.topAnchor constraintEqualToAnchor:gradient.bottomAnchor constant:contentPadding].active = YES;
        [content.bottomAnchor constraintEqualToAnchor:self.bottomAnchor constant:-contentPadding].active = YES;
        [content.rightAnchor constraintEqualToAnchor:self.rightAnchor constant:-contentPadding].active = YES;
        [content.leftAnchor constraintEqualToAnchor:iconView.rightAnchor constant:contentPadding].active = YES;

        static CGFloat const subtitlePadding = 10;

        //Add the title
        UILabel *titleLabel = [[UILabel alloc] init];
        titleLabel.font = [UIFont systemFontOfSize:16];
        titleLabel.text = title;
        titleLabel.numberOfLines = 1;
        titleLabel.textAlignment = NSTextAlignmentCenter;

        [content addSubview:titleLabel];

        titleLabel.translatesAutoresizingMaskIntoConstraints = NO;
        [titleLabel.centerXAnchor constraintEqualToAnchor:content.centerXAnchor].active = YES;
        [titleLabel.centerYAnchor constraintEqualToAnchor:content.centerYAnchor constant:subtitle ? -subtitlePadding : 0].active = YES;
        [titleLabel.widthAnchor constraintEqualToAnchor:content.widthAnchor].active = YES;

        if (subtitle) {
            UILabel *subtitleLabel = [[UILabel alloc] init];
            subtitleLabel.font = [UIFont systemFontOfSize:14];
            subtitleLabel.textColor = [UIColor.blackColor colorWithAlphaComponent:0.5];
            subtitleLabel.text = subtitle;
            subtitleLabel.numberOfLines = 1;
            subtitleLabel.textAlignment = NSTextAlignmentCenter;

            [content addSubview:subtitleLabel];

            subtitleLabel.translatesAutoresizingMaskIntoConstraints = NO;
            [subtitleLabel.centerXAnchor constraintEqualToAnchor:content.centerXAnchor].active = YES;
            [subtitleLabel.centerYAnchor constraintEqualToAnchor:content.centerYAnchor constant:subtitlePadding].active = YES;
            [subtitleLabel.widthAnchor constraintEqualToAnchor:content.widthAnchor].active = YES;
        }

        //Adds the shadown to the banner
        self.layer.shadowColor = UIColor.blackColor.CGColor;
        self.layer.shadowOpacity = 0.5;
        self.layer.shadowOffset = CGSizeZero;
        self.layer.shadowRadius = 4;

        [self addGestures];
    }
    return self;
}

- (void)didMoveToSuperview {
    [super didMoveToSuperview];

    if (!self.superview) {
        return;
    }

    [self.centerXAnchor constraintEqualToAnchor:self.superview.centerXAnchor].active = YES;

    _showingConstraint = [self.topAnchor constraintEqualToAnchor:self.superview.topAnchor constant:_topPadding];

    // Offset the bottom constraint to make room for the shadow to animate off screen.
    _hiddenConstraint = [self.bottomAnchor constraintEqualToAnchor:self.superview.topAnchor constant:-7.0];
}

- (void)show:(NSTimeInterval)duration {
    //Finds the current view controller being shown
    UIView *topView = [ZPCUtils getTopView];

    //Adds the banner to that view
    [topView addSubview:self];

    [self setVisibility:NO];

    dispatch_async(dispatch_get_main_queue(), ^{
        [UIView animateWithDuration:animationDuration
            delay:0.0
            usingSpringWithDamping:0.7
            initialSpringVelocity:1.5
            options:UIViewAnimationOptionAllowUserInteraction
            animations:^{
                [self setVisibility:YES];
            }
            completion:^(BOOL finished) {
                dispatch_after(dispatch_time(DISPATCH_TIME_NOW, (int64_t)(duration * NSEC_PER_SEC)), dispatch_get_main_queue(), ^{
                    [self dismiss];
                });
            }];
    });
}

- (void)addGestures {
    //Tap gesture
    [self addGestureRecognizer:[[UITapGestureRecognizer alloc] initWithTarget:self action:@selector(didTap:)]];

    //Swipe gesture
    UISwipeGestureRecognizer *swipe = [[UISwipeGestureRecognizer alloc] initWithTarget:self action:@selector(didSwipe:)];
    swipe.direction = UISwipeGestureRecognizerDirectionUp;
    [self addGestureRecognizer:swipe];
}

- (void)didTap:(UITapGestureRecognizer *)rec {
    [self dismiss];

    if (self.callback) {
        self.callback();
    }
}

- (void)didSwipe:(UISwipeGestureRecognizer *)rec {
    [self dismiss];
}

- (void)dismiss {
    [UIView animateWithDuration:animationDuration
        delay:0.0
        usingSpringWithDamping:0.7
        initialSpringVelocity:1.5
        options:UIViewAnimationOptionAllowUserInteraction
        animations:^{
            [self setVisibility:NO];
        }
        completion:^(BOOL finished) {
            [self removeFromSuperview];
        }];
}

- (void)setVisibility:(BOOL)show {
    if (show) {
        [self.superview removeConstraint:_hiddenConstraint];
        [self.superview addConstraint:_showingConstraint];
    } else {
        [self.superview removeConstraint:_showingConstraint];
        [self.superview addConstraint:_hiddenConstraint];
    }

    [self setNeedsLayout];
    [self setNeedsUpdateConstraints];

    // Managing different -layoutIfNeeded behaviours among iOS versions (for more, read the UIKit iOS 10 release notes)
    if (@available(iOS 10.0, *)) {
        [self.superview layoutIfNeeded];
    } else {
        [self layoutIfNeeded];
    }
    [self updateConstraintsIfNeeded];
}

@end
