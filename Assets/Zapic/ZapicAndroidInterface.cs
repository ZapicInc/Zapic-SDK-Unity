using System;
using System.Collections.Generic;
using UnityEngine;

namespace ZapicSDK
{
    internal sealed class ZapicAndroidInterface : IZapicInterface
    {
        public void Start()
        {
            using (var unityPlayerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            using (var gameActivityObject = unityPlayerClass.GetStatic<AndroidJavaObject>("currentActivity"))
            using (var zapicClass = new AndroidJavaClass("com.zapic.sdk.android.Zapic"))
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

        public void Show(Views view)
        {
            using (var unityPlayerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            using (var gameActivityObject = unityPlayerClass.GetStatic<AndroidJavaObject>("currentActivity"))
            using (var viewObject = new AndroidJavaObject("java.lang.String", view.ToString().ToLower()))
            using (var zapicClass = new AndroidJavaClass("com.zapic.sdk.android.Zapic"))
            {
                var methodId = AndroidJNI.GetStaticMethodID(
                    zapicClass.GetRawClass(),
                    "show",
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

        public string PlayerId()
        {
            string playerId;
            using (var zapicClass = new AndroidJavaClass("com.zapic.sdk.android.Zapic"))
            {
                var methodId = AndroidJNI.GetStaticMethodID(
                    zapicClass.GetRawClass(),
                    "getPlayerId",
                    "(Landroid/app/Activity;Ljava/lang/String;)V");
                var objectArray = new object[0];
                var argArray = AndroidJNIHelper.CreateJNIArgArray(objectArray);
                try
                {
                    playerId = AndroidJNI.CallStaticStringMethod(zapicClass.GetRawClass(), methodId, argArray);
                }
                finally
                {
                    AndroidJNIHelper.DeleteJNIArgArray(objectArray, argArray);
                }
            }

            return playerId;
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
    }
}