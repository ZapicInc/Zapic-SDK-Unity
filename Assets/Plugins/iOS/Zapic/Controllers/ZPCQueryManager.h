#import <Foundation/Foundation.h>
#import "ZPCChallenge.h"
#import "ZPCCompetition.h"
#import "ZPCMessageQueue.h"
#import "ZPCScriptMessageHandler.h"
#import "ZPCStatistic.h"

@interface ZPCQueryManager : NSObject

@property (nonatomic) BOOL isReady;

- (instancetype)initWithMessageHandler:(ZPCScriptMessageHandler *)messageHandler messageQueue:(ZPCMessageQueue *)messageQueue;

/**
 Gets the list of competitions.

 @param completionHandler Callback handler.
 */
- (void)getCompetitions:(void (^)(NSArray<ZPCCompetition *> *competitions, NSError *error))completionHandler;

/**
 Gets the list of competitions.
 
 @param completionHandler Callback handler.
 */
- (void)getStatistics:(void (^)(NSArray<ZPCStatistic *> *statistics, NSError *error))completionHandler;

/**
 Gets the list of challenges.
 
 @param completionHandler Callback handler.
 */
- (void)getChallenges:(void (^)(NSArray<ZPCChallenge *> *statistics, NSError *error))completionHandler;

/**
 Gets the player.
 
 @param completionHandler Callback handler.
 */
- (void)getPlayer:(void (^)(ZPCPlayer *player, NSError *error))completionHandler;
@end
