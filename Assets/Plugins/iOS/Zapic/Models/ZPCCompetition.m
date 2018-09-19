#import "ZPCCompetition.h"
#import "ZPCLog.h"
#import "ZPCUtils.h"

@implementation ZPCCompetition

- (instancetype)initWithId:(nonnull NSString *)identifier
                     title:(nullable NSString *)title
                      text:(nullable NSString *)text
                  metadata:(nullable NSString *)metadata
                    active:(BOOL)active
                     start:(nullable NSDate *)start
                       end:(nullable NSDate *)end
                totalUsers:(nullable NSNumber *)totalUsers
                    status:(ZPCCompetitionStatus)status
            formattedScore:(nullable NSString *)formattedScore
                     score:(nullable NSNumber *)score
           leaderboardRank:(nullable NSNumber *)leaderboardRank
                leagueRank:(nullable NSNumber *)leagueRank {
    if (self = [super init]) {
        _identifier = identifier;
        _title = title;
        _text = text;
        _metadata = metadata;
        _active = active;
        _start = start;
        _end = end;
        _totalUsers = totalUsers;
        _status = status;
        _formattedScore = formattedScore;
        _score = score;
        _leaderboardRank = leaderboardRank;
        _leagueRank = leagueRank;
    }
    return self;
}

- (instancetype)initWithData:(NSDictionary *)data {
    return [self initWithId:data[@"id"]
                      title:data[@"title"]
                       text:data[@"description"]
                   metadata:data[@"metadata"]
                     active:[data[@"active"] boolValue]
                      start:[ZPCUtils parseDateIso:data[@"start"]]
                        end:[ZPCUtils parseDateIso:data[@"end"]]
                 totalUsers:data[@"totalUsers"]
                     status:[ZPCCompetition stringToStatus:data[@"status"]]
             formattedScore:data[@"formattedScore"]
                      score:data[@"score"]
            leaderboardRank:data[@"leaderboardRank"]
                 leagueRank:data[@"leagueRank"]];
}

+ (NSArray<ZPCCompetition *> *)decodeList:(NSArray<NSDictionary *> *)data {
    NSMutableArray<ZPCCompetition *> *competitions = [NSMutableArray arrayWithCapacity:data.count];

    for (id compData in data) {
        ZPCCompetition *competition = [[ZPCCompetition alloc] initWithData:compData];
        [competitions addObject:competition];
    }

    return competitions;
}

+ (NSString *)statusToString:(ZPCCompetitionStatus)status {
    switch (status) {
        case ZPCCompetitionStatusIgnored:
            return @"ignored";
        case ZPCCompetitionStatusAccepted:
            return @"ignored";
        default:
            return @"invited";
    }
}
+ (ZPCCompetitionStatus)stringToStatus:(NSString *)string {
    if ([string isEqualToString:@"invited"]) {
        return ZPCCompetitionStatusInvited;
    } else if ([string isEqualToString:@"accepted"]) {
        return ZPCCompetitionStatusAccepted;
    } else if ([string isEqualToString:@"ignored"]) {
        return ZPCCompetitionStatusIgnored;
    } else {
        [ZPCLog error:@"Unknown competition status %@", string];
        return ZPCCompetitionStatusInvited;
    }
}

@end
