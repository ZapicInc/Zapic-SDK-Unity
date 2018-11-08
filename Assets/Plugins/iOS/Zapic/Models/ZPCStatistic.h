#import <Foundation/Foundation.h>

@interface ZPCStatistic : NSObject
/**
 The unique id for the statistic.
 */
@property (nonnull, readonly) NSString *identifier;

/**
 The title for the statistic.
 */
@property (nullable, readonly) NSString *title;

/**
 The developer defined metadata for this statistic.
 */
@property (nullable, readonly) NSString *metadata;

/**
 The current player's score, formatted as defined in the portal.
 */
@property (nullable, readonly) NSString *formattedScore;

/**
 The current player's score.
 */
@property (nullable, readonly) NSNumber *score;

/**
 The current player's percentile rank
 */
@property (nullable, readonly) NSNumber *percentile;

/**
 Initialize a new statistic with json data

 @param data Data
 @return The new instance.
 */
- (instancetype)initWithData:(nonnull NSDictionary *)data;

/**
 Decode a collection of json data.

 @param data Array of statistic data.
 @return The new statistics.
 */
+ (NSArray<ZPCStatistic *> *)decodeList:(nonnull NSArray *)data;

@end
