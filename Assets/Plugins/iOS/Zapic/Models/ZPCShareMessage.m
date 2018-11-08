#import "ZPCShareMessage.h"

@implementation ZPCShareMessage

- (instancetype)initWithText:(nullable NSString *)text
                      target:(nullable NSString *)target
                     subject:(nullable NSString *)subject
                   withImage:(nullable UIImage *)image
                     withURL:(nullable NSURL *)url {
    if (self = [super init]) {
        _text = text;
        _target = target;
        _subject = subject;
        _image = image;
        _url = url;
    }
    return self;
}
@end
