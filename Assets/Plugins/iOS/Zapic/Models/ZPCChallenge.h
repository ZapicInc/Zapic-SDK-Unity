#import <Foundation/Foundation.h>

typedef NS_ENUM(NSUInteger, ZPCChallengeStatus) {
    ZPCChallengeStatusInvited,
    ZPCChallengeStatusIgnored,
    ZPCChallengeStatusAccepted,
};

@interface ZPCChallenge : NSObject

/**
 The unique id for the statistic.
 */
@property (nonnull, readonly) NSString *identifier;

/**
 The title for the challenge.
 */
@property (nullable, readonly) NSString *title;

/**
 The flag indicating if the challenge is active.
 */
@property (readonly) BOOL active;

/**
 The description.
 */
@property (nullable, readonly) NSString *textDescription;

/**
 When the challenge started.
 */
@property (nullable, readonly) NSDate *start;

/**
 When the competition ends.
 */
@property (nullable, readonly) NSDate *end;

/**
 The current player's score, formatted as defined in the portal.
 */
@property (nullable, readonly) NSString *formattedScore;

/**
 The current player's score.
 */
@property (nullable, readonly) NSNumber *score;

/**
 The developer defined metadata.
 */
@property (nullable, readonly) NSString *metadata;

/**
 The player's rank on the leaderboard (ex. Top 100). If the player
 is not on the leaderboard this value will be nil.
 */
@property (nullable, readonly) NSNumber *rank;

/**
 The current player's status.
 */
@property (readonly) ZPCChallengeStatus status;

/**
The total number of players in the challenge.
 */
@property (nullable, readonly) NSNumber *totalUsers;

/**
 Initialize a new instance from json data
 
 @param data Data
 @return The new instance.
 */
- (instancetype)initWithData:(nonnull NSDictionary *)data;

/**
 Decode a collection of json data.
 
 @param data Array of json data.
 @return The new challenges.
 */
+ (NSArray<ZPCChallenge *> *)decodeList:(nonnull NSArray *)data;

/**
 Converts the status to a string.

 @param status The status.
 @return The string.
 */
+ (NSString *)statusToString:(ZPCChallengeStatus)status;

/**
 Parse a status string

 @param string The string.
 @return The status.
 */
+ (ZPCChallengeStatus)stringToStatus:(NSString *)string;

@end
