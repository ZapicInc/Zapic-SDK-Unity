#import <Foundation/Foundation.h>

@interface ZPCQueue : NSObject
@property (strong) NSMutableArray *data;
@property (readonly) NSUInteger count;
- (void)enqueueMany:(NSArray *)source;
- (void)enqueue:(id)anObject;
- (id)dequeue;
@end
