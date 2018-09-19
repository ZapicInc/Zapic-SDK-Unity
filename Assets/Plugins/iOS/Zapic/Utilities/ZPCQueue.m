#import "ZPCQueue.h"

@implementation ZPCQueue

- (NSUInteger)count {
    return _data.count;
}

- (instancetype)init {
    if (self = [super init]) {
        _data = [[NSMutableArray alloc] init];
    }
    return self;
}

- (void)enqueueMany:(NSArray *)source {
    [self.data addObjectsFromArray:source];
}

- (void)enqueue:(id)anObject {
    [self.data addObject:anObject];
}

- (id)dequeue {
    id headObject = [self.data objectAtIndex:0];
    if (headObject != nil) {
        [self.data removeObjectAtIndex:0];
    }
    return headObject;
}

@end
