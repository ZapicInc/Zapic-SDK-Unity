#import <Foundation/Foundation.h>
#import "Zapic.h"
#import "ZPCWebApp.h"
#import "ZPCLog.h"
#import "ZPCUtils.h"

typedef struct
{
  char* identifier;
  char* name;
  char* url;
  char* notificationToken;
} ZPCUPlayer;

typedef struct
{
  char* identifier;
  char* title;
  char* description;
  char* metadata;
  BOOL active;
  char* start;
  char* end;
  int totalUsers;
  int status;
  char* formattedScore;
  BOOL hasScore;
  double score;
  BOOL hasLeaderboardRank;
  int leaderboardRank;
  BOOL hasLeagueRank;
  int leagueRank;
} ZPCUCompetition;

typedef struct
{
  char* identifier;
  char* title;
  BOOL active;
  char* description;
  char* metadata;
  char* start;
  char* end;
  int totalUsers;
  int status;
  char* formattedScore;
  BOOL hasScore;
  double score;
  BOOL hasRank;
  int rank;
} ZPCUChallenge;

typedef struct
{
  char* identifier;
  char* title;
  char* metadata;
  char* formattedScore;
  BOOL hasScore;
  double score;
  BOOL hasPercentile;
  int percentile;
} ZPCUStatistic;

typedef struct
{
  int code;
  char* message;
} ZPCUError;

/// Converts C style string to NSString
NSString* fromCString (const char* string)
{
  if (string)
    return [NSString stringWithUTF8String: string];
  else
    return [NSString stringWithUTF8String: ""];
}

/// Converts an NSString to a C string
char* toCString(NSString *string){
  if(string == nil || [string isEqual:[NSNull null]])
    return NULL;
  
  const char *str = [string UTF8String];
  
  if (str == NULL)
    return NULL;
  
  char* res = (char*)malloc(strlen(str) + 1);
  strcpy(res, str);
  return res;
}

ZPCUError emptyError(){
  ZPCUError e;
  e.code = 0;
  e.message = toCString(@"");
  return e;
}

NSDictionary* deserilizeJson(const char* string){
  //Convert the data to a string
  NSString* json = fromCString(string);
  NSError* error;
  //Deserialize the string into a dictionary
  NSData *jData = [json dataUsingEncoding:NSUTF8StringEncoding];
  NSDictionary *dict = [NSJSONSerialization JSONObjectWithData:jData
                                                       options:kNilOptions
                                                         error:&error];
  return dict;
}

ZPCUPlayer toUnityPlayer(ZPCPlayer *p){
  ZPCUPlayer player;
  player.identifier = toCString(p.identifier);
  player.name = toCString(p.name);
  player.url = toCString(p.iconUrl.absoluteString);
  player.notificationToken = toCString(p.notificationToken);
  return player;
}

BOOL hasValue(id object){
  return !(object == nil || [object isEqual:[NSNull null]]);
}

ZPCUCompetition toUnityCompetition(ZPCCompetition *c){
  ZPCUCompetition competition;
  competition.identifier = toCString(c.identifier);
  competition.title = toCString(c.title);
  competition.description = toCString(c.text);
  competition.metadata = toCString(c.metadata);
  competition.active = c.active;
  competition.start = toCString([ZPCUtils toIsoDate:c.start]);
  competition.end = toCString([ZPCUtils toIsoDate:c.end]);
  competition.totalUsers = [c.totalUsers intValue];
  competition.status = (int)c.status;
  competition.formattedScore = toCString(c.formattedScore);
  competition.hasScore = hasValue(c.score);
  competition.score = [c.score doubleValue];
  competition.hasLeaderboardRank = hasValue(c.leaderboardRank);
  competition.leaderboardRank = [c.leaderboardRank intValue];
  competition.hasLeagueRank = hasValue(c.leagueRank);
  competition.leagueRank = [c.leagueRank intValue];
  return competition;
}

ZPCUChallenge toUnityChallenge(ZPCChallenge *c){
  ZPCUChallenge challenge;
  challenge.identifier = toCString(c.identifier);
  challenge.title= toCString(c.title);
  challenge.description = toCString(c.textDescription);
  challenge.metadata = toCString(c.metadata);
  challenge.active = c.active;
  challenge.start = toCString([ZPCUtils toIsoDate:c.start]);
  challenge.end =toCString([ZPCUtils toIsoDate:c.end]);
  challenge.totalUsers = [c.totalUsers intValue];
  challenge.status = (int)c.status;
  challenge.formattedScore = toCString(c.formattedScore);
  challenge.hasScore = hasValue(c.score);
  challenge.score = [c.score doubleValue];
  challenge.hasRank = hasValue(c.rank);
  challenge.rank = [c.rank intValue];
  return challenge;
}

ZPCUStatistic toUnityStatistic(ZPCStatistic *c){
  ZPCUStatistic statistic;
  statistic.identifier = toCString(c.identifier);
  statistic.title=toCString(c.title);
  statistic.metadata = toCString(c.metadata);
  statistic.formattedScore=toCString(c.formattedScore);
  statistic.hasScore = hasValue(c.score);
  statistic.score = [c.score doubleValue];
  statistic.hasPercentile = hasValue(c.percentile);
  statistic.percentile = [c.percentile floatValue];
  return statistic;
}

