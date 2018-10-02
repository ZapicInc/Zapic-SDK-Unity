#if !UNITY_EDITOR && UNITY_ANDROID

using UnityEngine;

namespace ZapicSDK
{
    /// <summary>Provides static methods to enable or disable verbose SDK logging.</summary>
    internal static class ZapicAndroidLog
    {
        /// <summary>Disables verbose SDK logging.</summary>
        internal static void Disable()
        {
            using (var zapicLogClass = new AndroidJavaClass("com/zapic/sdk/android/ZapicLog"))
            {
                zapicLogClass.CallStatic("setLogLevel", 7);
            }
        }

        /// <summary>Enables verbose SDK logging.</summary>
        internal static void Enable()
        {
            using (var zapicLogClass = new AndroidJavaClass("com/zapic/sdk/android/ZapicLog"))
            {
                zapicLogClass.CallStatic("setLogLevel", 2);
            }
        }
    }
}

#endif
