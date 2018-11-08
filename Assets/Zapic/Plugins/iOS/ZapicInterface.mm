#import <Foundation/Foundation.h>
#import "Zapic.h"
#import "ZPCWebApp.h"
#import "ZPCLog.h"
#import "ZPCUtils.h"

typedef struct
{
  char* identifier;
  char* notificationToken;
} ZPCUPlayer;

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
  player.notificationToken = toCString(p.notificationToken);
  return player;
}

BOOL hasValue(id object){
  return !(object == nil || [object isEqual:[NSNull null]]);
}

extern "C" {
  
#pragma mark - Callback definitions
  typedef void (*ZAPIC_LOGIN_CALLBACK)(ZPCUPlayer player);
  typedef void (*ZAPIC_LOGOUT_CALLBACK)(ZPCUPlayer player);
  
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

  ZPCUPlayer zpc_player(){
    ZPCPlayer *p = [Zapic player];
    return toUnityPlayer(p);
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
}