ZPCUStatistic* toUnityStatistics(NSArray<ZPCStatistic *> *stats){
  ZPCUStatistic* array = new ZPCUStatistic[stats.count];
  int i = 0;
  for(id s in stats){
    array[i++]=toUnityStatistic(s);
  }
  return array;
}

ZPCUChallenge* toUnityChallenges(NSArray<ZPCChallenge *> *challenges){
  ZPCUChallenge* array = new ZPCUChallenge[challenges.count];
  int i = 0;
  for(id s in challenges){
    array[i++]=toUnityChallenge(s);
  }
  return array;
}

ZPCUCompetition* toUnityCompetitions(NSArray<ZPCCompetition *> *competitions){
  ZPCUCompetition* array = new ZPCUCompetition[competitions.count];
  int i = 0;
  for(id s in competitions){
    array[i++]=toUnityCompetition(s);
  }
  return array;
}

ZPCUError toUnityError(NSError *error){
  ZPCUError e;
  e.message = toCString([error localizedDescription]);
  e.code = (int)error.code;
  return e;
}

extern "C" {
  
#pragma mark - Callback definitions
  typedef void (*ZAPIC_LOGIN_CALLBACK)(ZPCUPlayer player);
  typedef void (*ZAPIC_LOGOUT_CALLBACK)(ZPCUPlayer player);
  typedef void (*ZAPIC_GET_RESPONSE_CALLBACK)(char* id, void* stats, int count, ZPCUError error);
  typedef void (*ZAPIC_GET_PLAYER_CALLBACK)(char* id, ZPCUPlayer player, ZPCUError error);
  
#pragma mark - API Methods
  void zpc_start(){
    [Zapic start];
  }
  
  void zpc_show(char* pageName){
    [Zapic showPage:fromCString(pageName)];
  }
  
  void zpc_showDefault(){
    [Zapic showDefaultPage];
  }
  
  void zpc_submitEventWithParams(char* data){
    NSDictionary *event = deserilizeJson(data);
    [Zapic submitEvent:event];
  }
  
  /// Sets the login handler
  void zpc_setLoginHandler(ZAPIC_LOGIN_CALLBACK callback){
    Zapic.loginHandler = ^(ZPCPlayer *p) {
      callback(toUnityPlayer(p));
    };
  }
  
  /// Sets the logout handler
  void zpc_setLogoutHandler(ZAPIC_LOGOUT_CALLBACK callback){
    Zapic.logoutHandler = ^(ZPCPlayer *p) {
      callback(toUnityPlayer(p));
    };
  }
  
  /// Handle data provided by Zapic to an external source (push notification, deep link...)
  void zpc_handleInteraction(char* data){
    NSDictionary *json = deserilizeJson(data);
    [Zapic handleInteractionData:json];
  }
  
  void zpc_getStatistics(char* reqId, ZAPIC_GET_RESPONSE_CALLBACK callback){
    NSString *i = fromCString(reqId);
    [Zapic getStatistics:^(NSArray<ZPCStatistic *> *statistics, NSError *error) {
      ZPCUError e;
      ZPCUStatistic* data;
      if(error){
        e = toUnityError(error);
      }else{
        e = emptyError();
        data = toUnityStatistics(statistics);
      }
      callback(toCString(i), data, (int)statistics.count, e);
    }];
  }
  
  void zpc_getChallenges(char* reqId, ZAPIC_GET_RESPONSE_CALLBACK callback){
    NSString *i = fromCString(reqId);
    [Zapic getChallenges:^(NSArray<ZPCChallenge *> *challenges, NSError *error) {
      ZPCUError e;
      ZPCUChallenge* data;
      if(error){
        e = toUnityError(error);
      }else{
        e = emptyError();
        data = toUnityChallenges(challenges);
      }
      callback(toCString(i), data, (int)challenges.count, e);
    }];
  }
  
  void zpc_getCompetitions(char* reqId, ZAPIC_GET_RESPONSE_CALLBACK callback){
    NSString *i = fromCString(reqId);
    [Zapic getCompetitions:^(NSArray<ZPCCompetition *> *competitions, NSError *error) {
      ZPCUError e;
      ZPCUCompetition* data;
      if(error){
        e = toUnityError(error);
      }else{
        e = emptyError();
        data = toUnityCompetitions(competitions);
      }
      callback(toCString(i), data, (int)competitions.count, e);
    }];
  }
  
  void zpc_getPlayer(char* reqId, ZAPIC_GET_PLAYER_CALLBACK callback){
    NSString *i = fromCString(reqId);
    [Zapic getPlayer:^(ZPCPlayer *player, NSError *error) {
      ZPCUError e;
      ZPCUPlayer p;
      if(error){
        e = toUnityError(error);
      }else{
        e = emptyError();
        p = toUnityPlayer(player);
      }
      callback(toCString(i),p, e);
    }];
  }
}
