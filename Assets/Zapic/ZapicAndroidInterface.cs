using System;
using System.Collections.Generic;
using UnityEngine;

namespace ZapicSDK
{
    internal sealed class ZapicAndroidInterface : IZapicInterface
    {
        private bool _started;

        public Action<ZapicPlayer> OnLogin { get; set; }

        public Action<ZapicPlayer> OnLogout { get; set; }

        public ZapicAndroidInterface()
        {
            _started = false;
        }

        public void Start()
        {
            using (var zapicClass = new AndroidJavaClass("com.zapic.sdk.android.Zapic"))
            {
                if (!_started)
                {
                    _started = true;

                    var methodId = AndroidJNI.GetStaticMethodID(
                        zapicClass.GetRawClass(),
                        "start",
                        "(Lcom/zapic/sdk/android/Zapic$AuthenticationHandler;)V");
                    var objectArray = new object[1];
                    var argArray = AndroidJNIHelper.CreateJNIArgArray(objectArray);
                    try
                    {
                        argArray[0].l = AndroidJNIHelper.CreateJavaProxy(new AuthenticationHandler(this));
                        AndroidJNI.CallStaticVoidMethod(zapicClass.GetRawClass(), methodId, argArray);
                    }
                    finally
                    {
                        AndroidJNIHelper.DeleteJNIArgArray(objectArray, argArray);
                    }
                }

                using (var unityPlayerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
                using (var gameActivityObject = unityPlayerClass.GetStatic<AndroidJavaObject>("currentActivity"))
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
            }
        }

        public void ShowDefaultPage()
        {
            using (var unityPlayerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            using (var gameActivityObject = unityPlayerClass.GetStatic<AndroidJavaObject>("currentActivity"))
            using (var zapicClass = new AndroidJavaClass("com.zapic.sdk.android.Zapic"))
            {
                var methodId = AndroidJNI.GetStaticMethodID(
                    zapicClass.GetRawClass(),
                    "showDefaultPage",
                    "(Landroid/app/Activity;Ljava/lang/String;)V");
                var objectArray = new object[2];
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
            using (var unityPlayerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            using (var gameActivityObject = unityPlayerClass.GetStatic<AndroidJavaObject>("currentActivity"))
            using (var viewObject = new AndroidJavaObject("java.lang.String", page.ToString().ToLower()))
            using (var zapicClass = new AndroidJavaClass("com.zapic.sdk.android.Zapic"))
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
                    argArray[1].l = viewObject.GetRawObject();
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
            using (var zapicClass = new AndroidJavaClass("com.zapic.sdk.android.Zapic"))
            {
                var methodId = AndroidJNI.GetStaticMethodID(
                    zapicClass.GetRawClass(),
                    "getPlayer",
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

            using (var unityPlayerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            using (var gameActivityObject = unityPlayerClass.GetStatic<AndroidJavaObject>("currentActivity"))
            using (var jsonObject = new AndroidJavaObject("org.json.JSONObject", json))
            using (var zapicClass = new AndroidJavaClass("com.zapic.sdk.android.Zapic"))
            {
                var methodId = AndroidJNI.GetStaticMethodID(
                    zapicClass.GetRawClass(),
                    "handleInteraction",
                    "(Landroid/app/Activity;Lorg/json/JSONObject;)V");
                var objectArray = new object[2];
                var argArray = AndroidJNIHelper.CreateJNIArgArray(objectArray);
                try
                {
                    argArray[0].l = gameActivityObject.GetRawObject();
                    argArray[1].l = jsonObject.GetRawObject();
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

            using (var unityPlayerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            using (var gameActivityObject = unityPlayerClass.GetStatic<AndroidJavaObject>("currentActivity"))
            using (var parametersObject = new AndroidJavaObject("java.lang.String", json))
            using (var zapicClass = new AndroidJavaClass("com.zapic.sdk.android.Zapic"))
            {
                var methodId = AndroidJNI.GetStaticMethodID(
                    zapicClass.GetRawClass(),
                    "submitEvent",
                    "(Landroid/app/Activity;Ljava/lang/String;)V");
                var objectArray = new object[2];
                var argArray = AndroidJNIHelper.CreateJNIArgArray(objectArray);
                try
                {
                    argArray[0].l = gameActivityObject.GetRawObject();
                    argArray[1].l = parametersObject.GetRawObject();
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
            using (var zapicPlayerClass = new AndroidJavaClass("com.zapic.sdk.android.ZapicPlayer"))
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

            return new ZapicPlayer
            {
                NotificationToken = notificationToken ?? string.Empty,
                PlayerId = playerId ?? string.Empty,
            };
        }

        private sealed class AuthenticationHandler : AndroidJavaProxy, IDisposable
        {
            private IZapicInterface _zapicInterface;

            public AuthenticationHandler(IZapicInterface zapicInterface)
                : base("com.zapic.sdk.android.Zapic$AuthenticationHandler")
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
