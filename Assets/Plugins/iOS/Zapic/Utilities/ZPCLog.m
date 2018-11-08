#import "ZPCLog.h"

@implementation ZPCLog

+ (void)error:(NSString *)message, ... NS_FORMAT_FUNCTION(1, 2) {
    va_list args;
    va_start(args, message);

    NSString *s = [[NSString alloc] initWithFormat:message arguments:args];

    [self writeLog:s withSymbol:@"ERROR"];

    va_end(args);
}

+ (void)warn:(NSString *)message, ... NS_FORMAT_FUNCTION(1, 2) {
    va_list args;
    va_start(args, message);

    NSString *s = [[NSString alloc] initWithFormat:message arguments:args];

    [self writeLog:s withSymbol:@"WARN"];

    va_end(args);
}

+ (void)info:(NSString *)message, ... NS_FORMAT_FUNCTION(1, 2) {
    va_list args;
    va_start(args, message);

    NSString *s = [[NSString alloc] initWithFormat:message arguments:args];

    [self writeLog:s withSymbol:@"INFO"];

    va_end(args);
}

+ (void)writeLog:(NSString *)message withSymbol:(NSString *)symbol {

    NSLog(@"[Zapic][%@]-%@", symbol, message);
}

@end
