using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace ZapicSDK
{
    internal interface IZapicInterface
    {
        /// <summary>
        /// Starts zapic. This should be called
        /// as soon as possible during app startup.
        /// </summary>
        /// <param name="version">App version id.</param>
        void Start(string version);

        /// <summary>
        /// Shows the given zapic window
        /// </summary>
        /// <param name="view">View to show.</param>
        void Show(Views view);

        /// <summary>
        /// Gets the current players unique id.
        /// </summary>
        /// <returns>The unique id.</returns>
        Guid? PlayerId();

        /// <summary>
        /// Submit a new in-game event to zapic.
        /// </summary>
        /// <param name="param">Collection of parameter names and associate values (numeric, string, bool)</param>
        void SubmitEvent(Dictionary<string, object> param);
    }

    internal sealed class ZapicEditorInterface : IZapicInterface
    {
        public void Start(string version)
        {
            LogZapicCall("Start");
        }

        public void Show(Views view)
        {
            LogZapicCall("Show");
        }

        public void SubmitEvent(Dictionary<string, object> param)
        {
            LogZapicCall("SubmitEvent");
        }

        public Guid? PlayerId()
        {
            LogZapicCall("GetPlayerId");
            return Guid.Empty;
        }

        private static void LogZapicCall(string method)
        {
            Debug.LogFormat("Zapic: Called {0}", method);
        }

    }

    internal sealed class ZapicAndroidInterface : IZapicInterface
    {
        public void Start(string version)
        {
            using (var unityPlayerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            using (var currentActivityObject = unityPlayerClass.GetStatic<AndroidJavaObject>("currentActivity"))
            using (var versionObject = new AndroidJavaObject("java.lang.String", version))
            using (var zapicFragmentClass = new AndroidJavaClass("com.zapic.android.zapiclibrary.ZapicFragment"))
            {
                IntPtr methodId = AndroidJNI.GetStaticMethodID(
                    zapicFragmentClass.GetRawClass(),
                    "startZapicFragment",
                    "(Landroid/app/Activity;Ljava/lang/String;)V");
                object[] objectArray = new object[2];
                jvalue[] argArray = AndroidJNIHelper.CreateJNIArgArray(objectArray);
                try
                {
                    argArray[0].l = currentActivityObject.GetRawObject();
                    argArray[1].l = versionObject.GetRawObject();
                    AndroidJNI.CallStaticVoidMethod(zapicFragmentClass.GetRawClass(), methodId, argArray);
                }
                finally
                {
                    AndroidJNIHelper.DeleteJNIArgArray(objectArray, argArray);
                }
            }
        }

        public void Show(Views view)
        {
            using (var viewObject = new AndroidJavaObject("java.lang.String", view.ToString().ToLower()))
            using (var zapicFragmentClass = new AndroidJavaClass("com.zapic.android.zapiclibrary.ZapicFragment"))
            {
                IntPtr methodId = AndroidJNI.GetStaticMethodID(
                    zapicFragmentClass.GetRawClass(),
                    "showZapicFragment",
                    "(Ljava/lang/String;)V");
                object[] objectArray = new object[1];
                jvalue[] argArray = AndroidJNIHelper.CreateJNIArgArray(objectArray);
                try
                {
                    argArray[0].l = viewObject.GetRawObject();
                    AndroidJNI.CallStaticVoidMethod(zapicFragmentClass.GetRawClass(), methodId, argArray);
                }
                finally
                {
                    AndroidJNIHelper.DeleteJNIArgArray(objectArray, argArray);
                }
            }
        }

        public Guid? PlayerId()
        {
            return null;
        }

        public void SubmitEvent(Dictionary<string, object> param)
        {
            var json = JsonUtility.ToJson(param);

            using (var paramObject = new AndroidJavaObject("java.lang.String", json))
            using (var zapicFragmentClass = new AndroidJavaClass("com.zapic.android.zapiclibrary.ZapicFragment"))
            {
                IntPtr methodId = AndroidJNI.GetStaticMethodID(
                    zapicFragmentClass.GetRawClass(),
                    "submitEventZapicFragment",
                    "(Ljava/lang/String;)V");
                object[] objectArray = new object[1];
                jvalue[] argArray = AndroidJNIHelper.CreateJNIArgArray(objectArray);
                try
                {
                    argArray[0].l = paramObject.GetRawObject();
                    AndroidJNI.CallStaticVoidMethod(zapicFragmentClass.GetRawClass(), methodId, argArray);
                }
                finally
                {
                    AndroidJNIHelper.DeleteJNIArgArray(objectArray, argArray);
                }
            }
        }
    }

    internal sealed class ZapiciOSInterface : IZapicInterface
    {
        #region DLLImports

        [DllImport("__Internal")]
        private static extern void z_start(string version);

        [DllImport("__Internal")]
        private static extern void z_show(string viewName);

        [DllImport("__Internal")]
        private static extern void z_submitEventWithParams(string eventJson);

        [DllImport("__Internal")]
        private static extern string z_playerId();

        #endregion

        /// <summary>
        /// Starts zapic. This should be called
        /// as soon as possible during app startup.
        /// </summary>
        /// <param name="version">App version id.</param>
        public void Start(string version)
        {
            z_start(version);
        }

        /// <summary>
        /// Shows the given zapic window
        /// </summary>
        /// <param name="view">View to show.</param>
        public void Show(Views view)
        {
            z_show(view.ToString().ToLower());
        }

        /// <summary>
        /// Gets the current players unique id.
        /// </summary>
        /// <returns>The unique id.</returns>
        public Guid? PlayerId()
        {
            var val = z_playerId();

            if (val == null)
                return null;

            var id = new Guid(val);

            return id;
        }

        /// <summary>
        /// Submit a new in-game event to zapic.
        /// </summary>
        /// <param name="param">Collection of parameter names and associate values (numeric, string, bool)</param>
        public void SubmitEvent(Dictionary<string, object> param)
        {
            var json = JsonUtility.ToJson(param);

            z_submitEventWithParams(json);
        }
    }
}