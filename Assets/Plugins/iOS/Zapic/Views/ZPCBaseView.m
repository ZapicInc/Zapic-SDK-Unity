#import "ZPCBaseView.h"
#import "ZPCGradientBar.h"
#import "ZPCImageUtils.h"
#import "ZPCLog.h"
#import "ZPCUtils.h"

@implementation ZPCBaseView

- (instancetype)initWithSpinner {
    return [self initWithText:nil subText:nil showSpinner:YES];
}

- (instancetype)initWithText:(NSString *)text subText:(NSString *)subText showSpinner:(BOOL)showSpinner {
    if (self = [super initWithFrame:CGRectZero]) {
        //Background color
        self.backgroundColor = [UIColor.blackColor colorWithAlphaComponent:0.8];

        //Add the gradient to the top
        ZPCGradientBar *gradient = [[ZPCGradientBar alloc] init];
        [self addSubview:gradient];

        //Add the zapic logo to the center of the screen
        UIImageView *icon = [[UIImageView alloc] initWithImage:[ZPCImageUtils getZapicLogo]];
        icon.contentMode = UIViewContentModeScaleAspectFit;
        icon.translatesAutoresizingMaskIntoConstraints = NO;

        [self addSubview:icon];

        CGFloat const iconSize = 64;
        [icon.centerXAnchor constraintEqualToAnchor:self.centerXAnchor].active = YES;
        [icon.centerYAnchor constraintEqualToAnchor:self.centerYAnchor constant:-150].active = YES;
        [icon.heightAnchor constraintEqualToConstant:iconSize].active = YES;
        [icon.widthAnchor constraintEqualToConstant:iconSize].active = YES;

        NSLayoutYAxisAnchor *lastAnchor = icon.bottomAnchor;

        if (showSpinner) {
            UIActivityIndicatorView *spinner = [[UIActivityIndicatorView alloc] initWithActivityIndicatorStyle:UIActivityIndicatorViewStyleWhiteLarge];
            spinner.translatesAutoresizingMaskIntoConstraints = NO;
            [spinner startAnimating];

            [self addSubview:spinner];

            [spinner.topAnchor constraintEqualToAnchor:lastAnchor constant:20].active = YES;
            [spinner.centerXAnchor constraintEqualToAnchor:self.centerXAnchor].active = YES;

            lastAnchor = spinner.bottomAnchor;
        }

        //Adds the text
        if (text) {
            UILabel *title = [[UILabel alloc] init];
            title.font = [UIFont systemFontOfSize:22];
            title.text = text;
            title.textAlignment = NSTextAlignmentCenter;
            title.textColor = UIColor.whiteColor;
            title.translatesAutoresizingMaskIntoConstraints = NO;

            [self addSubview:title];

            [title.centerXAnchor constraintEqualToAnchor:self.centerXAnchor].active = YES;
            [title.topAnchor constraintEqualToAnchor:lastAnchor constant:15].active = YES;

            lastAnchor = title.bottomAnchor;
        }

        //Adds the subtext
        if (subText) {
            UILabel *details = [[UILabel alloc] init];
            details.font = [UIFont systemFontOfSize:18];
            details.text = subText;
            details.textAlignment = NSTextAlignmentCenter;
            details.lineBreakMode = NSLineBreakByWordWrapping;
            details.numberOfLines = 0;
            details.textColor = [UIColor.whiteColor colorWithAlphaComponent:0.5];
            details.translatesAutoresizingMaskIntoConstraints = NO;

            [self addSubview:details];

            [details.leftAnchor constraintEqualToAnchor:self.leftAnchor constant:50].active = YES;
            [details.rightAnchor constraintEqualToAnchor:self.rightAnchor constant:-50].active = YES;
            [details.topAnchor constraintEqualToAnchor:lastAnchor constant:15].active = YES;
        }

        UIButton *closeButton = [[UIButton alloc] init];
        [closeButton setTitle:@"Done" forState:UIControlStateNormal];
        [closeButton addTarget:self action:@selector(closeButtonAction:) forControlEvents:UIControlEventTouchUpInside];
        closeButton.translatesAutoresizingMaskIntoConstraints = NO;

        [self addSubview:closeButton];

        [closeButton.topAnchor constraintEqualToAnchor:gradient.bottomAnchor constant:10].active = YES;
        [closeButton.leftAnchor constraintEqualToAnchor:self.leftAnchor constant:10].active = YES;
    }
    return self;
}

- (void)closeButtonAction:(UIButton *)sender {
    if (!_viewController) {
        [ZPCLog error:@"Unable to close the view. No view controller set"];
        return;
    }

    [ZPCLog info:@"Close button tapped"];
    [_viewController closePage];
}

@end
