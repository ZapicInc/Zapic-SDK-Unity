#import <Foundation/Foundation.h>

@interface ZPCStorage : NSObject
- (void)store:(NSArray<NSString *> *)objects;
- (NSArray<NSString *> *)retrieve;
- (void)clear;
@end
