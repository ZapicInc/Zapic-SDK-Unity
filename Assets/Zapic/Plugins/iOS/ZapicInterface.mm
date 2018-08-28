#import <Foundation/Foundation.h>
#import "Zapic.h"

// Converts C style string to NSString
NSString* CreateNSString (const char* string)
{
  if (string)
    return [NSString stringWithUTF8String: string];
  else
    return [NSString stringWithUTF8String: ""];
}

// Helper method to create C string copy
char* MakeStringCopy (const char* string)
{
  if (string == NULL)
    return NULL;
  
  char* res = (char*)malloc(strlen(string) + 1);
  strcpy(res, string);
  return res;
}

char* serializePlayer(ZPlayer *player)
{
  if(player == NULL)
    return NULL;
  NSDictionary *playerDict = @{
                               @"playerId": player.playerId,
                               @"notificationToken": player.notificationToken
                               };;
  NSData *jsonData = [NSJSONSerialization dataWithJSONObject:playerDict options:0 error:nil];
  NSString *json = [[NSString alloc] initWithData:jsonData encoding:NSUTF8StringEncoding];
  
  return MakeStringCopy([json UTF8String]);
}

extern "C" {
  
  /// Player call back type
  typedef void (*ZAPIC_LOGIN_CALLBACK)(const char *);
  typedef void (*ZAPIC_LOGOUT_CALLBACK)(const char *);
  
  void z_start(){
    [Zapic start];
  }
  
  void z_show(char* pageName){
    [Zapic showPage:CreateNSString(pageName)];
  }
  
  void z_showDefault(){
    [Zapic showDefaultPage];
  }
  
  void z_submitEventWithParams(char* data){
    //Convert the data to a string
    NSString* json = CreateNSString(data);
    NSError* error;
    //Deserialize the string into a dictionary
    NSData *jData = [json dataUsingEncoding:NSUTF8StringEncoding];
    NSDictionary *dict = [NSJSONSerialization JSONObjectWithData:jData
                                                                 options:kNilOptions
                                                                   error:&error];

    //Sumbit the event
    [Zapic submitEvent:dict];
  }
  
  /// Returns the unique player as json
  const char* z_player(){
    return serializePlayer([Zapic player]);
  }
  
  /// Sets the login handler
  void z_setLoginHandler(ZAPIC_LOGIN_CALLBACK callback){
    Zapic.loginHandler = ^(ZPlayer *p) {
       callback(serializePlayer(p));
    };
  }
  
  /// Sets the logout handler
  void z_setLogoutHandler(ZAPIC_LOGOUT_CALLBACK callback){
    Zapic.logoutHandler = ^(ZPlayer *p) {
      callback(serializePlayer(p));
    };
  }
  
  /// Handle data provided by Zapic to an external source (push notification, deep link...)
  void z_handleInteraction(char* data){
    //Convert the data to a string
    NSString* json = CreateNSString(data);
    
    [Zapic handleInteractionString:json];
  }
}
