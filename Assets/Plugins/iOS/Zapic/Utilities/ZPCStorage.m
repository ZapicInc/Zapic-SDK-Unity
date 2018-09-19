#import "ZPCStorage.h"
#import "ZPCLog.h"

@implementation ZPCStorage

NSString *const filename = @"zapic-events.json";

- (void)store:(NSArray<NSString *> *)objects {
    NSError *error = nil;
    NSData *json = [NSJSONSerialization dataWithJSONObject:objects options:kNilOptions error:&error];

    NSURL *url = [[self getUrl] URLByAppendingPathComponent:filename];

    //Delete the existing file
    if ([NSFileManager.defaultManager fileExistsAtPath:url.path]) {
        [NSFileManager.defaultManager removeItemAtURL:url error:&error];
    }

    [NSFileManager.defaultManager createFileAtPath:url.path contents:json attributes:nil];
}

- (NSArray<NSString *> *)retrieve {
    NSURL *url = [[self getUrl] URLByAppendingPathComponent:filename];

    if (![NSFileManager.defaultManager fileExistsAtPath:url.path]) {
        return nil;
    }
    NSError *error = nil;
    NSData *data = [NSFileManager.defaultManager contentsAtPath:url.path];
    NSArray<NSString *> *json = [NSJSONSerialization JSONObjectWithData:data options:kNilOptions error:&error];

    if (![NSFileManager.defaultManager removeItemAtPath:url.path error:&error]) {
        [ZPCLog warn:@"Unable to remove file at %@", url.path];
    }
    return json;
}

- (void)clear {
    NSURL *url = [[self getUrl] URLByAppendingPathComponent:filename];
    NSError *error = nil;
    if (![NSFileManager.defaultManager removeItemAtPath:url.path error:&error]) {
        [ZPCLog warn:@"Unable to clear file at %@", url.path];
    }
}

- (NSURL *)getUrl {
    NSString *documentsPath = [NSSearchPathForDirectoriesInDomains(NSDocumentDirectory, NSUserDomainMask, YES) firstObject];

    return [NSURL URLWithString:documentsPath];
}

@end
