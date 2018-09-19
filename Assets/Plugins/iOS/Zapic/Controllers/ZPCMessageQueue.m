#import "ZPCMessageQueue.h"
#import "ZPCLog.h"
#import "ZPCQueue.h"
#import "ZPCStorage.h"

@interface ZPCMessageQueue ()
@property (strong) ZPCQueue *queue;
@property (readonly) ZPCStorage *storage;
@property NSString *queuedPageEvent;
@property BOOL readyToSend;
@end

@implementation ZPCMessageQueue

- (instancetype)init {
    if (self = [super init]) {
        _queue = [[ZPCQueue alloc] init];
        _storage = [[ZPCStorage alloc] init];

        NSArray<NSString *> *savedItems = [_storage retrieve];

        if (savedItems) {
            [_queue enqueueMany:savedItems];
        }
    }
    return self;
}

- (void)sendMessage:(NSString *)function withPayload:(NSObject *)payload {
    [self sendMessage:function withPayload:payload isError:NO];
}

- (void)sendMessage:(NSString *)function withPayload:(NSObject *)payload isError:(BOOL)isError {
    [ZPCLog info:@"Dispatching JS event type %@", function];

    NSMutableDictionary *msg = [[NSMutableDictionary alloc] init];
    msg[@"type"] = function;
    msg[@"payload"] = payload;

    if (isError) {
        msg[@"error"] = @true;
    }

    NSData *jsonData = [NSJSONSerialization dataWithJSONObject:msg options:0 error:nil];
    NSString *json = [[NSString alloc] initWithData:jsonData encoding:NSUTF8StringEncoding];
    NSString *js = [[NSString alloc] initWithFormat:@"zapic.dispatch(%@);", json];

    //Send the message to the web app if it's ready
    if (_readyToSend) {
        [self runJavaScript:js];
    }
    //Queue up messages
    else {
        [ZPCLog info:@"Web client is not ready to run JS. Adding to queue"];

        if (function == ZPCWebFunctionOpenPage) {
            _queuedPageEvent = js;
        } else if (function == ZPCWebFunctionClosePage) {
            _queuedPageEvent = nil;
        } else {
            [_queue enqueue:js];

            if (_queue.count > 1000) {
                [_queue dequeue];
            }

            //Save the events to storage
            [_storage store:_queue.data];
        }
    }
}

- (void)sendQueuedMessages {
    _readyToSend = YES;

    //If there is a queued page, send it
    if (_queuedPageEvent) {
        [ZPCLog info:@"Resending page open request"];
        [self runJavaScript:_queuedPageEvent];
        _queuedPageEvent = nil;
    }

    [ZPCLog info:@"Starting to resend %lu events", (unsigned long)_queue.count];

    //Clears any stored events from disk
    [_storage clear];

    while (_queue.count > 0) {
        NSString *jsEvent = [_queue dequeue];

        [self runJavaScript:jsEvent];
    }

    [ZPCLog info:@"Done resending queued messages"];
}

- (void)runJavaScript:(NSString *)js {
    if (!js) {
        return;
    }
    [_webApp evaluateJavaScript:js];
}

@end
