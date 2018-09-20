#import "ZPCQueryManager.h"
#import "ZPCLog.h"

typedef void (^ResponseBlock)(id response, NSError *error);

@interface ZPCQueryManager ()
@property (readonly) ZPCScriptMessageHandler *messageHandler;
@property (readonly) ZPCMessageQueue *messageQueue;
@property (nonnull, readonly) NSMutableDictionary<NSString *, ResponseBlock> *requests;
@property (nonnull, readonly) NSMutableDictionary<NSString *, NSString *> *requestTypes;
@end

@implementation ZPCQueryManager
static NSString *const CompetitionQuery = @"competitions";
static NSString *const StatisticsQuery = @"statistics";
static NSString *const ChallengeQuery = @"challenges";
static NSString *const PlayerQuery = @"player";

static NSString *const ZPCErrorDomain = @"com.Zapic";
static NSInteger const ZPCErrorUnavailable = 2600;

- (void)setIsReady:(BOOL)isReady {
    if (isReady == _isReady) {
        return;
    }

    _isReady = isReady;

    //If the manager is not available, close all pending queries
    if (!_isReady) {
        [self failAllQueries];
    }
}

- (instancetype)initWithMessageHandler:(ZPCScriptMessageHandler *)messageHandler messageQueue:(ZPCMessageQueue *)messageQueue {
    if (self = [super init]) {
        _messageQueue = messageQueue;
        _messageHandler = messageHandler;
        _requests = [[NSMutableDictionary alloc] init];
        _requestTypes = [[NSMutableDictionary alloc] init];
        _isReady = YES;

        __weak ZPCQueryManager *weakSelf = self;

        [_messageHandler addQueryResponseHandler:^(NSDictionary *message) {
            [weakSelf handleResponse:message];
        }];
    }
    return self;
}

- (void)handleResponse:(NSDictionary *)data {
    NSDictionary *payload = data[@"payload"];
    BOOL error = [data[@"error"] boolValue];
    NSString *requestId = payload[@"requestId"];

    //Gets the callback for this request
    ResponseBlock handler = [_requests objectForKey:requestId];
    NSString *dataType = [_requestTypes objectForKey:requestId];

    if (!handler) {
        [ZPCLog warn:@"Unable to find handler for requestId: %@", requestId];
        return;
    }

    [_requests removeObjectForKey:requestId];
    [_requestTypes removeObjectForKey:requestId];

    //If this is an error response, trigger the callback right away
    if (error) {
        NSString *msg = payload[@"errorMessage"];
        id codeObj = payload[@"code"];
        NSError *error;
        if (codeObj == nil) {
            error = [NSError errorWithDomain:ZPCErrorDomain code:ZPCErrorUnavailable userInfo:@{@"errorMsg": @"Unknown error"}];
        } else {
            error = [NSError errorWithDomain:ZPCErrorDomain code:[codeObj intValue] userInfo:@{@"errorMsg": msg}];
        }

        handler(nil, error);
        return;
    }

    id response = nil;
    id responseData = payload[@"response"];

    if ([dataType isEqualToString:CompetitionQuery]) {
        response = [ZPCCompetition decodeList:responseData];
    } else if ([dataType isEqualToString:StatisticsQuery]) {
        response = [ZPCStatistic decodeList:responseData];
    } else if ([dataType isEqualToString:ChallengeQuery]) {
        response = [ZPCChallenge decodeList:responseData];
    } else if ([dataType isEqualToString:PlayerQuery]) {
        response = [[ZPCPlayer alloc] initWithData:responseData];
    }

    //Trigger the callback with the reponse data
    handler(response, nil);
}

- (void)sendQuery:(NSString *)dataType withCompletionHandler:(ResponseBlock)completionHandler {
    //If requests cant be processed now, cancel immediately
    if (!_isReady) {
        NSError *error = [NSError errorWithDomain:ZPCErrorDomain
                                             code:ZPCErrorUnavailable
                                         userInfo:nil];

        completionHandler(nil, error);
        return;
    }

    //Generate a new unique id
    NSString *requestId = [NSUUID UUID].UUIDString;

    //Save the callback for this request id
    [_requests setObject:completionHandler forKey:requestId];
    [_requestTypes setObject:dataType forKey:requestId];

    NSDictionary *msg = @{
        @"requestId": requestId,
        @"dataType": dataType,
        @"dataTypeVersion": @1
    };

    //Send the query to JS
    [_messageQueue sendMessage:ZPCWebFunctionQuery withPayload:msg];
}

- (void)failAllQueries {
    for (NSString *requestId in _requests) {
        NSError *error = [NSError errorWithDomain:ZPCErrorDomain
                                             code:ZPCErrorUnavailable
                                         userInfo:nil];

        //Gets the handler
        ResponseBlock handler = [_requests objectForKey:requestId];

        //Trigger the handler with the error
        handler(nil, error);
    }

    [_requests removeAllObjects];
    [_requestTypes removeAllObjects];
}

- (void)getCompetitions:(void (^)(NSArray<ZPCCompetition *> *competitions, NSError *error))completionHandler {
    [self sendQuery:CompetitionQuery withCompletionHandler:completionHandler];
}

- (void)getStatistics:(void (^)(NSArray<ZPCStatistic *> *statistics, NSError *error))completionHandler {
    [self sendQuery:StatisticsQuery withCompletionHandler:completionHandler];
}

- (void)getChallenges:(void (^)(NSArray<ZPCChallenge *> *statistics, NSError *error))completionHandler {
    [self sendQuery:ChallengeQuery withCompletionHandler:completionHandler];
}

- (void)getPlayer:(void (^)(ZPCPlayer *player, NSError *error))completionHandler {
    [self sendQuery:PlayerQuery withCompletionHandler:completionHandler];
}

@end
