using System;
using System.Collections.Generic;
using UnityEngine;

namespace ZapicSDK
{
    internal sealed class ZapicAndroidInterface : IZapicInterface
    {
        private ZapicPlayerAuthenticationHandler _authenticationHandler;

        internal ZapicAndroidInterface()
        {
            _authenticationHandler = null;
        }

        public Action<ZapicPlayer> OnLogin { get; set; }

        public Action<ZapicPlayer> OnLogout { get; set; }

        public void Start()
        {
            var unityPlayerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            var gameActivityObject = unityPlayerClass.GetStatic<AndroidJavaObject>("currentActivity");
            gameActivityObject.Call("runOnUiThread", new AndroidJavaRunnable(StartOnUI));
        }

        private void StartOnUI()
        {
            using(var zapicClass = new AndroidJavaClass("com.zapic.sdk.android.Zapic"))
            {
                using(var unityPlayerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
                using(var gameActivityObject = unityPlayerClass.GetStatic<AndroidJavaObject>("currentActivity"))
                {
                    var methodId = AndroidJNI.GetStaticMethodID(
                        zapicClass.GetRawClass(),
                        "attachFragment",
                        "(Landroid/app/Activity;)V");
                    var objectArray = new object[1];
                    var argArray = AndroidJNIHelper.CreateJNIArgArray(objectArray);
                    try
                    {
                        argArray[0].l = gameActivityObject.GetRawObject();
                        AndroidJNI.CallStaticVoidMethod(zapicClass.GetRawClass(), methodId, argArray);
                    }
                    finally
                    {
                        AndroidJNIHelper.DeleteJNIArgArray(objectArray, argArray);
                    }
                }

                if (_authenticationHandler == null)
                {
                    _authenticationHandler = new ZapicPlayerAuthenticationHandler(this);
                    var methodId = AndroidJNI.GetStaticMethodID(
                        zapicClass.GetRawClass(),
                        "setPlayerAuthenticationHandler",
                        "(Lcom/zapic/sdk/android/ZapicPlayerAuthenticationHandler;)V");
                    var objectArray = new object[1];
                    var argArray = AndroidJNIHelper.CreateJNIArgArray(objectArray);
                    try
                    {
                        argArray[0].l = AndroidJNIHelper.CreateJavaProxy(_authenticationHandler);
                        AndroidJNI.CallStaticVoidMethod(zapicClass.GetRawClass(), methodId, argArray);
                    }
                    finally
                    {
                        AndroidJNIHelper.DeleteJNIArgArray(objectArray, argArray);
                    }
                }
            }
        }

        public void ShowDefaultPage()
        {
            using(var unityPlayerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            using(var gameActivityObject = unityPlayerClass.GetStatic<AndroidJavaObject>("currentActivity"))
            using(var zapicClass = new AndroidJavaClass("com.zapic.sdk.android.Zapic"))
            {
                var methodId = AndroidJNI.GetStaticMethodID(
                    zapicClass.GetRawClass(),
                    "showDefaultPage",
                    "(Landroid/app/Activity;)V");
                var objectArray = new object[1];
                var argArray = AndroidJNIHelper.CreateJNIArgArray(objectArray);
                try
                {
                    argArray[0].l = gameActivityObject.GetRawObject();
                    AndroidJNI.CallStaticVoidMethod(zapicClass.GetRawClass(), methodId, argArray);
                }
                finally
                {
                    AndroidJNIHelper.DeleteJNIArgArray(objectArray, argArray);
                }
            }
        }

        public void ShowPage(ZapicPages page)
        {
            using(var unityPlayerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            using(var gameActivityObject = unityPlayerClass.GetStatic<AndroidJavaObject>("currentActivity"))
            using(var pageObject = new AndroidJavaObject("java.lang.String", page.ToString().ToLower()))
            using(var zapicClass = new AndroidJavaClass("com.zapic.sdk.android.Zapic"))
            {
                var methodId = AndroidJNI.GetStaticMethodID(
                    zapicClass.GetRawClass(),
                    "showPage",
                    "(Landroid/app/Activity;Ljava/lang/String;)V");
                var objectArray = new object[2];
                var argArray = AndroidJNIHelper.CreateJNIArgArray(objectArray);
                try
                {
                    argArray[0].l = gameActivityObject.GetRawObject();
                    argArray[1].l = pageObject.GetRawObject();
                    AndroidJNI.CallStaticVoidMethod(zapicClass.GetRawClass(), methodId, argArray);
                }
                finally
                {
                    AndroidJNIHelper.DeleteJNIArgArray(objectArray, argArray);
                }
            }
        }

        public ZapicPlayer Player()
        {
            var playerPointer = IntPtr.Zero;
            using(var zapicClass = new AndroidJavaClass("com.zapic.sdk.android.Zapic"))
            {
                var methodId = AndroidJNI.GetStaticMethodID(
                    zapicClass.GetRawClass(),
                    "getCurrentPlayer",
                    "()Lcom/zapic/sdk/android/ZapicPlayer;");
                var objectArray = new object[0];
                var argArray = AndroidJNIHelper.CreateJNIArgArray(objectArray);
                try
                {
                    playerPointer = AndroidJNI.CallStaticObjectMethod(zapicClass.GetRawClass(), methodId, argArray);
                }
                catch (Exception e)
                {
                    if (e.Message.Contains("null"))
                    {
                        playerPointer = IntPtr.Zero;
                    }
                    else
                    {
                        throw;
                    }
                }
                finally
                {
                    AndroidJNIHelper.DeleteJNIArgArray(objectArray, argArray);
                }
            }

            var player = ConvertPlayer(playerPointer);
            return player;
        }

        public void HandleInteraction(Dictionary<string, object> data)
        {
            var json = MiniJSON.Json.Serialize(data);

            using(var parametersObject = new AndroidJavaObject("java.lang.String", json))
            using(var zapicClass = new AndroidJavaClass("com.zapic.sdk.android.Zapic"))
            {
                var methodId = AndroidJNI.GetStaticMethodID(
                    zapicClass.GetRawClass(),
                    "handleInteraction",
                    "(Ljava/lang/String;)V");
                var objectArray = new object[1];
                var argArray = AndroidJNIHelper.CreateJNIArgArray(objectArray);
                try
                {
                    argArray[0].l = parametersObject.GetRawObject();
                    AndroidJNI.CallStaticVoidMethod(zapicClass.GetRawClass(), methodId, argArray);
                }
                finally
                {
                    AndroidJNIHelper.DeleteJNIArgArray(objectArray, argArray);
                }
            }
        }

        public void SubmitEvent(Dictionary<string, object> param)
        {
            var json = MiniJSON.Json.Serialize(param);

            using(var parametersObject = new AndroidJavaObject("java.lang.String", json))
            using(var zapicClass = new AndroidJavaClass("com.zapic.sdk.android.Zapic"))
            {
                var methodId = AndroidJNI.GetStaticMethodID(
                    zapicClass.GetRawClass(),
                    "submitEvent",
                    "(Ljava/lang/String;)V");
                var objectArray = new object[1];
                var argArray = AndroidJNIHelper.CreateJNIArgArray(objectArray);
                try
                {
                    argArray[0].l = parametersObject.GetRawObject();
                    AndroidJNI.CallStaticVoidMethod(zapicClass.GetRawClass(), methodId, argArray);
                }
                finally
                {
                    AndroidJNIHelper.DeleteJNIArgArray(objectArray, argArray);
                }
            }
        }

        private static ZapicPlayer ConvertPlayer(IntPtr playerPointer)
        {
            if (playerPointer == IntPtr.Zero)
            {
                return null;
            }

            string notificationToken;
            string playerId;
            using(var zapicPlayerClass = new AndroidJavaClass("com.zapic.sdk.android.ZapicPlayer"))
            {
                var methodId = AndroidJNI.GetMethodID(
                    zapicPlayerClass.GetRawClass(),
                    "getNotificationToken",
                    "()Ljava/lang/String;");
                var objectArray = new object[0];
                var argArray = AndroidJNIHelper.CreateJNIArgArray(objectArray);
                try
                {
                    notificationToken = AndroidJNI.CallStringMethod(playerPointer, methodId, argArray);
                }
                finally
                {
                    AndroidJNIHelper.DeleteJNIArgArray(objectArray, argArray);
                }

                methodId = AndroidJNI.GetMethodID(
                    zapicPlayerClass.GetRawClass(),
                    "getPlayerId",
                    "()Ljava/lang/String;");
                objectArray = new object[0];
                argArray = AndroidJNIHelper.CreateJNIArgArray(objectArray);
                try
                {
                    playerId = AndroidJNI.CallStringMethod(playerPointer, methodId, argArray);
                }
                finally
                {
                    AndroidJNIHelper.DeleteJNIArgArray(objectArray, argArray);
                }
            }

            //TODO: Kyle, use ZapicPlayer constructor
            // return new ZapicPlayer
            // {
            //     NotificationToken = notificationToken ?? string.Empty,
            //     PlayerId = playerId ?? string.Empty,
            // };
            return null;
        }

        public void GetCompetitions(Action<ZapicCompetition[], ZapicError> callback)
        {
            //TODO:Kyle
            throw new NotImplementedException();
        }

        public void GetStatistics(Action<ZapicStatistic[], ZapicError> callback)
        {
            //TODO:Kyle
            throw new NotImplementedException();
        }

        public void GetChallenges(Action<ZapicChallenge[], ZapicError> callback)
        {
            //TODO:Kyle
            throw new NotImplementedException();
        }

        public void GetPlayer(Action<ZapicPlayer, ZapicError> callback)
        {
            //TODO:Kyle
            throw new NotImplementedException();
        }

        private sealed class ZapicPlayerAuthenticationHandler : AndroidJavaProxy, IDisposable
        {
            private IZapicInterface _zapicInterface;

            public ZapicPlayerAuthenticationHandler(IZapicInterface zapicInterface) : base("com.zapic.sdk.android.ZapicPlayerAuthenticationHandler")
            {
                _zapicInterface = zapicInterface;
            }

            public void Dispose()
            {
                javaInterface.Dispose();
            }

            public void onLogin(AndroidJavaObject androidPlayer)
            {
                var handler = _zapicInterface.OnLogin;
                if (handler == null)
                {
                    return;
                }

                var player = ConvertPlayer(androidPlayer.GetRawObject());
                handler(player);
            }

            public void onLogout(AndroidJavaObject androidPlayer)
            {
                var handler = _zapicInterface.OnLogin;
                if (handler == null)
                {
                    return;
                }

                var player = ConvertPlayer(androidPlayer.GetRawObject());
                handler(player);
            }
        }
    }
}