#import "ZPCChallenge.h"
#import "ZPCLog.h"
#import "ZPCUtils.h"

@implementation ZPCChallenge

- (instancetype)initWithData:(nonnull NSDictionary *)data {
    if (self = [super init]) {
        _identifier = data[@"id"];
        _title = data[@"title"];
        _active = [data[@"active"] boolValue];
        _textDescription = data[@"description"];
        _start = [ZPCUtils parseDateIso:data[@"start"]];
        _end = [ZPCUtils parseDateIso:data[@"end"]];
        _formattedScore = data[@"formattedScore"];
        _score = data[@"score"];
        _status = [ZPCChallenge stringToStatus:data[@"status"]];
        _metadata = data[@"metadata"];
        _rank = data[@"rank"];
        _totalUsers = data[@"totalUsers"];
    }
    return self;
}

+ (NSArray<ZPCChallenge *> *)decodeList:(nonnull NSArray *)data {
    NSMutableArray<ZPCChallenge *> *challenges = [NSMutableArray arrayWithCapacity:data.count];

    for (id compData in data) {
        ZPCChallenge *challenge = [[ZPCChallenge alloc] initWithData:compData];
        [challenges addObject:challenge];
    }

    return challenges;
}

+ (NSString *)statusToString:(ZPCChallengeStatus)status {
    switch (status) {
        case ZPCChallengeStatusIgnored:
            return @"ignored";
        case ZPCChallengeStatusAccepted:
            return @"ignored";
        default:
            return @"invited";
    }
}
+ (ZPCChallengeStatus)stringToStatus:(NSString *)string {
    if ([string isEqualToString:@"invited"]) {
        return ZPCChallengeStatusInvited;
    } else if ([string isEqualToString:@"accepted"]) {
        return ZPCChallengeStatusAccepted;
    } else if ([string isEqualToString:@"ignored"]) {
        return ZPCChallengeStatusIgnored;
    } else {
        [ZPCLog error:@"Unknown challenge status %@", string];
        return ZPCChallengeStatusInvited;
    }
}

@end
